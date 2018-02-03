///////////////////////////////////
// Ev3TrackedExplor3r            //
//                               //
// Ev3Robot.cs                   //
// Copyright Smallrobots.it 2015 //
///////////////////////////////////

using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3RobotModel : Model
    {
        #region Properties
        int batteryLevel = 0;
        /// <summary>
        /// Gets or private sets the battery level
        /// </summary>
        public int BatteryLevel
        {
            get => batteryLevel;
            private set
            {
                batteryLevel = value;
                RaisePropertyChanged();
                RaisePropertyChanged("BatteryLevelAsFloat");
            }
        }

        public string BatteryLevelAsFloat
        {
            get => ((double)batteryLevel / 1000000).ToString("N2");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new outbound message
        /// </summary>
        /// <returns>New message</returns>
        public Ev3RobotMessage CreateOutboundMessage()
        {
            Ev3RobotMessage message = new Ev3RobotMessage();
            return message;
        }

        /// <summary>
        /// Process the incoming message
        /// </summary>
        /// <param name="messageIn">JSON encoded incoming message</param>
        public void ProcessIncomingMessage(string messageIn)
        {
            Ev3RobotMessage theMessage = JsonConvert.DeserializeObject<Ev3RobotMessage>(messageIn);
            DispatcherHelper.CheckBeginInvokeOnUI( ()=> BatteryLevel = theMessage.BatteryLevel);
            
        }
        #endregion
    }
}
