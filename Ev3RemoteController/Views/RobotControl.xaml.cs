using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace Smallrobots.Ev3RemoteController.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class RobotControl : Page
    {
        public RobotControl()
        {
            this.InitializeComponent();
        }

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
    }
}
