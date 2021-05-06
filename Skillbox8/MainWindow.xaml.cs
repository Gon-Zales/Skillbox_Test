using Skillbox.App.ViewModel;
using System.Windows;

namespace Skillbox.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();// obv, a test hack, not a prod version
            Closing += ((MainViewModel)DataContext).RequestClose;
        }
    }
}
