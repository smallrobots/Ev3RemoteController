using Smallrobots.Ev3RemoteController.ViewModels;
using System;
using Windows.Gaming.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace Smallrobots.Ev3RemoteController.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class Settings : UserControl
    {
        #region Properties and fields about the GamePad
        double leftThumbStickX;
        /// <summary>
        /// Gets or sets the X position of the left thumbstick
        /// </summary>
        public double LeftThumbStickX
        {
            get => leftThumbStickX;
            set
            {
                if (leftThumbStickX != value)
                {
                    leftThumbStickX = value;
                    ((MainViewModel)DataContext).LeftThumbStickX = leftThumbStickX;
                }
            }
        }

        double leftThumbStickY;
        /// <summary>
        /// Gets or sets the Y position of the left thumbstick
        /// </summary>
        public double LeftThumbStickY
        {
            get => leftThumbStickY;
            set
            {
                if (leftThumbStickY != value)
                {
                    leftThumbStickY = value;
                    ((MainViewModel)DataContext).LeftThumbStickY = leftThumbStickY;
                }
            }
        }

        double rightThumbStickX;
        /// <summary>
        /// Gets or sets the X position of the right Thumbstick
        /// </summary>
        public double RightThumbStickX
        {
            get => rightThumbStickX;
            set
            {
                if (rightThumbStickX != value)
                {
                    rightThumbStickX = value;
                    ((MainViewModel)DataContext).RightThumbStickX = rightThumbStickX;
                }
            }
        }

        bool buttonL1Pressed = false;
        /// <summary>
        /// Gets or sets the condition of Button L1 Pressed
        /// </summary>
        public bool ButtonL1Pressed
        {
            get => buttonL1Pressed;
            set
            {
                if (buttonL1Pressed != value)
                {
                    buttonL1Pressed = value;
                    // Update the view model
                    ((MainViewModel)DataContext).ButtonL1Pressed = buttonL1Pressed;
                }
            }
        }

        bool gameControllerConnected;
        /// <summary>
        /// Get or Sets the game controller connected flag
        /// </summary>
        public bool GameControllerConnected
        {
            get => gameControllerConnected;
            set
            {
                if (gameControllerConnected != value)
                {
                    gameControllerConnected = value;

                    var viewModel = (MainViewModel)DataContext;
                    viewModel.GameControllerConnected = value;
                }
            }
        }

        DispatcherTimer gameControllerUpdateTimer;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;

            // Gamecontroller
            gameControllerUpdateTimer = new DispatcherTimer();
            gameControllerUpdateTimer.Tick += gameControllerUpdateTimer_Callback;
            gameControllerUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            GameControllerConnected = false;
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
        }
        #endregion

        #region UI event handlers
        /// <summary>
        /// This has to be removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;

            // Set initial state for the visual state manager 
            vm.VsmConnectionStatus = "CanConnect_Or_Ping";
        }

        /// <summary>
        /// This event handler is used to scroll the log textbox automatically
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var grid = (Grid)VisualTreeHelper.GetChild(textBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer)) continue;
                ((ScrollViewer)obj).ChangeView(0.0f, ((ScrollViewer)obj).ExtentHeight, 1.0f, true);
                break;
            }
        }
        #endregion

        #region Gamepad event handlers
        /// <summary>
        /// It is called periodically to check for user input throught the gamepad
        /// </summary>
        void gameControllerUpdateTimer_Callback(object sender, object e)
        {
            if (GameControllerConnected)
            {
                Gamepad gamePad = Gamepad.Gamepads[0];

                // Readings
                GamepadReading readings = gamePad.GetCurrentReading();
                LeftThumbStickX = readings.LeftThumbstickX;
                LeftThumbStickY = readings.LeftThumbstickY;
                RightThumbStickX = readings.RightThumbstickX;

                // Buttons
                // Parte da modificare 03/11/2018
                ButtonL1Pressed = GamepadButtons.LeftShoulder == (readings.Buttons & GamepadButtons.LeftShoulder);
            }
        }

        /// <summary>
        /// It is called upon gamepad detection
        /// </summary>
        async void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            await Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal, () =>
                    {
                        GameControllerConnected = true;
                        gameControllerUpdateTimer.Start();
                    });

        }

        /// <summary>
        /// It is called upon gamepad removal
        /// </summary>
        async void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            await Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal, () =>
                    {
                        GameControllerConnected = false;
                        gameControllerUpdateTimer.Stop();
                    });
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
