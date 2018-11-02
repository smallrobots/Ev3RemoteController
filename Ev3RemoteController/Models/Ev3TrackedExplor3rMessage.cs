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

        [JsonIgnore]
        int singleIrReading = 0;
        /// <summary>
        /// Gets or sets a single reading from the IR Sensor
        /// </summary>
        [JsonProperty("single_ir_reading")]
        public int SingleIrReading
        {
            get => singleIrReading;
            set
            {
                singleIrReading = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int headMotorPosition = 0;
        /// <summary>
        /// Gets or sets the Head Motor Position
        /// </summary>
        [JsonProperty("head_motor_position")]
        public int HeadMotorPosition
        {
            get => headMotorPosition;
            set
            {
                headMotorPosition = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int[] irContinuousScan = new int[30];
        /// <summary>
        /// Gets or sets the IR Continuous Scan List
        /// </summary>
        [JsonProperty("ircs_scan_list")]
        public int[] IRContinuousScan
        {
            get => irContinuousScan;
            set
            {
                irContinuousScan = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int gyro_reading = 0;
        /// <summary>
        /// Gets or sets the Gyro Reading giving the measured heading of the rover
        /// </summary>
        [JsonProperty("rover_measured_heading")]
        public int GyroReading
        {
            get => gyro_reading;
            set
            {
                gyro_reading = value;
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

        [JsonIgnore]
        bool isContinuousScanActivated = false;
        [JsonProperty("is_continuous_scan_activated")]
        /// <summary>
        /// Gets or sets the activation of the continuous scan
        /// </summary>
        public bool IsContinuousScanActivated
        {
            get => isContinuousScanActivated;
            set
            {
                isContinuousScanActivated = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        int roverSelected = 0;
        [JsonProperty("rover_selected")]
        /// <summary>
        /// Gets or sets the index of the rover selected
        /// So far:
        /// 0 - Ev3 Tracked Explor3r
        /// 1 - IR Scan Tester
        /// 2 - Ev3 Tracked Explor3r Mark II
        /// </summary>
        public int RoverSelected
        {
            get => roverSelected;
            set
            {
                if (roverSelected != value)
                {
                    roverSelected = value;
                    RaisePropertyChanged();
                }
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
