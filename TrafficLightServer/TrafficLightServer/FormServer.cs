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
// This project makes an extremely simple server to connect to the other  //
// traffic light clients.  Because of the personal firewall on the lab    //
// computers being switched on, the server cannot use a listening socket  //
// accept incomming connections.  So the server to actually connects to a //
// sort of proxy (running in my office) that accepts the incomming        //
// connection.                                                            //    
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

namespace TrafficLightServer
{

    //New wrapper class.
    public delegate void UI_UpdateHandler(String message);

    public partial class FormServer : Form
    {
        public FormServer()
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

        //******************************************************//
        // Server attributes.                                   //
        //******************************************************//
        private List<TrafficLightClient> connectedClients = new List<TrafficLightClient>();
        private int newestConnectedID = 0;
        private Bitmap imageToDraw;
        private CrossroadImage crossroad;

        //*******************************************************************//
        // This one is needed so that we can post messages back to the form's//
        // thread and don't violate C#'s threading rule that says you can    //
        // only touch the UI components from the form's thread.              //
        //*******************************************************************//
        private SynchronizationContext uiContext = null;



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
        // The OnClick for the "connect"command button.  Create a new client   //
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
                listBoxOutput.Items.Add(ee.Message);				 	       //can't make it, we can just access
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
                inStream         = new BinaryReader(connectionStream);
                outStream        = new BinaryWriter(connectionStream);

                listBoxOutput.Items.Add("Socket connected to " + serverName);

                labelStatus.BackColor = Color.Green;
                labelStatus.Text = "Connected to " + serverName;


                //**********************************************************//
                // Discale connect button (we can only connect once) and    //
                // enable other components.                                 //
                //**********************************************************//
                buttonConnect.Enabled = false;
                templateComboBox.Enabled = true;
                startTemplateButton.Enabled = true;
                disconnectAllClientsButton.Enabled = true;
                imageToDraw = new Bitmap(trafficLightsPanel.Width, trafficLightsPanel.Height);

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
                Thread threadRunner = new Thread(new ThreadStart(threadConnection.run));
                threadRunner.Start();

                Console.WriteLine("Created new connection class");


            }
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
                int length   = stringToSend.Length;
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
            }
            catch (Exception doh)
            {
                listBoxOutput.Items.Add("An error occurred: " + doh.Message);
            }

        }

        /// <summary>
        /// Extracts IP address from message received
        /// </summary>
        private string GetIPFromMessage(string message)
        {
            string address;
            string[] splitMessage;
            char[] delimiter;

            // Set the delimiter and split message by white space
            delimiter = new char[1];
            delimiter[0] = ' ';
            splitMessage = message.Split(delimiter);

            // Get the IP address from array of strings
            address = splitMessage[0];
            return address;
        }

        //*********************************************************************//
        // Message was posted back to us.  This is to get over the C# threading//
        // rules whereby we can only touch the UI components from the thread   //
        // that created them, which is the form's main thread.                 // 
        //*********************************************************************//
        public void MessageReceived(Object received)
        {
            String message = (String) received;
            // Display the message with the timestamp
            listBoxOutput.Items.Add(DateTime.Now.ToString("h:mm:ss tt") + "  " + message);
            // Convert the received message to message protocol object
            MessageProtocol receivedMessage = new MessageProtocol(message);

            // If server receives request for connection
            if (receivedMessage.GetActionType().Equals("ConnectionRequest"))
            {
                // Create a new ID
                newestConnectedID++;
                string newID = (newestConnectedID).ToString();

                // Get the IP address from the message that was sent
                string newIPAddress = GetIPFromMessage(message);

                // Create a traffic light object & add to the connected Clients list
                int trafficLightIndex = connectedClients.Count;
                TrafficLightClient newTrafficLight = new TrafficLightClient(newID, newIPAddress, trafficLightIndex, trafficLightsPanel.Width, trafficLightsPanel.Height);
                connectedClients.Add(newTrafficLight);

                // Assign the client a unique ID
                MessageProtocol requestAccepted = new MessageProtocol("#", "ConnectionRequestAccepted", newID);
                sendString(requestAccepted.GetMessageToSend(), newTrafficLight.GetIPAddress());               
            }
            // If recieves request to disconnect
            else if (receivedMessage.GetActionType().Equals("ClientDisconnecting"))
            {
                DisconnectAllClients();
            }
            // If receives a car from traffic light client
            else if (receivedMessage.GetActionType().Equals("CarSent"))
            {
                string clientIDToUpdate = receivedMessage.GetID();

                // Find the client id that the car was sent from
                for (int i = 0; i < connectedClients.Count; i++)
                {
                    if (connectedClients[i].GetID() == clientIDToUpdate)
                    {
                        // Add car to the client if found
                        connectedClients[i].AddCar();
                        break;
                    }
                }
            }
            
        }
        
        /// <summary>
        /// Kills off any connection thread that was ever created
        /// and checks if any clients are still connected before closing server
        /// </summary>
        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connectedClients.Count > 0)
            {
                MessageBox.Show("Cannot exit while there are clients connected, please disconnect them first.", "Clients are connected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Cancel closing server form
                e.Cancel = true;
            }
            else
            {
                if (threadConnection != null) threadConnection.StopThread();
            }
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update logic of the traffic lights and cars assigned to them
            for (int i = 0; i < connectedClients.Count; i++)
            {
                UpdateTrafficLight(i);
                connectedClients[i].ManageTrafficFlow();
            }

            // Update panel every timer tick
            DrawOntoPanel();
        }

        /// <summary>
        /// Updates light states of single traffic light 
        /// </summary>
        private void UpdateTrafficLight(int index)
        {
            // Check which lights are currently on
            if (connectedClients[index].GetIsRedOn() == true && connectedClients[index].GetIsAmberOn() == false)
            {
                // Update the amount of time red light has been on
                connectedClients[index].UpdateHowLongRedOn();

                // If the red light has been on for 9 seconds
                if (connectedClients[index].GetHowLongRedOn() == 9)
                {
                    // Reset the time since red was on and turn amber light on
                    connectedClients[index].ResetHowLongRedOn();
                    ChangeLightRequest(index, "Amber_On");
                }
            }
            else if (connectedClients[index].GetIsRedOn() == true)
            {
                connectedClients[index].UpdateHowLongRedAmberOn();
                
                if (connectedClients[index].GetHowLongRedAmberOn() == 1)
                {
                    connectedClients[index].ResetHowLongRedAmberOn();
                    ChangeLightRequest(index, "Red_Off");
                    ChangeLightRequest(index, "Amber_Off");
                    ChangeLightRequest(index, "Green_On");
                }
            }
            else if (connectedClients[index].GetIsGreenOn() == true)
            {
                connectedClients[index].UpdateHowLongGreenOn();
                
                if (connectedClients[index].GetHowLongGreenOn() == 5)
                {
                    connectedClients[index].ResetHowLongGreenOn();
                    ChangeLightRequest(index, "Green_Off");
                    ChangeLightRequest(index, "Amber_On");
                }

            }
            else if (connectedClients[index].GetIsAmberOn() == true)
            {
                connectedClients[index].UpdateHowLongAmberOn();
                
                if (connectedClients[index].GetHowLongAmberOn() == 1)
                {
                    connectedClients[index].ResetHowLongAmberOn();
                    ChangeLightRequest(index, "Amber_Off");
                    ChangeLightRequest(index, "Red_On");
                }
            }
        }

        private void ChangeLightRequest(int clientIndex, string actionValue)
        {
            // Send the message to the client
            MessageProtocol changeLightMessage = new MessageProtocol(connectedClients[clientIndex].GetID(), "ChangeLight", actionValue);
            sendString(changeLightMessage.GetMessageToSend(), connectedClients[clientIndex].GetIPAddress());
           
            // Update the state on TrafficLightClient object
            if (actionValue.Equals("Red_On")) connectedClients[clientIndex].SetIsRedOn(true);
            if (actionValue.Equals("Amber_On")) connectedClients[clientIndex].SetIsAmberOn(true);
            if (actionValue.Equals("Green_On")) connectedClients[clientIndex].SetIsGreenOn(true);
            if (actionValue.Equals("Red_Off")) connectedClients[clientIndex].SetIsRedOn(false);
            if (actionValue.Equals("Amber_Off")) connectedClients[clientIndex].SetIsAmberOn(false);
            if (actionValue.Equals("Green_Off")) connectedClients[clientIndex].SetIsGreenOn(false);

        }

        /// <summary>
        /// Start button has been pressed
        /// </summary>
        private void startTemplateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Determine which template is selected and check if there are enough connected clients
                if (templateComboBox.SelectedItem.Equals("Crossroad") && connectedClients.Count >= 4)
                {
                    templateComboBox.Enabled = false;

                    // Create instance of crossroad class
                    TrafficLightClient[] clientArray = { connectedClients[0], connectedClients[1], connectedClients[2], connectedClients[3] };
                    crossroad = new CrossroadImage(clientArray, trafficLightsPanel.Width, trafficLightsPanel.Height);

                    // Initialise the lights
                    ChangeLightRequest(0, "Red_On");
                    ChangeLightRequest(1, "Green_On");
                    ChangeLightRequest(2, "Red_On");
                    ChangeLightRequest(3, "Green_On");

                    // Change states of the buttons
                    startTemplateButton.Enabled = false;
                    stopTemplateButton.Enabled = true;

                    // Draw image and start timer
                    DrawOntoPanel();
                    timer.Start();
                }
                // Potential to add more templates in the future
                else
                {
                    listBoxOutput.Items.Add("\n\nWrong number of clients connected!");
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Select a template!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Stop button has been pressed
        /// </summary>
        private void stopTemplateButton_Click(object sender, EventArgs e)
        {
            // Change sates of the buttons
            stopTemplateButton.Enabled = false;
            startTemplateButton.Enabled = true;
            templateComboBox.Enabled = true;

            ResetTheStateOfLights();
            
            timer.Stop();
            EraseThePanel();
        }

        /// <summary>
        ///  Resets the state of lights on the client and server
        /// </summary>
        private void ResetTheStateOfLights()
        {
            for (int i = 0; i < connectedClients.Count; i++)
            {
                // Reset the lights on client form
                ChangeLightRequest(i, "Red_Off");
                ChangeLightRequest(i, "Amber_Off");
                ChangeLightRequest(i, "Green_Off");

                // Reset the states of lights and remove cars in traffic light client object
                connectedClients[i].ResetAllStates();
            }
        }

        /// <summary>
        /// Disable contents of template combo box if there are not enough clients connected
        /// </summary>
        private void templateComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background 
            e.DrawBackground();

            // Get the item text    
            string text = ((ComboBox)sender).Items[e.Index].ToString();

            // Determine the forecolor based on whether or not the item is selected    
            Brush brush = Brushes.Black;
            if (e.Index == 0 && connectedClients.Count < 4)
            {
                brush = Brushes.Gray;
            }
            // Possible other templates as else if statements

            // Draw the text    
            e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        }

        /// <summary>
        /// Prevent user from selecting 'disabled' templates
        /// </summary>
        private void templateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (templateComboBox.SelectedIndex == 0 && connectedClients.Count < 4)
            {
                templateComboBox.SelectedIndex = -1;
            }
            else if (templateComboBox.SelectedIndex == 1 && connectedClients.Count < 3)
            {
                templateComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Draw the simulation on the panel
        /// </summary>
        private void DrawOntoPanel()
        {
            // If crossroad is selected, get the image from CrossroadImage class
            if (templateComboBox.SelectedItem.Equals("Crossroad"))
            {
                imageToDraw = crossroad.DrawCrossroad();
            }

            using (Graphics graphics = trafficLightsPanel.CreateGraphics())
            {
                graphics.DrawImage(imageToDraw, 0, 0, trafficLightsPanel.Width, trafficLightsPanel.Height);
            }
        }

        /// <summary>
        /// When disconnect all button is pressed
        /// </summary>
        private void disconnectAllClientsButton_Click(object sender, EventArgs e)
        {
            DisconnectAllClients();

            // Enable start button
            stopTemplateButton.Enabled = false;
            startTemplateButton.Enabled = true;
            templateComboBox.Enabled = true;

            timer.Stop();
            EraseThePanel();
        }

        /// <summary>
        /// Erases the image on the panel
        /// </summary>
        private void EraseThePanel()
        {
            using (Graphics backgroundGraphics = Graphics.FromImage(imageToDraw))
            {
                // Erase the image 
                backgroundGraphics.Clear(Color.White);
            }
            using (Graphics graphics = trafficLightsPanel.CreateGraphics())
            {
                graphics.DrawImage(imageToDraw, 0, 0, trafficLightsPanel.Width, trafficLightsPanel.Height);
            }
        }

        /// <summary>
        /// Disconnects all of the clients connected to server
        /// </summary>
        private void DisconnectAllClients()
        {
            // Send disconnect message to every client
            for (int i = (connectedClients.Count - 1); i >= 0; i--)
            {
                MessageProtocol changeLightMessage = new MessageProtocol(connectedClients[i].GetID(), "ClientDisconnect", "-");
                sendString(changeLightMessage.GetMessageToSend(), connectedClients[i].GetIPAddress());

                // Remove clients from the list
                connectedClients.RemoveAt(i);
            }
        }


    }   // End of classy class.
}       // End of namespace
