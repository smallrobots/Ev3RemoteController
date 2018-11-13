﻿///////////////////////////////////
// Ev3TrackedExplor3r            //
// Ev3TrackedExplor3rModel.cs    //
//                               //
// Copyright Smallrobots.it 2017 //
///////////////////////////////////

using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        bool isContinuousScanActivated = false;
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

        bool keepRoute = false;
        /// <summary>
        /// Gets or sets KeepRoute
        /// When KeepRoute is True, the rover will try to keep its route using the Gyroscope sensor
        /// </summary>
        public bool KeepRoute
        {
            get => keepRoute;
            set
            {
                if (keepRoute != value)
                {
                    keepRoute = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("KeepRouteNotification");
                }
            }
        }

        public string KeepRouteNotification
        {
            get
            {
                if (keepRoute)
                    return "Yes";
                else
                    return "No";
            }
        }

        int roverSelected = 0;
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

        bool roamingModeActivated = false;
        /// <summary>
        /// Gets or sets the command that activate the roaming mode on the rover
        /// </summary>
        public bool RoamingModeActivated
        {
            get => roamingModeActivated;
            set
            {
                if (roamingModeActivated != value)
                {
                    roamingModeActivated = value;
                    RaisePropertyChanged();
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

        int singleIrReading = 0;
        /// <summary>
        /// Gets or sets a single reading from the IR Sensor
        /// </summary>
        public int SingleIrReading
        {
            get => singleIrReading;
            set
            {
                singleIrReading = value;
                RaisePropertyChanged();
            }
        }

        int headMotorPosition_Zero = 0;
        /// <summary>
        /// Gets or sets the zero for the head motor
        /// </summary>
        public int HeadMotorPosition_Zero
        {
            get => headMotorPosition_Zero;
            set
            {
                headMotorPosition_Zero = value;
                RaisePropertyChanged();
                RaisePropertyChanged("HeadMotorPosition_Calibrated");
            }
        }

        /// <summary>
        /// Gets the head motor position calibrated
        /// </summary>
        public int HeadMotorPosition_Calibrated
        {
            get
            {
                if (RoverSelected == 0)
                {
                    // Ev3 Tracked Explor3r Mark I
                    // return (int)((HeadMotorPosition - HeadMotorPosition_Zero) / 145.0 * 90.0);
                    // Ev3 Tracked Explor3r Mark I
                    return (int)((HeadMotorPosition - HeadMotorPosition_Zero) / 1250.0 * 90.0);
                }
                else if (RoverSelected == 1)
                {
                    // IRScan Test
                    return (int)((HeadMotorPosition - HeadMotorPosition_Zero) / 1200.0 * 90.0);
                }
                else return 0;
            }
        }

        int headMotorPosition = 0;
        /// <summary>
        /// Gets or sets the Head Motor Position
        /// </summary>
        public int HeadMotorPosition
        {
            get => headMotorPosition;
            set
            {
                headMotorPosition = value;
                RaisePropertyChanged();
                RaisePropertyChanged("HeadMotorPosition_Calibrated");
            }
        }

        ObservableCollection<int> iRContinuousScan;
        /// <summary>
        /// Gets or sets the IR Continuos Scan List
        /// </summary>
        public ObservableCollection<int> IRContinuousScan
        {
            get => iRContinuousScan;
            set
            {
                iRContinuousScan = value;
                RaisePropertyChanged();
            }
        }

        int gyro_reading = 0;
        /// <summary>
        /// Gets or sets the Gyro Reading giving the measured heading of the rover
        /// </summary>
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

        #region Constructor
        public Ev3TrackedExplor3rModel() : base()
        {
            //float step = 100f / 30f;
            //int[] arrayInit = new int[30];
            //for (int i = 0; i < 30; i++)
            //{
            //    arrayInit[i] = (int) (step * i);
            //}
            IRContinuousScan = new ObservableCollection<int>();
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
            message.IsContinuousScanActivated = IsContinuousScanActivated;
            message.RoverSelected = RoverSelected;
            message.KeepRouteCommand = KeepRoute;
            message.RoamingModeActivated = RoamingModeActivated;
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
            DispatcherHelper.CheckBeginInvokeOnUI(() => SingleIrReading = theMessage.SingleIrReading);
            DispatcherHelper.CheckBeginInvokeOnUI(() => HeadMotorPosition = theMessage.HeadMotorPosition);
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
                IRContinuousScan = new ObservableCollection<int>(theMessage.IRContinuousScan));
            DispatcherHelper.CheckBeginInvokeOnUI(() => GyroReading = theMessage.GyroReading);
        }
        #endregion
    }
}
