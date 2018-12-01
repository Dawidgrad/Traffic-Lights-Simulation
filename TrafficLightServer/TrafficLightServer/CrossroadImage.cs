using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TrafficLightServer
{
    class CrossroadImage
    {
        private int panelWidth;
        private int panelHeight;
        private Bitmap crossroad;
        private TrafficLightClient[] crossroadLights;

        public CrossroadImage(TrafficLightClient[] clientsArray, int panelWidth, int panelHeight)
        {
            this.panelWidth = panelWidth;
            this.panelHeight = panelHeight;

            crossroadLights = clientsArray;

            InitialiseStates();
        }

        /// <summary>
        /// Starts red lights a little bit ahead
        /// </summary>
        private void InitialiseStates()
        {
            crossroadLights[0].UpdateHowLongRedOn();
            crossroadLights[0].UpdateHowLongRedOn();
            crossroadLights[2].UpdateHowLongRedOn();
            crossroadLights[2].UpdateHowLongRedOn();
        }

        /// <summary>
        /// Draws the crossroad template on the image and returns it
        /// </summary>
        public Bitmap DrawCrossroad()
        {
            crossroad = new Bitmap(panelWidth, panelHeight);

            int roadWidth = 126;
            int widthMid = panelWidth / 2;
            int lengthMid = panelHeight / 2;

            int y1Start = lengthMid - (roadWidth / 2);
            int y2Start = lengthMid + (roadWidth / 2);

            int x1Start = widthMid - (roadWidth / 2);
            int x2Start = widthMid + (roadWidth / 2);


            // Determine starting and ending points of roads
            Point line1StartPoint = new Point(x1Start, 0);
            Point line1EndPoint = new Point(x1Start, y1Start);

            Point line2StartPoint = new Point(x2Start, 0);
            Point line2EndPoint = new Point(x2Start, y1Start);

            Point line3StartPoint = new Point(0, y1Start);
            Point line3EndPoint = new Point(x1Start, y1Start);

            Point line4StartPoint = new Point(0, y2Start);
            Point line4EndPoint = new Point(x1Start, y2Start);

            Point line5StartPoint = new Point(x1Start, y2Start);
            Point line5EndPoint = new Point(x1Start, panelHeight);

            Point line6StartPoint = new Point(x2Start, y2Start);
            Point line6EndPoint = new Point(x2Start, panelHeight);

            Point line7StartPoint = new Point(x2Start, y2Start);
            Point line7EndPoint = new Point(panelWidth, y2Start);

            Point line8StartPoint = new Point(x2Start, y1Start);
            Point line8EndPoint = new Point(panelWidth, y1Start);

            using (Graphics backgroundGraphics = Graphics.FromImage(crossroad))
            {
                // Erase the image 
                backgroundGraphics.Clear(Color.White);

                Pen myPen = new Pen(Color.Black);
                Brush blackBrush = new SolidBrush(Color.Black);
                Brush redBrush = new SolidBrush(Color.Red);
                Brush amberBrush = new SolidBrush(Color.Orange);
                Brush greenBrush = new SolidBrush(Color.Green);

                // Draw the roads
                backgroundGraphics.DrawLine(myPen, line1StartPoint, line1EndPoint);
                backgroundGraphics.DrawLine(myPen, line2StartPoint, line2EndPoint);
                backgroundGraphics.DrawLine(myPen, line3StartPoint, line3EndPoint);
                backgroundGraphics.DrawLine(myPen, line4StartPoint, line4EndPoint);
                backgroundGraphics.DrawLine(myPen, line5StartPoint, line5EndPoint);
                backgroundGraphics.DrawLine(myPen, line6StartPoint, line6EndPoint);
                backgroundGraphics.DrawLine(myPen, line7StartPoint, line7EndPoint);
                backgroundGraphics.DrawLine(myPen, line8StartPoint, line8EndPoint);
                backgroundGraphics.DrawLine(myPen, line8StartPoint, line8EndPoint);

                for (int i = 0; i < crossroadLights.Length; i++)
                {
                    // Draw traffic lights
                    backgroundGraphics.DrawRectangle(myPen, crossroadLights[i].GetTrafficLightRectangle());

                    if (crossroadLights[i].GetIsRedOn() == true)
                    {
                        backgroundGraphics.FillEllipse(redBrush, crossroadLights[i].GetRedLight());
                    }

                    if (crossroadLights[i].GetIsAmberOn() == true)
                    {
                        backgroundGraphics.FillEllipse(amberBrush, crossroadLights[i].GetAmberLight());
                    }

                    if (crossroadLights[i].GetIsGreenOn() == true)
                    {
                        backgroundGraphics.FillEllipse(greenBrush, crossroadLights[i].GetGreenLight());
                    }

                    Font aFont = new Font("Arial", 12, FontStyle.Bold);
                    backgroundGraphics.DrawString(crossroadLights[i].GetID(), aFont, blackBrush, crossroadLights[i].GetIDOnPanelPoint());

                    // Draw every car
                    List<Car> allCars = crossroadLights[i].GetAllCars();
                    for (int j = 0; j < allCars.Count; j++)
                    {
                        Brush carBrush = new SolidBrush(allCars[j].GetColour());
                        backgroundGraphics.FillRectangle(carBrush, allCars[j].GetVector());
                    }
                }
            }

            return crossroad;
        }
    }
}
