//////////////////////////////////////////////////////////
// Mosaico.Shared                                       //
// Model.cs                                             //
//                                                      //
// Copyright Mosaico Monitoraggio Integrato s.r.l. 2015 //
//////////////////////////////////////////////////////////

// Inclusioni di progetto
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Smallrobots.Ev3RemoteController.Models
{
    //[Serializable]
    public abstract partial class Model : object, INotifyPropertyChanged
    {
        #region Proprietà
        [JsonIgnore]
        private bool notificationsEnabled;
        /// <summary>
        /// Ottiene o imposta l'abilitazione alla notifica del cambiamento di una delle proprietà del modello
        /// Se False, la modifica non viene notificata
        /// Se True, la modifica viene notificata
        /// </summary>
        [JsonIgnore]
        public bool NotificationsEnabled
        {
            get
            {
                return notificationsEnabled;
            }
            set
            {
                if (notificationsEnabled != value)
                {
                    notificationsEnabled = value;
                    RaisePropertyChanged("NotificationsEnabled");
                    return;
                }
            }
        }
        #endregion

        #region Costruttori
        /// <summary>
        /// Costruttore di default
        /// </summary>
        public Model() : base()
        {
            // Inizializzazione campi
            notificationsEnabled = true;
            return;
        }
        #endregion

        #region Implementazione interfaccia INotifyPropertyChanged
        /// <summary>
        /// Evento da lanciato quando il valore di una proprietà cambia
        /// </summary>
        //[field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metodo di notifica del cambiamento del valore di una proprietà
        /// </summary>
        /// <param name="propertyName">Identificatico della proprietà di cui notificare il cambiamento</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (NotificationsEnabled)
            {
                // Verifica la registrazione dell'evento
                if (PropertyChanged != null)
                {
                    // Invoca l'evento registrato
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                }


            }
            return;
        }

        #endregion

        
    }
}

