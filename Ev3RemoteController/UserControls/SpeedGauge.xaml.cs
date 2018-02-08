using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace Smallrobots.Ev3RemoteController.UserControls
{
    public sealed partial class SpeedGauge : UserControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Speed
        /// </summary>
        public string Speed
        {
            get => (string)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        /// <summary>
        /// Speed
        /// </summary>
        public static readonly DependencyProperty SpeedProperty
            = DependencyProperty.Register("Speed", typeof(string), typeof(SpeedGauge), null);
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public SpeedGauge()
        {
            InitializeComponent();
        }
        #endregion
    }
}
