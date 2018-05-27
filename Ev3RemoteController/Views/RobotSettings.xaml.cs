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

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace Smallrobots.Ev3RemoteController.Views
{
    public sealed partial class RobotSettings : UserControl
    {
        #region Fields
        RobotSettings_Ev3TrackedExplor3r rsEvte = null;
        RobotSettings_Ev3TrackedExplor3r_MarkII rsEvte2 = null;
        RobotSettings_IRScanTester irScan = null;
        #endregion

        public RobotSettings()
        {
            InitializeComponent();         
        }

        private void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainViewArea != null)
            {
                if (((ComboBoxItem)cmbRoverType.SelectedItem).Content.Equals("Tracked Explor3r"))
                {
                    if (rsEvte == null)
                        rsEvte = new RobotSettings_Ev3TrackedExplor3r();
                    mainViewArea.Content = rsEvte;
                }
                if (((ComboBoxItem)cmbRoverType.SelectedItem).Content.Equals("IRScan tester"))
                {
                    if (rsEvte2 == null)
                        rsEvte2 = new RobotSettings_Ev3TrackedExplor3r_MarkII();
                    mainViewArea.Content = rsEvte2;
                }
                if (((ComboBoxItem)cmbRoverType.SelectedItem).Content.Equals("Tracked Explor3r Mark II"))
                {
                    if (irScan == null)
                        irScan = new RobotSettings_IRScanTester();
                    mainViewArea.Content = irScan;
                }
            }
            return;
        }
    }
}
