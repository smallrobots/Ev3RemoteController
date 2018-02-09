using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Smallrobots.Ev3RemoteController.Models
{
    public enum MessageType
    {
        subscribe = 1,
        unsubscribe = 2,
        command = 3,
        robot_status = 4
    }

    /// <summary>
    /// Message exchanged between the robot and the controller
    /// </summary>
    public class Ev3RobotMessage : Model
    {
        #region Communication properties
        [JsonIgnore]
        int messageId = 0;
        /// <summary>
        /// Gets or sets the MessageId
        /// </summary>
        [JsonProperty("message_id")]
        public int MessageId
        {
            get => messageId;
            set
            {
                messageId = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string robotName = "";
        /// <summary>
        /// Gets or sets the Robot Name
        /// </summary>
        [JsonProperty("robot_name")]
        public string RobotName
        {
            get => robotName;
            set
            {
                robotName = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string robotAddress = "";
        /// <summary>
        /// Gets or sets the Robot IP Address
        /// </summary>
        [JsonProperty("robot_address")]
        public string RobotAddress
        {
            get => robotAddress;
            set
            {
                robotAddress = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string robotPort = "";
        /// <summary>
        /// Gets or sets the Robot IP Port
        /// </summary>
        [JsonProperty("robot_port")]
        public string RobotPort
        {
            get => robotPort;
            set
            {
                robotPort = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string remoteControllerName = "";
        /// <summary>
        /// Gets or sets the Remote Controller Name
        /// </summary>
        [JsonProperty("remote_controller_name")]
        public string RemoteControllerName
        {
            get => remoteControllerName;
            set
            {
                remoteControllerName = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string remoteControllerAddress = "";
        /// <summary>
        /// Gets or setr the Remote Controller Address
        /// </summary>
        [JsonProperty("remote_controller_address")]
        public string RemoteControllerAddress
        {
            get => remoteControllerAddress;
            set
            {
                remoteControllerAddress = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        string remoteControllerPort = "";
        /// <summary>
        /// Gets or sets the Remote Controller Port
        /// </summary>
        [JsonProperty("remote_controller_port")]
        public string RemoteControllerPort
        {
            get => remoteControllerPort;
            set
            {
                remoteControllerPort = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        MessageType messageFunction = MessageType.subscribe;
        /// <summary>
        /// Gets or sets the message function
        /// </summary>
        [JsonProperty("message_function")]
        public MessageType MessageFunction
        {
            get => messageFunction;
            set
            {
                messageFunction = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Telemetry properties
        [JsonIgnore]
        int batteryLevel = 0;
        /// <summary>
        /// Gets or sets the battery level in volts / 1000000
        /// </summary>
        [JsonProperty("battery_level")]
        public int BatteryLevel
        {
            get => batteryLevel;
            set
            {
                batteryLevel = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int batteryAmperage = 0;
        /// <summary>
        /// Gets or sets the battery measured current in microAmps
        /// </summary>
        [JsonProperty("battery_amperage")]
        public int BatteryAmperage
        {
            get => batteryAmperage;
            set
            {
                batteryAmperage = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Command properties
        [JsonIgnore]
        int forwardCommand = 0;
        [JsonProperty("forward_command")]
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

        [JsonIgnore]
        int turnCommand = 0;
        [JsonProperty("turn_command")]
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

    }
}
