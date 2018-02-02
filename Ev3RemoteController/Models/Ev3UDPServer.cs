using System.Net;
using System;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Windows.Networking;

namespace Smallrobots.Ev3RemoteController.Models
{
    public class Ev3UDPServer : Model
    {
        #region Fields
        /// <summary>
        /// Host to contact
        /// </summary>
        IPAddress hostIpAddres;

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
        #endregion

        #region Constructor

        #endregion
    }
}
