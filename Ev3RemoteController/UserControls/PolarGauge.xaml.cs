using System;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace Smallrobots.Ev3RemoteController.UserControls
{
    public sealed partial class PolarGauge : UserControl
    {

        #region Dependentcy Properties
        /// <summary>
        /// Gets or sets the IRContinuousScan
        /// </summary>
        public ObservableCollection<int> IRContinuousScan
        {
            get => (ObservableCollection<int>)GetValue(IRContinuousScanProperty);
            set => SetValue(IRContinuousScanProperty, value);
        }

        /// <summary>
        /// IRContinuousScan Dependency Property
        /// </summary>
        public static readonly DependencyProperty IRContinuousScanProperty
            = DependencyProperty.Register("IRContinuousScan", typeof(ObservableCollection<int>), typeof(PolarGauge), 
                new PropertyMetadata(null, new PropertyChangedCallback(updateScanListCartesian)));

        private static void updateScanListCartesian(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ObservableCollection<int> updatedValue = (ObservableCollection<int>) e.NewValue;
            for (int i = 0; i< updatedValue.Count; i++)
            {
                float angle = step / 2 + step * i;
                Point position = PolarToCanvas(angle, updatedValue[i], defaultTransform);
                Canvas.SetLeft(ellipses[i], position.X);
                Canvas.SetTop(ellipses[i], position.Y);
            }
        }

        /// <summary>
        /// Gets or sets the Angle
        /// </summary>
        public string NeedleAngle
        {
            get => (string)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        /// <summary>
        /// Angle property
        /// </summary>
        public static readonly DependencyProperty AngleProperty
            = DependencyProperty.Register("NeedleAngle", typeof(string), typeof(PolarGauge), null);

        #endregion

        #region Properties

        #endregion

        #region Fields
        /// <summary>
        /// IR Scan markers
        /// </summary>
        static Point defaultTransform = new Point(106, 116);
        static float needleMagnitude = 98;
        static int ellipsesNumber = 30;
        static float step = 180f / ellipsesNumber;
        static Ellipse[] ellipses = new Ellipse[ellipsesNumber];
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public PolarGauge()
        {
            InitializeComponent();

            // Create 30 IR Scan markers
            for (int i = 0; i < ellipsesNumber; i++)
            {
                ellipses[i] = new Ellipse()
                {
                    Width = 4.4,
                    Height = 4.4,
                    Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x84, 0x45, 0x00)),
                    StrokeThickness = 0.2,
                    Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
                    StrokeMiterLimit = 4,
                    StrokeLineJoin = PenLineJoin.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                IrScan.Children.Add(ellipses[i]);
                float angle = step/2 + step * i;
                Point position = PolarToCanvas(angle, needleMagnitude, defaultTransform);
                Canvas.SetLeft(ellipses[i], position.X);
                Canvas.SetTop(ellipses[i], position.Y);
            }
        }
        #endregion

        #region Private methods
        static Point PolarToCanvas(float angle, float magnitude, Point transform)
        {
            Point retValue = new Point(0, 0);

            retValue.X = magnitude * Math.Cos(Math.PI / 180 * angle);
            retValue.Y = magnitude * Math.Sin(Math.PI / 180 * angle);

            if (transform != null)
            {
                retValue.X = -retValue.X + transform.X;
                retValue.Y = -retValue.Y + transform.Y;
            }

            return retValue;
        }
        #endregion
    }
}
