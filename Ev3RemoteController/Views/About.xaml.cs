using Windows.UI.Xaml.Controls;

namespace Smallrobots.Ev3RemoteController.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class About : UserControl
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public About()
        {
            InitializeComponent();
        }
        #endregion

        #region UI Handlers
        /// <summary>
        /// This is used to avoid that Gamepad switches from one page to the other
        /// </summary>
        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Up ||
                e.Key == Windows.System.VirtualKey.Down ||
                e.Key == Windows.System.VirtualKey.Left ||
                e.Key == Windows.System.VirtualKey.Right)
            {
                // Nothing
                e.Handled = true;
            }

            if (e.Key == Windows.System.VirtualKey.GamepadLeftThumbstickUp ||
                e.Key == Windows.System.VirtualKey.GamepadLeftThumbstickDown ||
                e.Key == Windows.System.VirtualKey.GamepadLeftThumbstickLeft ||
                e.Key == Windows.System.VirtualKey.GamepadLeftThumbstickRight)
            {
                // Nothing
                e.Handled = true;
            }
        }
        #endregion
    }
}
