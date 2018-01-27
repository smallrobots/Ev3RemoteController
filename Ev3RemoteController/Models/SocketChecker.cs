using System.Net;
using System;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;

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
        /// String use as log
        /// </summary>
        string logString;

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
            // Create a message
            Ev3RobotMessage message = new Ev3RobotMessage();
            message.MessageFunction = MessageType.subscribe;
            message.RemoteControllerAddress = controllerIpAddress.ToString();
            message.RemoteControllerPort = controllerIpPort.ToString();

            string serializedMessage = JsonConvert.SerializeObject(message);
            
            using (var udpClient = new DatagramSocket())
            {
                try
                {
                    udpClient.MessageReceived += UdpClient_MessageReceived;
                    var controllerName = new Windows.Networking.HostName(controllerIpAddress.ToString());
                    await udpClient.BindEndpointAsync(controllerName, controllerIpPort.ToString());

                    var remoteHostName = new Windows.Networking.HostName(hostIpAddres.ToString());
                    await udpClient.ConnectAsync(remoteHostName, remoteHostPort.ToString());

                    DataWriter writer;
                    writer = new DataWriter(udpClient.OutputStream);

                    writer.WriteString(JsonConvert.SerializeObject(message));

                    await writer.StoreAsync();

                }
                catch
                {

                }
            }
        }

        void UdpClient_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            throw new NotImplementedException();
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
