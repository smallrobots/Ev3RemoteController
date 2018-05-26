using Smallrobots.Ev3RemoteController.Views;
using Windows.UI.Xaml.Controls;

namespace Ev3RemoteController
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Fields
        About aboutView = null;
        RobotTelemetry robotTelemetryView = null;
        Settings settingsView = null;
        RobotSettings robotSettingsView = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            aboutView = new About();
            robotTelemetryView = new RobotTelemetry();
            settingsView = new Settings();
            robotSettingsView = new RobotSettings();

            // Initial View
            mainViewArea.Content = robotTelemetryView;
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

        private void View_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender.Equals(settings_Button))
                mainViewArea.Content = settingsView;
            if (sender.Equals(robot_settings_Button))
                mainViewArea.Content = robotSettingsView;
            if (sender.Equals(drive_Button))
                mainViewArea.Content = robotTelemetryView;
            if (sender.Equals(about_button))
                mainViewArea.Content = aboutView;
        }
    }
}
