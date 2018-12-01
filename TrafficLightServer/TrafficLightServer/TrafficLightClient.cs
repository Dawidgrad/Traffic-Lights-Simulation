using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightServer
{
    class TrafficLightClient
    {
        // Attributes that identify each client
        private string id;
        private string ipAddress;
        private int index;

        // Attributes that store how long each state of the 
        // traffic lights has been on
        private int howLongRedOn;
        private int howLongGreenOn;
        private int howLongAmberOn;
        private int howLongRedAmberOn;

        // States of traffic light
        private bool isRedOn;
        private bool isGreenOn;
        private bool isAmberOn;

        // Cars that are waiting for the green light to turn on
        private List<Car> waitingCars;

        // Cars that have passed traffic light
        private List<Car> movingCars;

        // Animation attributes
        private int gapSize;
        private int roadWidth = 126;
        private int widthMid;
        private int lengthMid;
        private int trafficLightWidth = 30;
        private int trafficLightHeight = 90;
        private int panelWidth;
        private int panelHeight;
        private Point idOnPanelPoint;
        private Rectangle trafficLightRectangle;
        private Rectangle redLight;
        private Rectangle amberLight;
        private Rectangle greenLight;
        
        
        /// <summary>
        /// Constructor that takes in the parameters id & ipAddress 
        /// </summary>
        public TrafficLightClient(string id, string ipAddress, int index, int panelWidth, int panelHeight)
        {
            this.id = id;
            this.ipAddress = ipAddress;
            this.index = index;
            this.panelWidth = panelWidth;
            this.panelHeight = panelHeight;

            // Default values assigned
            this.gapSize = 15;
            this.howLongRedOn = 0;
            this.howLongGreenOn = 0;
            this.isRedOn = false;
            this.isGreenOn = false;
            this.waitingCars = new List<Car>();
            this.movingCars = new List<Car>();

            // Assign location details
            this.widthMid = panelWidth / 2;
            this.lengthMid = panelHeight / 2;

            SetLightPosition();
        }
        
        /// <summary>
        /// Sets traffic light position on the panel which depends on index
        /// </summary>
        private void SetLightPosition()
        {
            int locationX;
            int locationY;

            // Determine which index this light client has
            if (this.index == 0)
            {
                // Set the position of a traffic light
                locationX = widthMid - (roadWidth / 2) - trafficLightWidth;
                locationY = lengthMid + (roadWidth / 2);
                trafficLightRectangle = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightHeight);

                // Set the position of each light in this particular traffic light
                redLight = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightWidth);
                amberLight = new Rectangle(locationX, locationY + trafficLightWidth, trafficLightWidth, trafficLightWidth);
                greenLight = new Rectangle(locationX, locationY + (trafficLightWidth * 2), trafficLightWidth, trafficLightWidth);

                // Set the position of an traffic light ID on the panel
                idOnPanelPoint = new Point(locationX, locationY + trafficLightHeight);
            }
            else if (this.index == 1)
            {
                locationX = widthMid + (roadWidth / 2);
                locationY = lengthMid + (roadWidth / 2);

                trafficLightRectangle = new Rectangle(locationX, locationY, trafficLightHeight, trafficLightWidth);

                redLight = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightWidth);
                amberLight = new Rectangle(locationX + trafficLightWidth, locationY, trafficLightWidth, trafficLightWidth);
                greenLight = new Rectangle(locationX + (trafficLightWidth * 2), locationY, trafficLightWidth, trafficLightWidth);

                idOnPanelPoint = new Point(locationX, locationY + trafficLightWidth);

            }
            else if (this.index == 2)
            {
                locationX = widthMid + (roadWidth / 2);
                locationY = lengthMid - (roadWidth / 2) - trafficLightHeight;

                trafficLightRectangle = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightHeight);

                redLight = new Rectangle(locationX, locationY + (trafficLightWidth * 2), trafficLightWidth, trafficLightWidth);
                amberLight = new Rectangle(locationX, locationY + trafficLightWidth, trafficLightWidth, trafficLightWidth);
                greenLight = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightWidth);

                idOnPanelPoint = new Point(locationX + trafficLightWidth, locationY);

            }
            else if (this.index == 3)
            {
                locationX = widthMid - (roadWidth / 2) - trafficLightHeight;
                locationY = lengthMid - (roadWidth / 2) - trafficLightWidth;

                trafficLightRectangle = new Rectangle(locationX, locationY, trafficLightHeight, trafficLightWidth);

                redLight = new Rectangle(locationX + (trafficLightWidth * 2), locationY, trafficLightWidth, trafficLightWidth);
                amberLight = new Rectangle(locationX + trafficLightWidth, locationY, trafficLightWidth, trafficLightWidth);
                greenLight = new Rectangle(locationX, locationY, trafficLightWidth, trafficLightWidth);

                idOnPanelPoint = new Point(locationX, locationY - 20);
            }
        }

        /// <summary>
        /// Creates new car and adds it to waiting queue
        /// </summary>
        public void AddCar()
        {
            int carSize = Car.CarSize;
            int locationX = 0;
            int locationY = 0;

            // Determine which index traffic light client has
            if (index == 0)
            {
                // Set starting x and y coordinates for the car
                locationX = widthMid - (roadWidth / 2) + gapSize;
                locationY = lengthMid + (roadWidth / 2) + DistanceFromLight(3);
            }
            else if (index == 1)
            {
                locationX = widthMid + (roadWidth / 2) + DistanceFromLight(4);
                locationY = lengthMid + (roadWidth / 2) - (gapSize + carSize);
            }
            else if (index == 2)
            {
                locationX = widthMid + (roadWidth / 2) - (gapSize + carSize);
                locationY = lengthMid - (roadWidth / 2) - DistanceFromLight(4) + gapSize;
            }
            else if (index == 3)
            {
                locationX = widthMid - (roadWidth / 2) - DistanceFromLight(5) + gapSize;
                locationY = lengthMid - (roadWidth / 2) + gapSize;
            }

            // Add new car at specified position
            Point carPosition = new Point(locationX, locationY);
            Car newCar = new Car(carPosition);
            waitingCars.Add(newCar);
        }

        /// <summary>
        /// Manages movement of all of the cars assigned to traffic light
        /// </summary>
        public void ManageTrafficFlow()
        {
            ManageWaitingCars();
            ManageMovingCars();
        }

        /// <summary>
        /// Manages movement of cars that have not passed the lights
        /// </summary>
        private void ManageWaitingCars()
        {
            // Position where car passes the lights
            int lightPosition;
            
            for (int i = waitingCars.Count - 1; i >= 0; i--)
            {
                // Determine the index of traffic light
                if (index == 0)
                {
                    lightPosition = lengthMid + (roadWidth / 2);

                    // If passed the light
                    if (waitingCars[i].GetVector().Y < lightPosition)
                    {
                        // Move the car from waiting cars to moving cars list
                        movingCars.Add(waitingCars[i]);
                        waitingCars.RemoveAt(i);
                    }
                    // If the car is still before the lights
                    else
                    {
                        // Move car if the green light is on
                        if (this.isGreenOn)
                        {
                            Point carMoveChange = new Point(0, -(Car.CarSize + gapSize));
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                        // Move the car as close to traffic lights as possible
                        // taking into account other cars
                        else if (waitingCars[i].GetVector().Y > lightPosition + DistanceFromLight(i))
                        {
                            Point carMoveChange = new Point(0, -(Car.CarSize + gapSize));
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                    }
                }
                else if (index == 1)
                {
                    lightPosition = widthMid + (roadWidth / 2);

                    if (waitingCars[i].GetVector().X < lightPosition)
                    {
                        movingCars.Add(waitingCars[i]);
                        waitingCars.RemoveAt(i);
                    }
                    else
                    {
                        if (this.isGreenOn)
                        {
                            Point carMoveChange = new Point(-(Car.CarSize + gapSize), 0);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                        else if (waitingCars[i].GetVector().X > lightPosition + DistanceFromLight(i))
                        {
                            Point carMoveChange = new Point(-(Car.CarSize + gapSize), 0);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                    }
                }
                else if (index == 2)
                {
                    lightPosition = lengthMid - (roadWidth / 2) - Car.CarSize;

                    if (waitingCars[i].GetVector().Y > lightPosition)
                    {
                        movingCars.Add(waitingCars[i]);
                        waitingCars.RemoveAt(i);
                    }
                    else
                    {
                        if (this.isGreenOn)
                        {
                            Point carMoveChange = new Point(0, Car.CarSize + gapSize);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                        else if (waitingCars[i].GetVector().Y < lightPosition - DistanceFromLight(i))
                        {
                            Point carMoveChange = new Point(0, Car.CarSize + gapSize);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                    }
                }
                else if (index == 3)
                {
                    lightPosition = widthMid - (roadWidth / 2) - Car.CarSize;

                    if (waitingCars[i].GetVector().X > lightPosition)
                    {
                        movingCars.Add(waitingCars[i]);
                        waitingCars.RemoveAt(i);
                    }
                    else
                    {
                        if (this.isGreenOn)
                        {
                            Point carMoveChange = new Point(Car.CarSize + gapSize, 0);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                        else if (waitingCars[i].GetVector().X < lightPosition - DistanceFromLight(i))
                        {
                            Point carMoveChange = new Point(Car.CarSize + gapSize, 0);
                            waitingCars[i].MoveCar(carMoveChange);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the distance that car should be away from the traffic light
        /// </summary>
        private int DistanceFromLight(int carIndex)
        {
            int distance;
            
            distance = (Car.CarSize + gapSize) * carIndex;

            return distance;
        }

        /// <summary>
        /// Manage cars that have already passed the light
        /// </summary>
        private void ManageMovingCars()
        {
            for (int i = (movingCars.Count - 1); i >= 0; i--)
            {
                // Determine the index of the traffic light
                if (index == 0)
                {
                    // Move the car forward
                    Point carMoveChange = new Point(0, -(Car.CarSize + gapSize));
                    movingCars[i].MoveCar(carMoveChange);

                    // If the car has already disappeared from the panel, remove it from the list 
                    if (movingCars[i].GetVector().Y < -Car.CarSize)
                    {
                        movingCars.RemoveAt(i);
                    }

                }
                else if (index == 1)
                {
                    Point carMoveChange = new Point(-(Car.CarSize + gapSize), 0);
                    movingCars[i].MoveCar(carMoveChange);

                    if (movingCars[i].GetVector().X < -Car.CarSize)
                    {
                        movingCars.RemoveAt(i);
                    }
                }
                else if (index == 2)
                {
                    Point carMoveChange = new Point(0, Car.CarSize + gapSize);
                    movingCars[i].MoveCar(carMoveChange);

                    if (movingCars[i].GetVector().Y > (panelHeight + Car.CarSize))
                    {
                        movingCars.RemoveAt(i);
                    }
                }
                else if (index == 3)
                {
                    Point carMoveChange = new Point(Car.CarSize + gapSize, 0);
                    movingCars[i].MoveCar(carMoveChange);

                    if (movingCars[i].GetVector().X > (panelWidth + Car.CarSize))
                    {
                        movingCars.RemoveAt(i);
                    }
                }
            }
        }



        // Increment time since particular state of traffic light was on
        public void UpdateHowLongGreenOn() { this.howLongGreenOn++; }
        public void UpdateHowLongAmberOn() { this.howLongAmberOn++; }
        public void UpdateHowLongRedOn() { this.howLongRedOn++; }
        public void UpdateHowLongRedAmberOn() { this.howLongRedAmberOn++; }

        // Reset time since particular state of traffic light was on
        public void ResetHowLongGreenOn() { this.howLongGreenOn = 0; }
        public void ResetHowLongAmberOn() { this.howLongAmberOn = 0; }
        public void ResetHowLongRedOn() { this.howLongRedOn = 0; }
        public void ResetHowLongRedAmberOn() { this.howLongRedAmberOn = 0; }

        // Reset all of the states and car lists
        public void ResetAllStates()
        {
            this.howLongGreenOn = 0;
            this.howLongAmberOn = 0;
            this.howLongRedOn = 0;
            this.howLongRedAmberOn = 0;

            movingCars = new List<Car>();
            waitingCars = new List<Car>();
        }

        // Get time since particular traffic light state was on
        public int GetHowLongGreenOn() { return this.howLongGreenOn; }
        public int GetHowLongAmberOn() { return this.howLongAmberOn; }
        public int GetHowLongRedOn() { return this.howLongRedOn; }
        public int GetHowLongRedAmberOn() { return this.howLongRedAmberOn; }

        // Get light state
        public bool GetIsGreenOn() { return this.isGreenOn; }
        public bool GetIsAmberOn() { return this.isAmberOn; }
        public bool GetIsRedOn() { return this.isRedOn; }

        // Set light state
        public void SetIsGreenOn(bool lightStatus) { this.isGreenOn = lightStatus; }
        public void SetIsAmberOn(bool lightStatus) { this.isAmberOn = lightStatus; }
        public void SetIsRedOn(bool lightStatus) { this.isRedOn = lightStatus; }

        // Get traffic light position
        public Rectangle GetTrafficLightRectangle() { return this.trafficLightRectangle; }
        public Rectangle GetRedLight() { return this.redLight; }
        public Rectangle GetAmberLight() { return this.amberLight; }
        public Rectangle GetGreenLight() { return this.greenLight; }

        // Get number 
        public Point GetIDOnPanelPoint() { return this.idOnPanelPoint; }

        /// <summary>
        /// Returns the ID 
        /// </summary>
        public string GetID() { return id; }

        /// <summary>
        /// Returns the IP Address
        /// </summary>
        public string GetIPAddress() { return ipAddress; }

        /// <summary>
        /// Returns the number of cars
        /// </summary>
        public int GetNumOfWaitingCars() { return waitingCars.Count; }

        /// <summary>
        /// Returns all of the cars that were managed by single traffic light
        /// </summary>
        public List<Car> GetAllCars()
        {
            // Concatenate moving and waiting cars lists
            List<Car> allCars = new List<Car>();
            allCars.AddRange(movingCars);
            allCars.AddRange(waitingCars);

            return allCars;
        }
    }
}
