using Smallrobots.Ev3RemoteController.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace Smallrobots.Ev3RemoteController.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;

            // Set initial state for the visual state manager 
            vm.VsmConnectionStatus = "CanConnect_Or_Ping";
        }
    }
}
