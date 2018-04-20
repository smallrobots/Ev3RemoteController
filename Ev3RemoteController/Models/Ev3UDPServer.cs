using System.Net;
using System;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Windows.Networking;
using Windows.System.Threading;
using System.IO;
using Windows.Foundation;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3UDPServer : Model
    {
        #region Fields
        /// <summary>
        /// Host to contact
        /// </summary>
        IPAddress remoteHostAddress;

        /// <summary>
        /// Host port of service to contact
        /// </summary>
        int remoteHostPort;

        /// <summary>
        /// This controller Ip address
        /// </summary>
        IPAddress controllerIpAddress;

        /// <summary>
        /// This controller Ip port
        /// </summary>
        int controllerIpPort;

        /// <summary>
        /// UDP listener socket
        /// </summary>
        DatagramSocket udpListener;

        /// <summary>
        /// UDP sender socket
        /// </summary>
        DatagramSocket udpSender;

        /// <summary>
        /// Timer to send outobound UDP packet to the robot
        /// </summary>
        ThreadPoolTimer senderTimer;
        int samplingPeriod = 200;
        #endregion

        #region Properties
        Ev3RobotModel robotModel;
        public Ev3RobotModel RobotModel
        {
            get => robotModel;
            set
            {
                robotModel = value;
                RaisePropertyChanged();
            }
        }

        bool isConnected = false;
        /// <summary>
        /// Gets or sets the connection status
        /// </summary>
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                RaisePropertyChanged();
            }
        }

        string logString = "";
        /// <summary>
        /// Gets or sets the current logstring
        /// </summary>
        public string LogString
        {
            get => logString;
            set
            {
                logString = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="hostIpAddres">IP address of the remote host</param>
        /// <param name="hostPort">IP port of the remote address</param>
        /// <param name="controllerIpAddress">IP address of this controller (used to receive)</param>
        /// <param name="controllerIpPort">IP port of this controller (used to receive)</param>
        public Ev3UDPServer(IPAddress hostIpAddres, int hostPort,
            IPAddress controllerIpAddress,
            int controllerIpPort, Ev3RobotModel robotModel)
        {
            // UDP communication parameters
            this.remoteHostAddress = hostIpAddres;
            this.remoteHostPort = hostPort;
            this.controllerIpAddress = controllerIpAddress;
            this.controllerIpPort = controllerIpPort;
            this.RobotModel = robotModel;

            // Properties initialization
            IsConnected = false;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Start the UDP Server
        /// </summary>
        public async void Start()
        {
            // Start the timer
            senderTimer = ThreadPoolTimer.CreatePeriodicTimer(Sender, new TimeSpan(0, 0, 0, 0, samplingPeriod));

            // Bind the inbound socket
            udpListener = new DatagramSocket();
            udpListener.MessageReceived += UdpListener_MessageReceived;
            await udpListener.BindServiceNameAsync(controllerIpPort.ToString());
        }

        /// <summary>
        /// Stop the UDP Server
        /// </summary>
        public async void Stop()
        {
            // Stop the timer
            senderTimer.Cancel();
            sendUnSubscribeMessage();
            await Task.Delay(5 * samplingPeriod);

            // Close the inbound socket
            udpListener.Dispose();
            udpListener = null;

            // Close the outbound socket
            // udpSender.Dispose();
            udpSender = null;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sender Task called periodically
        /// </summary>
        /// <param name="timer"></param>
        async void Sender(ThreadPoolTimer timer)
        {
            // Check if a connection has been previously already established
            if (udpSender == null)
            {
                // Create and connect the sender
                udpSender = new DatagramSocket();

                // Prepare the connection
                var remoteHostName = new HostName(remoteHostAddress.ToString());

                // Connect sender to the robot remot host
                // Be aware that an UDP connection is always succesfull even if the host does not exist
                LogString = "\n\nAttempting a connection to the remote robot host";
                await udpSender.ConnectAsync(remoteHostName, remoteHostPort.ToString());

                SendSubscribeMessage();
            }
            else
            {
                // Send UDP message on the already open DatagramSocket
                SendCommandMessage();
            }

        }

        /// <summary>
        /// Send the first subscribe message
        /// </summary>
        async void SendSubscribeMessage()
        {
            if (udpSender != null)
            {
                // Prepare the subscribe message
                Ev3RobotMessage message = RobotModel.CreateOutboundMessage();
                message.MessageFunction = MessageType.subscribe;
                message.RemoteControllerAddress = controllerIpAddress.ToString();
                message.RemoteControllerPort = controllerIpPort.ToString();

                // Encode the message
                string serializedMessage = JsonConvert.SerializeObject(message);

                // Send the message
                // LogString = "\nSending subscribe message";
                DataWriter writer;

                writer = new DataWriter(udpSender.OutputStream);
                writer.WriteString(serializedMessage);
                await writer.StoreAsync();
                // LogString = "\nMessage sent";

                // Release the output stream
                writer.DetachStream();
            }
        }

        async void SendCommandMessage()
        {
            if (udpSender != null)
            {
                // Prepare the subscribe message
                Ev3RobotMessage message = RobotModel.CreateOutboundMessage();
                message.MessageFunction = MessageType.command;
                message.RemoteControllerAddress = controllerIpAddress.ToString();
                message.RemoteControllerPort = controllerIpPort.ToString();

                // Encode the message
                string serializedMessage = JsonConvert.SerializeObject(message);

                // Send the message
                // LogString = "\nSending command message";
                DataWriter writer;

                writer = new DataWriter(udpSender.OutputStream);
                writer.WriteString(serializedMessage);
                try
                {
                    await writer.StoreAsync();
                }
                catch (Exception ex)
                {
                    LogString = "\nExcpetion in Ev3UDPServer.sendCommandMessage() " + ex.Message;
                    udpSender = null;
                }

                // Release the output stream
                writer.DetachStream();
                // LogString = "\nMessage sent";
            }
        }

        /// <summary>
        /// Send the last unsubscribe message
        /// </summary>
        async void sendUnSubscribeMessage()
        {
            if (udpSender != null)
            {
                // Prepare the unsubscribe message
                Ev3RobotMessage message = RobotModel.CreateOutboundMessage();
                message.MessageFunction = MessageType.unsubscribe;
                message.RemoteControllerAddress = controllerIpAddress.ToString();
                message.RemoteControllerPort = controllerIpPort.ToString();

                // Encode the message
                string serializedMessage = JsonConvert.SerializeObject(message);

                // Send the message
                LogString = "\nSending unsubscribe message";
                DataWriter writer;

                writer = new DataWriter(udpSender.OutputStream);
                writer.WriteString(serializedMessage);
                await writer.StoreAsync();
                LogString = "\nMessage sent";

                // Release the output stream
                writer.DetachStream();
            }

            udpSender.Dispose();
            udpSender = null;
        }

        /// <summary>
        /// Handler called upon message received on the socket
        /// </summary>
        void UdpListener_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            // Extract the incoming message
            DataReader reader = args.GetDataReader();
            uint bufferLength = reader.UnconsumedBufferLength;
            string inBoundMessage = "";
            try
            {
                // Get the message as a string
                inBoundMessage = reader.ReadString(bufferLength);
            }
            catch (Exception)
            {
                // Handle the exception somehow
            }

            RobotModel.ProcessIncomingMessage(inBoundMessage);

        }
        #endregion
    }
}
