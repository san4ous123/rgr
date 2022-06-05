using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using VisualDataBase.ViewModels;

namespace VisualDataBase.Views
{
    public partial class DataBaseVisualView : UserControl
    {
        public DataBaseVisualView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CloseTableTabItem(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null)
            {
                var context = this.DataContext as DataBaseVisualViewModel;
                if (context != null)
                {
                    context.TableTabItems.Remove(btn.DataContext as TableTabItemViewModel);
                }
            }
        }
    }
}
