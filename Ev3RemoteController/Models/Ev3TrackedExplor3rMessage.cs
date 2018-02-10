using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3TrackedExplor3rMessage : Ev3RobotMessage
    {
        #region Telemetry properties
        [JsonIgnore]
        int leftMotorSpeed = 0;
        /// <summary>
        /// Gets or sets the left motor speed telemetry
        /// </summary>
        [JsonProperty("left_motor_speed")]
        public int LeftMotorSpeed
        {
            get => leftMotorSpeed;
            set
            {
                leftMotorSpeed = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int rightMotorSpeed = 0;
        /// <summary>
        /// Gets or sets the right motor speed telemetry
        /// </summary>
        [JsonProperty("right_motor_speed")]
        public int RightMotorSpeed
        {
            get => rightMotorSpeed;
            set
            {
                rightMotorSpeed = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Command Properties
        [JsonIgnore]
        int turnHeadCommand = 0;
        [JsonProperty("turn_head_command")]
        /// <summary>
        /// Gets or sets the TurnHeadCommand
        /// </summary>
        public int TurnHeadCommand
        {
            get => turnHeadCommand;
            set
            {
                turnHeadCommand = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Ev3TrackedExplor3rMessage() : base()
        {

        }

        /// <summary>
        /// Constructor that accepts a base class instance
        /// </summary>
        /// <param name="baseMessage">Base class instance</param>
        public Ev3TrackedExplor3rMessage(Ev3RobotMessage baseMessage) : base()
        {
            // Copying base class propertie
            BatteryLevel = baseMessage.BatteryLevel;
            ForwardCommand = baseMessage.ForwardCommand;
            MessageFunction = baseMessage.MessageFunction;
            MessageId = baseMessage.MessageId;
            RemoteControllerAddress = baseMessage.RemoteControllerAddress;
            RemoteControllerName = baseMessage.RemoteControllerName;
            RemoteControllerPort = baseMessage.RemoteControllerPort;
            RobotAddress = baseMessage.RobotAddress;
            RobotName = baseMessage.RobotName;
            RobotPort = baseMessage.RobotPort;
            TurnCommand = baseMessage.TurnCommand;
        }
        #endregion
    }
}
