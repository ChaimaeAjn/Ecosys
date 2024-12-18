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
            DataContext = new MainWindowViewModel();  // Lier le ViewModel à la vue
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
