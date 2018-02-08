/////////////////////////////////////////////////////
// Ev3TrackedExplor3r                              //
// Ev3TrackedExplor3r_TCPRemoteConnection.cs       //
//                                                 //
// Copyright Smallrobots.it 2017                   //
////////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3TrackedExplor3r_TCPRemoteConnection : Model
    {
        #region Properties
        private Ev3TrackedExplor3rModel ev3RobotModel;
        /// <summary>
        /// Gets or sets the Ev3RobotModel for this connection
        /// </summary>
        public Ev3TrackedExplor3rModel Ev3RobotModel
        {
            get => ev3RobotModel;
            set
            {
                if (ev3RobotModel != value)
                {
                    ev3RobotModel = value;
                    RaisePropertyChanged("");
                }
            }
        }

        private IPAddress hostIP;
        /// <summary>
        /// Gets or Sets the IP Address for this connection
        /// </summary>
        public IPAddress HostIP
        {
            get => hostIP;
            set
            {
                if (hostIP != value)
                {
                    hostIP = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string hostPort;
        /// <summary>
        /// Gets or sets the host port for this connection
        /// </summary>
        public string HostPort
        {
            get => hostPort;
            set
            {
                if (!hostPort.Equals(value))
                {
                    hostPort = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool connected;
        /// <summary>
        /// Gets a bool, true if the connection has been established
        /// </summary>
        public bool Connected
        {
            get => connected;
            private set
            {
                if (connected != value)
                {
                    connected = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Fields
        StreamSocket socket;
        ThreadPoolTimer updateTimer;
        bool updating;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Ev3TrackedExplor3r_TCPRemoteConnection()
        {
            init();
        }

        /// <summary>
        /// Constructs a TCP remote connection to a Ev3 Host with the model specified
        /// </summary>
        /// <param name="model">Model of the Ev3 host to connect to</param>
        public Ev3TrackedExplor3r_TCPRemoteConnection(Ev3TrackedExplor3rModel model)
        {
            init();
            ev3RobotModel = model;
        }

        /// <summary>
        /// Default intialization
        /// </summary>
        private void init()
        {
            hostIP = IPAddress.Parse("0.0.0.0");
            hostPort = "8001";
            updateTimer = null;
            updating = false;
        }
        #endregion

        public bool CheckConnectionStatus()
        {
            return Connected;
        }

        public bool CloseConnection()
        {
            Connected = false;
            updateTimer.Cancel();
            return true;
        }

        public void StartConnection()
        {
            // Start the update timer
            updateTimer = ThreadPoolTimer.CreatePeriodicTimer(updateRemoteEv3Host, new TimeSpan(0, 0, 0, 0, 100));
            
            Connected = true;
         }

        public void StopConnection()
        {
            // Stop the update timer
            updateTimer.Cancel();
            Connected = false;
        }
        
        private async void updateRemoteEv3Host(ThreadPoolTimer timer)
        {
            if (!updating)
            {
                // Set the semaphore
                updating = true;

                // Open the socket
                socket = new StreamSocket();
                HostName hostName = new HostName(HostIP.ToString());
                socket.Control.KeepAlive = false;
                await socket.ConnectAsync(hostName, hostPort);

                // Prepare message to send
                DataWriter writer = new DataWriter(socket.OutputStream);
                string messageOut = JsonConvert.SerializeObject(ev3RobotModel);

                // Send the message to the remote Ev3 Host
                writer.WriteInt32(messageOut.Length);
                writer.WriteString(messageOut);
                await writer.StoreAsync();
                writer.DetachStream();

                // Wait for response
                DataReader reader = new DataReader(socket.InputStream);
                await reader.LoadAsync(sizeof(Int32));
                uint bufferLength = reader.ReadUInt32();
                await reader.LoadAsync(bufferLength);
                string messageIn = reader.ReadString(bufferLength);
                reader.DetachStream();

                // Deserialize the message
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                Ev3TrackedExplor3rModel newModel = JsonConvert.DeserializeObject<Ev3TrackedExplor3rModel>(messageIn, settings);
                ev3RobotModel.LastSampleID = newModel.LastSampleID;
                ev3RobotModel.LeftMotorSpeed_Command = newModel.LeftMotorSpeed_Command;
                ev3RobotModel.RightMotorSpeed_Command = newModel.RightMotorSpeed_Command;
                ev3RobotModel.RotateHeadLeft_Command = newModel.RotateHeadLeft_Command;
                ev3RobotModel.RotateHeadRight_Command = newModel.RotateHeadRight_Command;

                // Releasing the socket
                socket.Dispose();

                // Resetting the semaphore
                updating = false;
            }
        }
    }
}
