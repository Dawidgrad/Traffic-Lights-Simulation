using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLight
{
    class MessageProtocol
    {
        // Separate parts of the message
        private string idNumber;
        
        private string actionType;

        private string actionValue;

        // All parts of the message concatenated
        private string messageToSend;


        /// <summary>
        /// Constructor taking in the whole message
        /// </summary>
        public MessageProtocol(string wholeMessage)
        {
            // Split the whole message into parts and assign them to appropriate fields
            string[] splitMessage = SplitUpMessage(wholeMessage);
            
            this.idNumber = splitMessage[2];
            this.actionType = splitMessage[3];
            this.actionValue = splitMessage[4];
            
            // Prepare the message to send over the network
            this.messageToSend = this.idNumber + " " + this.actionType + " " + this.actionValue;
        }

        /// <summary>
        /// Constructor taking in parts of the message and 
        /// creating single message out of them
        /// </summary>
        public MessageProtocol(string idNumber, string actionType, string actionValue)
        {
            // Assign each parameter to its respective field
            this.idNumber = idNumber;
            this.actionType = actionType;
            this.actionValue = actionValue;

            // Prepare the message to send over the network
            this.messageToSend = this.idNumber + " " + this.actionType + " " + this.actionValue;
        }

        /// <summary>
        /// Splits the message into the array of strings
        /// </summary>
        private string[] SplitUpMessage(string theMessage)
        {
            string[] splitMessage;
            char[] delimiter;

            // Split the message by spaces
            delimiter = new char[1];
            delimiter[0] = ' ';
            splitMessage = theMessage.Split(delimiter);

            // Return array of parts of message
            return splitMessage;
        }

        /// <summary>
        /// Returns the whole message
        /// </summary>
        public string GetMessageToSend() { return messageToSend; }

        /// <summary>
        /// Returns the ID number
        /// </summary>
        public string GetID() { return idNumber; }

        /// <summary>
        /// Returns the type of action included in the message
        /// </summary>
        public string GetActionType() { return actionType; }

        /// <summary>
        /// Returns the value of the action included in the message
        /// </summary>
        public string GetActionValue() { return actionValue; }
    }
}
