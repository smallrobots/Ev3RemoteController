using System.Net;
using System;
using Windows.Networking.Sockets;
using System.Threading.Tasks;

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
        int hostPort;

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
        public SocketChecker(IPAddress hostIpAddres, int hostPort)
        {
            // Pinger parameters
            this.hostIpAddres = hostIpAddres;
            this.logString = "";
            this.hostPort = hostPort;

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

            try
            {
                using (var tcpClient = new StreamSocket())
                {
                    await tcpClient.ConnectAsync(
                        new Windows.Networking.HostName(hostIpAddres.ToString()),
                        hostPort.ToString(),
                        SocketProtectionLevel.PlainSocket);

                    var localIp = tcpClient.Information.LocalAddress.DisplayName;
                    var remoteIp = tcpClient.Information.RemoteAddress.DisplayName;

                    logString += String.Format("\nSuccess, remote server contacted at IP address {0}",
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
            finally
            {
                connectionInProgress = false;
            }

            return logString;
        }
        #endregion
    }
}
