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

        #region Constructor
        public Ev3TrackedExplor3rMessage() : base()
        {

        }

        public Ev3TrackedExplor3rMessage(Ev3RobotMessage baseMessage) : base()
        {
            this.BatteryLevel = baseMessage.BatteryLevel;
            this.ForwardCommand = baseMessage.ForwardCommand;
            this.MessageFunction = baseMessage.MessageFunction;
            this.MessageId = baseMessage.MessageId;
            this.RemoteControllerAddress = baseMessage.RemoteControllerAddress;
            this.RemoteControllerName = baseMessage.RemoteControllerName;
            this.RemoteControllerPort = baseMessage.RemoteControllerPort;
            this.RobotAddress = baseMessage.RobotAddress;
            this.RobotName = baseMessage.RobotName;
            this.RobotPort = baseMessage.RobotPort;
            this.TurnCommand = baseMessage.TurnCommand;
        }
        #endregion
    }
}
