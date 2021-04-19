using Skillbox8.ViewModel;
using System.Windows;

namespace Skillbox_8
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
