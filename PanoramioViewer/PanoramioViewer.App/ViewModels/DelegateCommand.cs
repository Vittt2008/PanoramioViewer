using System;
using System.Windows.Input;

namespace PanoramioViewer.App.ViewModels
{
	public class DelegateCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Func<bool> _canExecute;

		public bool CanExecute(object parameter)
		{
			return _canExecute();
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public DelegateCommand(Action<object> execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public DelegateCommand(Action<object> execute) : this(execute, () => true)
		{

		}
	}
}