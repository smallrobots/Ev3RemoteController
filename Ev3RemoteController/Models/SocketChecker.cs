using System.Net;
using System;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Windows.Networking;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class SocketChecker
    {
        #region Fields
        /// <summary>
        /// Host to ping
        /// </summary>
        IPAddress hostIpAddres;

        /// <summary>
        /// Host port of service to ping
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
        /// String used as logbook
        /// </summary>
        string logString;

        /// <summary>
        /// Message received back from the Robot
        /// </summary>
        int messagesReceived;

        bool connectionInProgress;
        #endregion

        #region Costructors
        /// <summary>
        /// Constructs a pinger object which can ping to the specified Host IP Address,
        /// and log the results to the supplied logstring
        /// </summary>
        /// <param name="hostIpAddres"></param>
        public SocketChecker(IPAddress hostIpAddres, int hostPort, 
            IPAddress controllerIpAddress,
            int controllerIpPort)
        {
            // Pinger parameters
            this.hostIpAddres = hostIpAddres;
            this.remoteHostPort = hostPort;
            this.controllerIpAddress = controllerIpAddress;
            this.controllerIpPort = controllerIpPort;
            this.logString = "";


            // Fields initialization
            connectionInProgress = false;

        }
        #endregion

        #region Methods
        public async Task<string> StartPingAsync()
        {
            logString += "\nStarting check to address: " + hostIpAddres.ToString();

            if (connectionInProgress)
                return "\nAlready processing...";

            connectionInProgress = true;

            // await TestTcpIP();

            await TestUdpIP();

            connectionInProgress = false;

            return logString;
        }

        async Task TestUdpIP()
        {
            // Message to the robot 
            Ev3RobotMessage message;

            // Writer to the DatagramSocket
            DataWriter writer;

            using (var udpClient = new DatagramSocket())
            {
                try
                {
                    // String containing the serializaed message
                    string serializedMessage = "";

                    //udpClient.MessageReceived += UdpClient_MessageReceived;
                    //var controllerName = new Windows.Networking.HostName(controllerIpAddress.ToString());
                    //await udpClient.BindEndpointAsync(controllerName, controllerIpPort.ToString());

                    //var remoteHostName = new Windows.Networking.HostName(hostIpAddres.ToString());
                    //await udpClient.ConnectAsync(remoteHostName, remoteHostPort.ToString());

                    var controllerName = new HostName(controllerIpAddress.ToString());
                    var remoteHostName = new HostName(hostIpAddres.ToString());
                    EndpointPair endpointpar = new EndpointPair(controllerName,
                                                                controllerIpPort.ToString(),
                                                                remoteHostName,
                                                                remoteHostPort.ToString());
                    udpClient.MessageReceived += UdpClient_MessageReceived;
                    await udpClient.ConnectAsync(endpointpar);

                    // Create a subscribe message
                    message = new Ev3RobotMessage
                    {
                        MessageFunction = MessageType.subscribe,
                        RemoteControllerAddress = controllerIpAddress.ToString(),
                        RemoteControllerPort = controllerIpPort.ToString()
                    };
                    serializedMessage = JsonConvert.SerializeObject(message);

                    // Reset the counter of messages received back from the remote robot
                    messagesReceived = 0;

                    // Send the message
                    logString += "\nSending subscribe message...";
                    writer = new DataWriter(udpClient.OutputStream);
                    writer.WriteString(JsonConvert.SerializeObject(message));
                    await writer.StoreAsync();
                    logString += "\nSubscribe message sent";

                    // Wait for robot status messages
                    logString += "\nWaiting for robot reply...";
                    await Task.Delay(5000);
                    logString += "\nMessages received back from remote robot in 5s: " + messagesReceived.ToString();

                    // Create an unsubscribe messages
                    message = new Ev3RobotMessage
                    {
                        MessageFunction = MessageType.unsubscribe,
                        RemoteControllerAddress = controllerIpAddress.ToString(),
                        RemoteControllerPort = controllerIpPort.ToString()
                    };
                    serializedMessage = JsonConvert.SerializeObject(message);

                    // Reset the counter of messages received back from the remote robot
                    messagesReceived = 0;

                    // Send the message
                    logString += "\nSending unsubscribe message...";
                    writer = new DataWriter(udpClient.OutputStream);
                    writer.WriteString(JsonConvert.SerializeObject(message));
                    await writer.StoreAsync();
                    logString += "\nSubscribe message sent";

                    // Robot should not send any message back
                    logString += "\nWaiting for robot reply...";
                    await Task.Delay(5000);
                    logString += "\nMessages received back from remote robot in 5s: " + messagesReceived.ToString();
                }
                catch
                {

                }
            }
        }

        void UdpClient_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            // Just increment the number of messages received
            messagesReceived++;
        }

        /// <summary>
        /// // Attempt to contact via TCP/IP
        /// </summary>
        async Task TestTcpIP()
        {           
            try
            {
                using (var tcpClient = new StreamSocket())
                {
                    await tcpClient.ConnectAsync(
                        new Windows.Networking.HostName(hostIpAddres.ToString()),
                        remoteHostPort.ToString(),
                        SocketProtectionLevel.PlainSocket);

                    var localIp = tcpClient.Information.LocalAddress.DisplayName;
                    var remoteIp = tcpClient.Information.RemoteAddress.DisplayName;

                    logString += String.Format("\nSuccess, TCP remote server contacted at IP address {0}",
                                                                 remoteIp);
                    tcpClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147013895)
                {
                    logString += "\nError: No such host is known";
                }
                else if (ex.HResult == -2147014836)
                {
                    logString += "\nError: Timeout when connecting (check hostname and port)";
                }
                else
                {
                    logString += "\nError: Exception returned from network stack:\n" + ex.Message;
                }
            }
        }
        #endregion
    }
}
