using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EcoSys.ViewModels;

namespace EcoSys.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}