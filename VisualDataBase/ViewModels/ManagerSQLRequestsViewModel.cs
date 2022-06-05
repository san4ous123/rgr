using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reflection;
using VisualDataBase.Models;

namespace VisualDataBase.ViewModels
{
    public class ManagerSQLRequestsViewModel : ViewModelBase
    {
        private ObservableCollection<Request> _requests;
        private Request _currentRequest;

        private ObservableCollection<TableField> _availableSelectFields;

        public ObservableCollection<Condition>? _currentSelectConditions;
        public Condition? _currentSelectCondition;

        private TableTypes? _typeCurrentJoinTable;
        private ObservableCollection<TableField>? _fieldsJoinRequest;
        public ObservableCollection<Condition>? _joinConditions;

        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set => this.RaiseAndSetIfChanged(ref _requests, value); 
        }
        public Request CurrentRequest
        {
            get
            {
                return _currentRequest;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentRequest, value);
            }
        }

        public static List<TableTypes> AvailableTables { get; } = new List<TableTypes> { TableTypes.Seasons, TableTypes.Nations, TableTypes.Players, TableTypes.PlayersSeasons };
        public ObservableCollection<TableField> AvailableSelectFields 
        { 
            get => _availableSelectFields;
            set => this.RaiseAndSetIfChanged(ref _availableSelectFields,  value);
        }


        public ObservableCollection<Condition>? CurrentSelectConditions
        {
            get { return _currentSelectConditions; }
            set { this.RaiseAndSetIfChanged(ref _currentSelectConditions, value); }
        }
        public Condition? CurrentSelectCondition
        {
            get { return _currentSelectCondition; }
            set { this.RaiseAndSetIfChanged(ref _currentSelectCondition, value); }
        } // For remove


        public ObservableCollection<Condition>? JoinConditions
        {
            get { return _joinConditions; }
            set { this.RaiseAndSetIfChanged(ref _joinConditions, value); }
        }
        public TableTypes? TypeCurrentJoinTable
        {
            get
            {
                return _typeCurrentJoinTable;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _typeCurrentJoinTable, value);
                //FieldsJoinRequest = UpdateFields(_typeCurrentJoinTable);

                //if (CurrentSelectConditions != null)
                //    CurrentSelectConditions = new ObservableCollection<Condition>();

            }
        }
        public ObservableCollection<TableField>? FieldsJoinRequest
        {
            get { return _fieldsJoinRequest; }
            set { this.RaiseAndSetIfChanged(ref _fieldsJoinRequest, value); }
        }
        public string CurrentFirstJoinOnField { get; set; }
        public string CurrentCommandOperatorJoinOnField { get; set; }
        public string CurrentSecondJoinOnField { get; set; }



        public ReactiveCommand<Unit, List<object>?> ExecuteRequestCommand { get; }
        public ReactiveCommand<int, Unit> AddConditionCommand { get; }
        public ReactiveCommand<int, Unit> RemoveConditionCommand { get; }

        public ManagerSQLRequestsViewModel()
        {
            Requests = new ObservableCollection<Request> { new Request { Title = "Req0" } };
            CurrentRequest = (Request)Requests.First().Clone();
            CurrentRequest.SelectFields = new List<string>();

            ExecuteRequestCommand = ReactiveCommand.Create<List<object>?>(ExecuteRequest);
            AddConditionCommand = ReactiveCommand.Create<int>(AddCondition);
            RemoveConditionCommand = ReactiveCommand.Create<int>(RemoveCondition);
        }

        private List<object>? ExecuteRequest()
        {
            if (SaveRequest() != 0)
                return null;

            IQueryable<object>? result = null;

            switch (CurrentRequest.TypeSelectTable)
            {
                case TableTypes.Seasons:
                    result = from d in MyDataBaseContext.db.Seasons select d;
                    break;
                case TableTypes.Nations:
                    result = from d in MyDataBaseContext.db.Nations select d;
                    break;
                case TableTypes.Players:
                    result = from d in MyDataBaseContext.db.Players select d;
                    break;
                case TableTypes.PlayersSeasons:  
                    result = from d in MyDataBaseContext.db.PlayersSeasons select d;
                    break;
            }

            /*
             * Operator - And 
             * Field - Id
             * OperatorCommand - <
             * Value - 1
             */

            if (CurrentSelectConditions is not null)
            {

            }

            return result.ToList();
        }

        private void AddRequest()
        {
            string title = "Req" + Requests.Count.ToString();

            foreach (var req in Requests)
            {
                if (req.Title == title)
                {
                    title += Requests.Count.ToString();
                    break;
                }
            }

            Requests.Add(new Request { Title = title });
        }

        private void DeleteCurrentRequest()
        {
            Requests.Remove(CurrentRequest);

            if (Requests.Count < 1)
            {
                AddRequest();
            }

            CurrentRequest = Requests.First();
        }

        private int SaveRequest()
        {
            if (!VerifyTitleCurrentRequest())
                return -1;

            if (CurrentRequest.TypeSelectTable is null)
                return -1;

            if (CurrentRequest.SelectFields is null)
                return -1;

            if (CurrentSelectConditions is not null)
            {
                foreach (var cond in CurrentSelectConditions)
                {
                    if (cond.Operator == null || cond.Field == null || cond.OperatorCommand == null || cond.Value == null)
                        return -1;
                }
            }
           

            foreach (var req in Requests)
            {
                if (req.Title == CurrentRequest.Title)
                {
                    req.Title = CurrentRequest.Title;
                    req.TypeSelectTable = CurrentRequest.TypeSelectTable;
                    req.SelectFields = new List<string>(CurrentRequest.SelectFields);

                    if (CurrentSelectConditions != null)
                        req.SelectConditions = new List<Condition>(CurrentSelectConditions);

                    return 0;
                }
            }

            return -1;
        }

        private bool VerifyTitleCurrentRequest()
        {
            if (CurrentRequest.Title == string.Empty)
                return false;

            if (int.TryParse(CurrentRequest.Title, out var parsedNum))
                return false;

            int countRepeatItems = 0;
            foreach (var req in Requests)
                if (req.Title == CurrentRequest.Title)
                    countRepeatItems++;
            if (countRepeatItems > 1)
                return false;

            return true;
        }

        private void AddCondition(int command)
        {
            if (CurrentRequest.TypeSelectTable == null)
                return;

            switch(command)
            {
                case 0: // Select
                    if (CurrentSelectConditions == null)
                        CurrentSelectConditions = new ObservableCollection<Condition>();

                    CurrentSelectConditions.Add(new Condition(CurrentRequest.TypeSelectTable));
                    break;
                case 1: // Join
                    if (JoinConditions == null)
                        JoinConditions = new ObservableCollection<Condition>();

                    JoinConditions.Add(new Condition(CurrentRequest.TypeSelectTable));
                    break;
            }
        }

        private void RemoveCondition(int command)
        {
            if (CurrentSelectConditions == null)
                return;

            switch (command)
            {
                case 0: // Select
                    CurrentSelectConditions.Remove(CurrentSelectCondition);
                    break;

            }
        }
    }
}
