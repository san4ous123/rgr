using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace VisualDataBase.ViewModels
{
    public class TableTabItemBase : ViewModelBase
    {
        private ObservableCollection<object> _content;

        public string Title { get; }
        public bool IsRequestTable { get; }
        public ObservableCollection<object> Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public TableTabItemBase(string title, bool isRequest)
        {
            Title = title;
            IsRequestTable = isRequest;
        }
    }
}
