using SkolaProgramiranja.Models;
using SkolaProgramiranja.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkolaProgramiranja
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Prijava_Click(object sender, RoutedEventArgs e)
        {
            var forma = new LoginView();
            
            forma.Show(); 
            this.Close(); 
        }

        private void Btn_SR_Click(object sender, RoutedEventArgs e)
        {
            LanguageManager.ChangeLanguage("sr");
            RestartWindow();
        }

        private void Btn_EN_Click(object sender, RoutedEventArgs e)
        {
            LanguageManager.ChangeLanguage("en");
            RestartWindow();
        }

        private void RestartWindow()
        {
            var noviProzor = new MainWindow();
            noviProzor.Show();
            this.Close();
        }

    }


}