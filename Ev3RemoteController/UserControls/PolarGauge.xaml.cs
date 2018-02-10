using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace Smallrobots.Ev3RemoteController.UserControls
{
    public sealed partial class PolarGauge : UserControl
    {
        /// <summary>
        /// Gets or sets the Angle
        /// </summary>
        public string NeedleAngle
        {
            get => (string)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        /// <summary>
        /// Angle
        /// </summary>
        public static readonly DependencyProperty AngleProperty
            = DependencyProperty.Register("NeedleAngle", typeof(string), typeof(PolarGauge), null);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolarGauge()
        {
            this.InitializeComponent();
        }
    }
}
