using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace ComputerGraphicsProject.ToolTabsViews
{
    public class ToolTabsViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public ICommand SelectFunctionFiltersCommand { get; }
        public ICommand SelectConvolutionFiltersCommand { get; }
        public ICommand SelectCustomFunctionFiltersCommand { get; }

        public ToolTabsViewModel()
        {
            SelectFunctionFiltersCommand = new RelayCommand(o => CurrentView = new FunctionFiltersView());
            SelectConvolutionFiltersCommand = new RelayCommand(o => CurrentView = new ConvolutionFiltersView());
            SelectCustomFunctionFiltersCommand = new RelayCommand(o => CurrentView = new CustomFunctionFiltersView());

            CurrentView = new FunctionFiltersView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
