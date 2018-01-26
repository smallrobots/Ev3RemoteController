///////////////////////////////////
// Ev3TrackedExplor3r            //
// Ev3TrackedExplor3rModel.cs    //
//                               //
// Copyright Smallrobots.it 2017 //
///////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace Smallrobots.Ev3TrackedExplor3r.Shared
{
    public class Ev3TrackedExplor3rModel : Ev3RobotModel
    {
        #region Command to the robot as Properties
        private int leftMotorSpeed_Command;
        /// <summary>
        /// Gets os Sets the speed of the left motor
        /// </summary>
        public int LeftMotorSpeed_Command
        {
            set
            {
                if (leftMotorSpeed_Command != value)
                {
                    leftMotorSpeed_Command = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return leftMotorSpeed_Command;
            }
        }

        private int rightMotorSpeed_Command;
        /// <summary>
        /// Gets or Sets the speed of the right motor
        /// </summary>
        public int RightMotorSpeed_Command
        {
            set
            {
                if (rightMotorSpeed_Command != value)
                {
                    rightMotorSpeed_Command = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return rightMotorSpeed_Command;
            }
        }

        private bool rotateHeadLeft_Command;
        /// <summary>
        /// Gets or Sets the command to rotate the head to the left
        /// </summary>
        public bool RotateHeadLeft_Command
        {
            set
            {
                if (rotateHeadLeft_Command != value)
                {
                    rotateHeadLeft_Command = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return rotateHeadLeft_Command;
            }
        }

        private bool rotateHeadRight_Command;
        /// <summary>
        /// Gets or Sets the command to rotate the head to the right
        /// </summary>
        public bool RotateHeadRight_Command
        {
            set
            {
                if (rotateHeadRight_Command != value)
                {
                    rotateHeadRight_Command = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return rotateHeadRight_Command;
            }
        }

        private bool followLine;
        /// <summary>
        /// Gets or sets the command to follow the line
        /// </summary>
        public bool FollowLine
        {
            set
            {
                if (followLine != value)
                {
                    followLine = value;
                    RaisePropertyChanged();
                }
            }
            get => followLine;
        }
        #endregion

        #region Status of the robot as Properties
        private ulong lastSampleID;
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
        #endregion
    }
}
