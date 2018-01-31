using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Smallrobots.Ev3RemoteController.Models;
using Windows.Storage;

namespace Smallrobots.Ev3RemoteController.ViewModels
{
    public enum ConnectionState
    {
        CanConnect_Or_Ping = 0,
        Connected,
        CannotConnect_Or_Ping,
        Pinging
    }

    public class MainViewModel : ViewModelBase
    {
        #region Old properties to be removed
        // Update property as usual
        string vsmConnectionStatus = "";
        /// <summary>
        /// Gets or sets the current state of the visual state manager
        /// ConnectionStatus
        /// </summary>
        public string VsmConnectionStatus
        {
            get => vsmConnectionStatus;
            set
            {
                //if (!vsmConnectionStatus.Equals(value))
                //{
                    vsmConnectionStatus = value;
                    RaisePropertyChanged();
                //}
            }
        }
        #endregion

        #region Properties about the gamepad
        double leftThumbStickX;
        /// <summary>
        /// Gets or sets the the X position of the Left Thubsticl
        /// </summary>
        public double LeftThumbStickX
        {
            get => leftThumbStickX;
            set
            {
                if (leftThumbStickX != value)
                {
                    if (Math.Abs(value) > 0.05)
                        leftThumbStickX = value;
                    else
                        leftThumbStickX = 0;
                    RaisePropertyChanged();
                }
            }
        }

        double leftThumbStickY;
        /// <summary>
        /// Gets or sets the the Y position of the Left Thubsticl
        /// </summary>
        public double LeftThumbStickY
        {
            get => leftThumbStickY;
            set
            {
                if (leftThumbStickY != value)
                {
                    if (Math.Abs(value) > 0.05)
                        leftThumbStickY = value;
                    else
                        leftThumbStickY = 0;
                    RaisePropertyChanged();
                }
            }
        }

        bool gameControllerConnected;
        /// <summary>
        /// Get or Sets the game controller connected flag
        /// </summary>
        public bool GameControllerConnected
        {
            get => gameControllerConnected;
            set
            {
                if (gameControllerConnected != value)
                {
                    gameControllerConnected = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("GameControllerConnectedNotification");
                }
            }
        }

        public string GameControllerConnectedNotification
        {
            get => GameControllerConnected ? "Joystick detected" : "No joystick detected";
        }
        #endregion

        #region Properties about the TCP Parameters
        bool robotIpAddress_IsValid = false;
        /// <summary>
        /// Gets or sets a property that indicates whether the Robot IP Address is valid
        /// </summary>
        public bool RobotIpAddress_IsValid
        {
            get => robotIpAddress_IsValid;
            set

            {
                if (robotIpAddress_IsValid != value)
                {
                    robotIpAddress_IsValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        bool robotIpPort_IsValid = false;
        /// <summary>
        /// Gets or sets a property that indicates whether the Robot IP Port is valid
        /// </summary>
        public bool RobotIpPort_IsValid
        {
            get => robotIpPort_IsValid;
            set

            {
                if (robotIpPort_IsValid != value)
                {
                    robotIpPort_IsValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        bool controllerIpAddress_IsValid = false;
        /// <summary>
        /// Gets or sets a property that indicates whether the Controller IP Address is valid
        /// </summary>
        public bool ControllerIpAddress_IsValid
        {
            get => controllerIpAddress_IsValid;
            set

            {
                if (controllerIpAddress_IsValid != value)
                {
                    controllerIpAddress_IsValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        bool controllerIpPort_IsValid = false;
        /// <summary>
        /// Gets or sets a property that indicates whether the Controller IP Port is valid
        /// </summary>
        public bool ControllerIpPort_IsValid
        {
            get => controllerIpPort_IsValid;
            set

            {
                if (controllerIpPort_IsValid != value)
                {
                    controllerIpPort_IsValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        IPAddress robotIpAddress = IPAddress.Parse("0.0.0.0");
        /// <summary>
        /// Gets or sets the Robot IP Address
        /// </summary>
        public string RobotIpAddress
        {
            get => robotIpAddress.ToString();
            set
            {
                // IP Address must be in the form xxx.xxx.xxx.xxx where xxx must be between 0 and 255
                RobotIpAddress_IsValid = IPAddress.TryParse(value, out robotIpAddress) &&
                                         value.Split(". ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length == 4;
                RaisePropertyChanged();

                // Store the property if it is valid
                if (RobotIpAddress_IsValid) StoreLocalSettings();
            }
        }

        int robotIPPort = 0;
        /// <summary>
        /// Gets or sets the Robot IP Port
        /// </summary>
        public string RobotIPPort
        {
            get => robotIPPort.ToString();
            set
            {
                RobotIpPort_IsValid = int.TryParse(value, out robotIPPort);
                if (RobotIpPort_IsValid && ((robotIPPort < 0) || (robotIPPort > 65000)))
                    RobotIpPort_IsValid = false;
                RaisePropertyChanged();

                // Store the property if it is valid
                if (RobotIpPort_IsValid) StoreLocalSettings();
            }
        }

        IPAddress controllerIpAddress = IPAddress.Parse("0.0.0.0");
        /// <summary>
        /// Gets or sets the Controller IP Address
        /// </summary>
        public string ControllerIpAddress
        {
            get => controllerIpAddress.ToString();
            set
            {
                // IP Address must be in the form xxx.xxx.xxx.xxx where xxx must be between 0 and 255
                ControllerIpAddress_IsValid = IPAddress.TryParse(value, out controllerIpAddress) &&
                                         value.Split(". ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length == 4;
                RaisePropertyChanged();

                // Store the property if it is valid
                if (ControllerIpAddress_IsValid) StoreLocalSettings();
            }
        }

        int controllerIPPort = 0;
        /// <summary>
        /// Gets or sets the Robot IP Port
        /// </summary>
        public string ControllerIPPort
        {
            get => controllerIPPort.ToString();
            set
            {
                ControllerIpPort_IsValid = int.TryParse(value, out controllerIPPort);
                if (ControllerIpPort_IsValid && ((controllerIPPort < 0) || (controllerIPPort > 65000)))
                    ControllerIpPort_IsValid = false;
                RaisePropertyChanged();

                // Store the property if it is valid
                if (ControllerIpPort_IsValid) StoreLocalSettings();
            }
        }

        string connectionLog = "";
        /// <summary>
        /// Gets or sets the connection log
        /// </summary>
        public string ConnectionLog
        {
            get => connectionLog;
            set
            {
                connectionLog = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            // Listen to self PropertyChanged event
            PropertyChanged += MainViewModel_PropertyChanged;

            // Command definitions
            Ping = new RelayCommand(Ping_ExecuteAsync, Ping_CanExecute);
            Connect = new RelayCommand(Connect_Execute, Connect_CanExecute);
            Disconnect = new RelayCommand(Disconnect_Execute, Disconnect_CanExecute);

            // Create or retrieve the local application settings
            CreateOrRetrieveLocalSettings();

            // Connection Log
            ConnectionLog = "Ev3 Remote Controller";
            ConnectionLog += "\nConnection Log";
            connectionLog += "\n" + DateTime.Now.ToString();

            // Verify current ConnectionStatus
            InitialConnectionStatus();
        }
        #endregion

        #region Event Handlers
        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RobotIpAddress" ||
                e.PropertyName == "RobotIPPort" ||
                e.PropertyName == "ControllerIpAddress" ||
                e.PropertyName == "ControllerIPPort")
            {
                if (RobotIpAddress_IsValid && RobotIpPort_IsValid &&
                    ControllerIpAddress_IsValid && ControllerIpPort_IsValid)
                    VsmConnectionStatus = "CanConnect_Or_Ping";
                else
                    VsmConnectionStatus = "CannotConnect_Or_Ping";
            }
        }
        #endregion

        #region Connection status
        ConnectionState connectionStatus;
        /// <summary>
        /// Gets or sets the connection status
        /// </summary>
        public ConnectionState ConnectionStatus
        {
            get => connectionStatus;
            set
            {
                connectionStatus = value;
                switch (connectionStatus)
                {
                    case ConnectionState.CanConnect_Or_Ping:
                        VsmConnectionStatus = "CanConnect_Or_Ping";
                        break;
                    case ConnectionState.CannotConnect_Or_Ping:
                        VsmConnectionStatus = "CannotConnect_Or_Ping";
                        break;
                    case ConnectionState.Connected:
                        VsmConnectionStatus = "Connected";
                        break;
                    case ConnectionState.Pinging:
                        VsmConnectionStatus = "Pinging";
                        break;
                }
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        /////////////////////////
        // Ping Command        // 
        /////////////////////////
        public RelayCommand Ping { get; set; }
        bool Ping_CanExecute()
        {
            bool retValue = false;

            if (ConnectionStatus == ConnectionState.CanConnect_Or_Ping)
                retValue = true;

            return retValue;
        }
        async void Ping_ExecuteAsync()
        {
            ConnectionStatus = ConnectionState.Pinging;
            VerifyAllCanExecuteCommands();

            SocketChecker pinger = new SocketChecker(robotIpAddress, robotIPPort,
                                                     controllerIpAddress, controllerIPPort);

            // Subscribe to property changed event to intercept the log
            pinger.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => 
                {
                    SocketChecker sc = (SocketChecker)sender; 
                    if (e.PropertyName.Equals("LogString"))
                    {
                        ConnectionLog += sc.LogString;
                    }
                };

            await pinger.StartPingAsync();

            ConnectionStatus = ConnectionState.CanConnect_Or_Ping;
            VerifyAllCanExecuteCommands();

        }

        /////////////////////////
        // Connect Command     // 
        /////////////////////////
        public RelayCommand Connect { get; set; }
        bool Connect_CanExecute()
        {
            bool retValue = false;

            if (ConnectionStatus == ConnectionState.CanConnect_Or_Ping)
                retValue = true;

            return retValue;
        }
        void Connect_Execute()
        {
            ConnectionStatus = ConnectionState.Connected;
            VerifyAllCanExecuteCommands();
        }

        /////////////////////////
        // Disconnect Command  // 
        /////////////////////////
        public RelayCommand Disconnect { get; set; }
        bool Disconnect_CanExecute()
        {
            bool retValue = false;

            if (ConnectionStatus == ConnectionState.Connected)
                retValue = true;

            return retValue;
        }
        void Disconnect_Execute()
        {
            ConnectionStatus = ConnectionState.CanConnect_Or_Ping;
            VerifyAllCanExecuteCommands();
        }

        /// <summary>
        /// Call all commands to verify CanExecute
        /// </summary>
        void VerifyAllCanExecuteCommands()
        {
            Ping.RaiseCanExecuteChanged();
            Connect.RaiseCanExecuteChanged();
            Disconnect.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private methods
        private void InitialConnectionStatus()
        {
            if (RobotIpPort_IsValid && RobotIpPort_IsValid &&
                ControllerIpPort_IsValid && RobotIpPort_IsValid)
                ConnectionStatus = ConnectionState.CanConnect_Or_Ping;
            else
                ConnectionStatus = ConnectionState.CannotConnect_Or_Ping;
            VerifyAllCanExecuteCommands();

        }

        /// <summary>
        /// Create or retrieve the local application settings
        /// </summary>
        void CreateOrRetrieveLocalSettings()
        {
            // Default values
            string default_RobotIpAddress = "192.168.0.1";
            string default_RobotIPPort = "25000";
            string default_ControllerIpAddress = "127.0.0.1";
            string default_ControllerIPPort = "19000";

            //Check if setting #postsToDownload exists and create it if it does not
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            try
            {
                // Robot IP Address
                var stored_RobotIpAddress = localSettings.Values["RobotIpAddress"];
                if (stored_RobotIpAddress == null)
                {
                    localSettings.Values["RobotIpAddress"] = default_RobotIpAddress;
                    robotIpAddress = IPAddress.Parse(default_RobotIpAddress);
                }
                else
                {
                    robotIpAddress = IPAddress.Parse(stored_RobotIpAddress.ToString());
                }
                robotIpAddress_IsValid = true;

                // Robot IP Port
                var stored_RobotIPPort = localSettings.Values["RobotIPPort"];
                if (stored_RobotIPPort == null)
                {
                    localSettings.Values["RobotIPPort"] = default_RobotIPPort;
                    robotIPPort = int.Parse(default_RobotIPPort);
                }
                else
                {
                    robotIPPort = int.Parse(stored_RobotIPPort.ToString());
                }
                robotIpPort_IsValid = true;

                // Controller IP Address
                var stored_ControllerIpAddress = localSettings.Values["ControllerIpAddress"];
                if (stored_ControllerIpAddress == null)
                {
                    localSettings.Values["ControllerIpAddress"] = default_ControllerIpAddress;
                    controllerIpAddress = IPAddress.Parse(default_ControllerIpAddress);
                }
                else
                {
                    controllerIpAddress = IPAddress.Parse(stored_ControllerIpAddress.ToString());
                }
                controllerIpAddress_IsValid = true;

                // Controller IP Port
                var stored_ControllerIPPort = localSettings.Values["ControllerIPPort"];
                if (stored_ControllerIPPort == null)
                {
                    localSettings.Values["ControllerIPPort"] = default_ControllerIPPort;
                    controllerIPPort = int.Parse(default_RobotIPPort);
                }
                else
                {
                    controllerIPPort = int.Parse(stored_ControllerIPPort.ToString());
                }
                controllerIpPort_IsValid = true;
            }
            catch (Exception ex)
            {
                // Problems retrieving the stored settings
                // Assigning very very default values
                RobotIpAddress = "192.168.0.1";
                RobotIPPort = "25000";
                ControllerIpAddress = "127.0.0.1";
                ControllerIPPort = "19000";

                ConnectionLog += "\n Exception retrieving stored user settings: \n" + ex.ToString() +
                    "\nUsing default values";
            }

        }

        /// <summary>
        /// Store user settings
        /// </summary>
        void StoreLocalSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                localSettings.Values["RobotIpAddress"] = RobotIpAddress;
                localSettings.Values["RobotIPPort"] = RobotIPPort;
                localSettings.Values["ControllerIpAddress"] = ControllerIpAddress;
                localSettings.Values["ControllerIPPort"] = ControllerIPPort;
            }
            catch (Exception ex)
            {
                // Problems storing the local settings
                ConnectionLog += "\n Exception storing user settings: \n" + ex.ToString() +
                    "\nUsing default values";
            }
        }
        #endregion
    }
}
