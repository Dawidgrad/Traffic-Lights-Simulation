using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightServer
{
    class Car
    {
        // Position and size of the car
        private Rectangle vector;
        private Color colour;
        private Color[] colourArray = {Color.Chocolate, Color.Sienna, Color.Yellow, Color.Gold, Color.Black, Color.Blue, Color.DarkCyan, Color.Turquoise};
        static public int CarSize = 30;

        public Car(Point initalPosition)
        {
            this.vector = new Rectangle(initalPosition.X, initalPosition.Y, CarSize, CarSize);

            // Assign colour to a car randomly 
            Random randomGen = new Random();
            this.colour = colourArray[randomGen.Next(colourArray.Length)];
        }

        // Returns position and size of a car as a rectangle object
        public Rectangle GetVector() { return this.vector; }

        // Moves a car in direction passed as an argument
        public void MoveCar(Point directionVector)
        {
            this.vector.X += directionVector.X;
            this.vector.Y += directionVector.Y;
        }

        // Returns colour of the car
        public Color GetColour() { return this.colour; }
    }
}
