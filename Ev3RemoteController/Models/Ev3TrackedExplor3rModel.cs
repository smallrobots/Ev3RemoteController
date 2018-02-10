///////////////////////////////////
// Ev3TrackedExplor3r            //
// Ev3TrackedExplor3rModel.cs    //
//                               //
// Copyright Smallrobots.it 2017 //
///////////////////////////////////

using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3TrackedExplor3rModel : Ev3RobotModel
    {
        #region Command to the robot as Properties
        int turnHeadCommand = 0;
        /// <summary>
        /// Gets or sets the TurnHeadCommand
        /// </summary>
        public int TurnHeadCommand
        {
            get => turnHeadCommand;
            set
            {
                if ((value >= -1000) && (value <= 1000))
                {
                    turnHeadCommand = value;
                    RaisePropertyChanged();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("TurnHeadCommand", value,
                        "TurnHeadCommand value must be within the range [-1000,1000]");
                }
            }
        }
        #endregion

        #region Robot Telemetry
        ulong lastSampleID;
        /// <summary>
        /// Gets the ID of the last sample
        /// </summary>
        public ulong LastSampleID
        {
            get => lastSampleID;
            set
            {
                if (lastSampleID != value)
                {
                    lastSampleID = value;
                    RaisePropertyChanged();
                }
            }
        }

        int leftMotorSpeed = 0;
        /// <summary>
        /// Gets or sets the left motor speed telemetry
        /// </summary>
        public int LeftMotorSpeed
        {
            get => -leftMotorSpeed;
            protected set
            {
                leftMotorSpeed = value;
                RaisePropertyChanged();
            }
        }

        int rightMotorSpeed = 0;
        /// <summary>
        /// Gets or sets the right motor speed telemetry
        /// </summary>
        public int RightMotorSpeed
        {
            get => -rightMotorSpeed;
            protected set
            {
                rightMotorSpeed = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Create a new outbound message
        /// </summary>
        /// <returns>New message</returns>
        public override Ev3RobotMessage CreateOutboundMessage()
        {
            Ev3TrackedExplor3rMessage message = new Ev3TrackedExplor3rMessage(base.CreateOutboundMessage());
            message.TurnHeadCommand = TurnHeadCommand;
            return message;
        }

        /// <summary>
        /// Process the incoming message
        /// </summary>
        /// <param name="messageIn">JSON encoded incoming message</param>
        public override void ProcessIncomingMessage(string messageIn)
        {
            // Call base method
            base.ProcessIncomingMessage(messageIn);
            Ev3TrackedExplor3rMessage theMessage = JsonConvert.DeserializeObject<Ev3TrackedExplor3rMessage>(messageIn);
            DispatcherHelper.CheckBeginInvokeOnUI(() => RightMotorSpeed = theMessage.RightMotorSpeed);
            DispatcherHelper.CheckBeginInvokeOnUI(() => LeftMotorSpeed = theMessage.LeftMotorSpeed);
        }
        #endregion
    }
}
