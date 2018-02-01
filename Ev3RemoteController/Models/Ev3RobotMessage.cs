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
        #region Properties
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
        #endregion

    }
}
