using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightServer
{
    class MessageProtocol
    {
        // All parts of the message concatenated
        private string messageToSend;

        // Separate parts of the message
        private string idNumber;
        private string actionType;
        private string actionValue;

        /// <summary>
        /// Constructor taking in the whole message and splits it into seperate message
        /// </summary>
        public MessageProtocol(string wholeMessage)
        {
            // wholeMessage is split into array where it's seperated around the spaces
            string[] splitMessage = SplitUpMessage(wholeMessage);
            
            this.idNumber = splitMessage[2];
            this.actionType = splitMessage[3];
            this.actionValue = splitMessage[4];

            this.messageToSend = this.idNumber + " " + this.actionType + " " + this.actionValue;
        }

        /// <summary>
        /// Constructor taking in parts of the message and 
        /// creating single message out of them
        /// </summary>
        public MessageProtocol(string idNumber, string actionType, string actionValue)
        {
            this.idNumber = idNumber;
            this.actionType = actionType;
            this.actionValue = actionValue;

            this.messageToSend = this.idNumber + " " + this.actionType + " " + this.actionValue;
        }

        /// <summary>
        /// Splits the message into the array of strings
        /// </summary>
        private string[] SplitUpMessage(string theMessage)
        {
            string[] splitMessage;
            char[] delimiter;

            delimiter = new char[1];
            delimiter[0] = ' ';
            splitMessage = theMessage.Split(delimiter);

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
