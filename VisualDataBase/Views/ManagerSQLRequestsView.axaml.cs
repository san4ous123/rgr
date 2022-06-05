using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualDataBase.Models;
using VisualDataBase.ViewModels;

namespace VisualDataBase.Views
{
    public partial class ManagerSQLRequestsView : UserControl
    {
        public ManagerSQLRequestsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ComboBoxSelectTable(object sender, SelectionChangedEventArgs e)
        {
            var context = this.DataContext as ManagerSQLRequestsViewModel;

            Type? classType = null;

            switch (e.AddedItems[0])
            {
                case TableTypes.Seasons:
                    classType = typeof(Season);
                    break;
                case TableTypes.Nations:
                    classType = typeof(Nation);
                    break;
                case TableTypes.Players:
                    classType = typeof(Player);
                    break;
                case TableTypes.PlayersSeasons:
                    classType = typeof(PlayersSeason);
                    break;
            }

            try
            {
                context.AvailableSelectFields = new ObservableCollection<TableField>();

                context.AvailableSelectFields.Add(new TableField("All"));
                foreach (var item in classType.GetProperties())
                {
                    context.AvailableSelectFields.Add(new TableField(item.Name));
                }
            }
            catch { }
        }

        private void FieldsCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            var context = this.DataContext as ManagerSQLRequestsViewModel;

            if (clickedBox.Content.ToString() == "All")
            {
                if (clickedBox.IsChecked.Value)
                {
                    context.CurrentRequest.SelectFields = new List<string>();
                    foreach (var item in context.AvailableSelectFields)
                    {
                        if (item.Title != "All")
                        {
                            item.IsSelected = true;
                            context.CurrentRequest.SelectFields.Add(item.Title);
                        }
                    }
                }
            }
            else
            {
                string changedItem = (string)clickedBox.Content;
                if (clickedBox.IsChecked.Value)
                {
                    if (!context.CurrentRequest.SelectFields.Contains(changedItem))
                    {
                        context.CurrentRequest.SelectFields.Add(changedItem);
                    }
                }
                else
                {
                    if (context.CurrentRequest.SelectFields.Contains(changedItem))
                    {
                        context.CurrentRequest.SelectFields.Remove(changedItem);
                    }
                }
            }
        }
    }
}
