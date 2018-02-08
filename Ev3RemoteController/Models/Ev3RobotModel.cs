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
        #region Ev3 Robot telemetry properties
        int batteryLevel = 0;
        /// <summary>
        /// Gets or private sets the battery level
        /// </summary>
        public int BatteryLevel
        {
            get => batteryLevel;
            protected set
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

        #region Command to the Ev3 Robot properties
        int forwardCommand = 0;
        /// <summary>
        /// Gets or sets the forward command (negative values mean backward)
        /// Allowed range [-1000, 1000]
        /// </summary>
        public int ForwardCommand
        {
            get => forwardCommand;
            set
            {
                if ((value >= -1000) && (value <= 1000))
                {
                    forwardCommand = value;
                    RaisePropertyChanged();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("ForwardCommand", value, 
                        "Forward value must be within the range [-1000,1000]");
                }
            }
        }

        int turnCommand = 0;
        /// <summary>
        /// Gets or sets the turn command 
        /// Positive values mean turn to right,
        /// Negative values mean turn to left)
        /// Allowed range [-1000, 1000]
        /// </summary>
        public int TurnCommand
        {
            get => turnCommand;
            set
            {
                if ((value >= -1000) && (value <= 1000))
                {
                    turnCommand = value;
                    RaisePropertyChanged();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("TurnCommand", value,
                        "Forward value must be within the range [-1000,1000]");
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new outbound message
        /// </summary>
        /// <returns>New message</returns>
        public virtual Ev3RobotMessage CreateOutboundMessage()
        {
            Ev3RobotMessage message = new Ev3RobotMessage();

            message.MessageFunction = MessageType.command;
            message.ForwardCommand = ForwardCommand;
            message.TurnCommand = TurnCommand;

            return message;
        }

        /// <summary>
        /// Process the incoming message
        /// </summary>
        /// <param name="messageIn">JSON encoded incoming message</param>
        public virtual void ProcessIncomingMessage(string messageIn)
        {
            Ev3RobotMessage theMessage = JsonConvert.DeserializeObject<Ev3RobotMessage>(messageIn);
            DispatcherHelper.CheckBeginInvokeOnUI( ()=> BatteryLevel = theMessage.BatteryLevel);
            
        }
        #endregion
    }
}
