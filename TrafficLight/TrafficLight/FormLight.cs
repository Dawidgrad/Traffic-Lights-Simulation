using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


//************************************************************************//
// This project makes an extremely simple traffic light.  Because of the  //
// personal firewall on the lab computers being switched on, this         //
// actually connects to a sort of proxy (running in my office) that       //
// accepts the incomming  connection.                                     //    
// By Nigel.                                                              //
//                                                                        //
// Please use this code, sich as it is,  for any eduactional or non       //
// profit making research porposes on the conditions that.                //
//                                                                        //
// 1.    You may only use it for educational and related research         //
//      pusposes.                                                         //
//                                                                        //
// 2.   You leave my name on it.                                          //
//                                                                        //
// 3.   You correct at least 10% of the typig and spekking mistskes.      //
//                                                                        //
// © Nigel Barlow nigel@soc.plymouth.ac.uk 2018                           //
//************************************************************************//

namespace TrafficLight
{
    public partial class FormTrafficLight : Form
    {
        public FormTrafficLight()
        {
            InitializeComponent();
        }


        //******************************************************//
        // Nigel Networking attributes.                         //
        //******************************************************//
        private int              serverPort       = 5000;
        private int              bufferSize       = 200;
        private TcpClient        socketClient     = null;
        private String           serverName       = "eeyore.fost.plymouth.ac.uk";  //A computer in my office.
        private NetworkStream    connectionStream = null;
        private BinaryReader     inStream         = null;
        private BinaryWriter     outStream        = null;
        private ThreadConnection threadConnection = null;


        //*******************************************************************//
        // This one is needed so that we can post messages back to the form's//
        // thread and don't violate C#'s threading rule that says you can    //
        // only touch the UI components from the form's thread.              //
        //*******************************************************************//
        SynchronizationContext uiContext = null;


        //*******************************************************************//
        // Client's attributes                                               //
        //*******************************************************************//
        private string uniqueIDNumber = null;
        private bool waitingToConnect = false;
        private Random randomGen = new Random();
    
        

        //*********************************************************************//
        // Form load.  Display an IP. Or a series of IPs.                      //                               
        //*********************************************************************//
        private void Form1_Load(object sender, EventArgs e)
        {
            //******************************************************************//
            //All this to find out IP number.                                   //
            //******************************************************************//
            IPHostEntry localHostInfo = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            listBoxOutput.Items.Add("You may have many IP numbers.");
            listBoxOutput.Items.Add("In the Plymouth labs, use the IP that looks like an IP4 number");
            listBoxOutput.Items.Add("something like 10.xx.xx.xx.");
            listBoxOutput.Items.Add("If at home using a VPN use the IP4 number that starts");
            listBoxOutput.Items.Add("something like 141.163.xx.xx");
            listBoxOutput.Items.Add(" ");


            foreach (IPAddress address in localHostInfo.AddressList)
                listBoxOutput.Items.Add(address.ToString());


            //******************************************************************//
            // Get the SynchronizationContext for the current thread (the form's//
            // thread).                                                         //
            //******************************************************************//
            uiContext = SynchronizationContext.Current;
            if (uiContext == null)
                listBoxOutput.Items.Add("No context for this thread");
            else
                listBoxOutput.Items.Add("We got a context");
        }



        //*********************************************************************//
        // Form closing.  If the connection thread was ever created then kill  //
        // it off.                                                             //                               
        //*********************************************************************//
        private void FormTrafficLight_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageProtocol disconncetionMessage = new MessageProtocol(uniqueIDNumber, "ClientDisconnecting", "-");
            sendString(disconncetionMessage.GetMessageToSend(), textBoxLightIP.Text);

            if (threadConnection != null) threadConnection.StopThread();
        }



        //*********************************************************************//
        // Message was posted back to us.  This is to get over the C# threading//
        // rules whereby we can only touch the UI components from the thread   //
        // that created them, which is the form's main thread.                 // 
        //*********************************************************************//
        public void MessageReceived(Object received)
        {
            String message = (String) received;
            listBoxOutput.Items.Add(DateTime.Now.ToString("h:mm:ss tt") + "  " + message);

            // Fit the received message to message protocol
            MessageProtocol receivedMessage = new MessageProtocol(message);

            // If the message is for this client and it is a light change request
            if (receivedMessage.GetID() == uniqueIDNumber && receivedMessage.GetActionType().Equals("ChangeLight"))
            {
                ChangeLights(receivedMessage);
            }
            // If the connection request is accepted and this client is waiting to connect
            else if (receivedMessage.GetActionType().Equals("ConnectionRequestAccepted") && waitingToConnect == true)
            {
                // Save the assigned ID number by the server
                uniqueIDNumber = receivedMessage.GetActionValue();
                idNumberLabel.Text = uniqueIDNumber;

                // Enable or disable some parts of UI
                buttonCarArrived.Enabled = true;
                randomSendCheckBox.Enabled = true;
                connectServerButton.Enabled = false;
                textBoxLightIP.Enabled = false;
                waitingToConnect = false;
            }
            // If the client receives message to disconnect from server
            else if (receivedMessage.GetActionType().Equals("ClientDisconnect"))
            {
                // Turn off the lights
                labelRed.Visible = false;
                labelAmber.Visible = false;
                labelGreen.Visible = false;

                // Reset ID number assigned to this client
                uniqueIDNumber = null;
                idNumberLabel.Text = "";

                // Enable or disable parts of UI
                buttonCarArrived.Enabled = false;
                connectServerButton.Enabled = true;
                textBoxLightIP.Enabled = true;
            }

        }


        //*********************************************************************//
        // Change the status of the lights.                                    //
        //*********************************************************************//
        private void ChangeLights(MessageProtocol command)
        {
            if (command == null) return;    // Nothing to do.
           
            // Determine which light to turn on/off
            if (command.GetActionValue().Equals("Red_On"))   labelRed.Visible   = true;
            if (command.GetActionValue().Equals("Amber_On")) labelAmber.Visible = true;
            if (command.GetActionValue().Equals("Green_On")) labelGreen.Visible = true;
            if (command.GetActionValue().Equals("Red_Off"))   labelRed.Visible = false;
            if (command.GetActionValue().Equals("Amber_Off")) labelAmber.Visible = false;
            if (command.GetActionValue().Equals("Green_Off")) labelGreen.Visible = false;
        }



        //*********************************************************************//
        // The OnClick for the "connect" command button. Create a new client   //
        // socket.   Much of this code is exception processing.                //
        //*********************************************************************//
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                socketClient = new TcpClient(serverName, serverPort);
            }
            catch (Exception ee)
            {
                listBoxOutput.Items.Add("Error in connecting to server");     //Console is a sealed object; we
                listBoxOutput.Items.Add(ee.Message);				 	      //can't make it, we can just access
                labelStatus.Text = "Error " + ee.Message;
                labelStatus.BackColor = Color.Red;
            }

            if (socketClient == null)
            {
                listBoxOutput.Items.Add("Socket not connected");

            }
            else
            {

                //**************************************************//
                // Make some streams.  They have rather more        //
                // capabilities than just a socket.  With this type //
                // of socket, we can't read from it and write to it //
                // directly.                                        //
                //**************************************************//
                connectionStream = socketClient.GetStream();
                inStream  = new BinaryReader(connectionStream);
                outStream = new BinaryWriter(connectionStream);

                listBoxOutput.Items.Add("Socket connected to " + serverName);

                labelStatus.BackColor = Color.Green;
                labelStatus.Text = "Connected to " + serverName;


                //**********************************************************//
                // Discale connect button (we can only connect once) and    //
                // enable other components.                                 //
                //**********************************************************//
                buttonConnect.Enabled    = false;
                textBoxLightIP.Enabled = true;
                connectServerButton.Enabled = true;


                //***********************************************************//
                //We have now accepted a connection.                         //
                //                                                           //
                //There are several ways to do this next bit.   Here I make a//
                //network stream and use it to create two other streams, an  //
                //input and an output stream.   Life gets easier at that     //
                //point.                                                     //
                //***********************************************************//
                threadConnection = new ThreadConnection(uiContext, socketClient, this);


                //***********************************************************//
                // Create a new Thread to manage the connection that receives//
                // data.  If you are a Java programmer, this looks like a    //
                // load of hokum cokum..                                     //
                //***********************************************************//
                Thread threadRunner = new Thread(new ThreadStart(threadConnection.Run));
                threadRunner.Start();

                Console.WriteLine("Created new connection class");
            }
        }




        //**********************************************************************//
        // Button cluck for the car arrived button.  All it does is send the    //
        // string "Car" to the server.                                          //
        //**********************************************************************//
        private void buttonCarArrived_Click(object sender, EventArgs e)
        {
            // Create a message to send to server
            MessageProtocol sendCarMessage = new MessageProtocol(uniqueIDNumber, "CarSent", "-");
            sendString(sendCarMessage.GetMessageToSend(), textBoxLightIP.Text);
        }



        //**********************************************************************//
        // Send a string to the IP you give.  The string and IP are bundled up  //
        // into one of there rather quirky Nigel style packets.                 // 
        //                                                                      //
        // This uses the pre-defined stream outStream.  If this strean doesn't  //
        // exist then this method will bomb.                                    //
        //                                                                      //
        // It also does the networking synchronously, in the form's main        //
        // Thread.  This is not good practise; all networking should really be  //
        // asynchronous.                                                        //
        //**********************************************************************//
        private void sendString(String stringToSend, String sendToIP)
        {
            try
            {
                byte[] packet = new byte[bufferSize];
                String[] ipStrings = sendToIP.Split('.'); //Split with . as separator

                packet[0] = Byte.Parse(ipStrings[0]);
                packet[1] = Byte.Parse(ipStrings[1]);   //Think about this.  It assumes the user
                packet[2] = Byte.Parse(ipStrings[2]);   //has entered the IP corrrectly, and 
                packet[3] = Byte.Parse(ipStrings[3]);   //sends the numbers without the bytes.

                int bufferIndex = 4;                    //Start assembling message

                //**************************************************************//
                // Turn the string into an array of characters.                 //
                //**************************************************************//
                int length = stringToSend.Length;
                char[] chars = stringToSend.ToCharArray();
                

                //**************************************************************//
                // Then turn each character into a byte and copy into my packet.//
                //**************************************************************//
                for (int i = 0; i < length; i++)
                {
                    byte b = (byte)chars[i];
                    packet[bufferIndex] = b;
                    bufferIndex++;
                }

                packet[bufferIndex] = 0;    //End of packet (even though it is always 200 bytes)

                outStream.Write(packet, 0, bufferSize);
                listBoxOutput.Items.Add("Sent " + stringToSend);
            }
            catch (Exception doh)
            {
                listBoxOutput.Items.Add("An error occurred: " + doh.Message);
            }

        }

        private void connectServerButton_Click(object sender, EventArgs e)
        {
            // Create message to send connection request
            MessageProtocol connectMessage = new MessageProtocol("#", "ConnectionRequest", "-");
            
            if (ValidateIPAddress() == true)
            {
                waitingToConnect = true;

                // Send string requesting the unique ID assigned by the server
                sendString(connectMessage.GetMessageToSend(), textBoxLightIP.Text);
            }
        }


        //**********************************************************************//
        // Validate the format and the values of IP Address                     //
        // Returns true if it is correct and false if it is not                 //
        //**********************************************************************//
        private bool ValidateIPAddress()
        {
            bool isCorrect = true;
            try
            {
                string ipAddress = textBoxLightIP.Text;
                string[] ipParts = ipAddress.Split('.');

                // Check if IP consists of 4 parts
                if (ipParts.Length != 4)
                {
                    throw new Exception("IP Address is in wrong format!");
                }

                // Check the value ranges
                for (int i = 0; i < ipParts.Length; i++)
                {
                    if (Int32.Parse(ipParts[i]) < 0 || Int32.Parse(ipParts[i]) > 255)
                    {
                        throw new Exception("Wrong range of IP Adress values!");
                    }
                }
            }
            // If it isn't correct, display a message box
            catch (Exception ex)
            {
                isCorrect = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isCorrect;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // If the client is connected, send a car every once in a while
            if (uniqueIDNumber != null && randomGen.Next(4) == 0)
            {
                MessageProtocol sendCarMessage = new MessageProtocol(uniqueIDNumber, "CarSent", "-");
                sendString(sendCarMessage.GetMessageToSend(), textBoxLightIP.Text);
            }
        }

        /// <summary>
        /// Determine if user wants to send cars randomly
        /// </summary>
        private void randomSendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (randomSendCheckBox.Checked == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }
    }   // End of classy class.
}   // End of namespace
