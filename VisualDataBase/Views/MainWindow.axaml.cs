using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VisualDataBase.ViewModels;

namespace VisualDataBase.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
