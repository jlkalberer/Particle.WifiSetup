namespace Particle.WifiSetup
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// The relay command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The method to execute.
        /// </summary>
        private readonly Action<object> methodToExecute;

        /// <summary>
        /// The can execute evaluator.
        /// </summary>
        private readonly Func<object, bool> canExecuteEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="methodToExecute">
        /// The method to execute.
        /// </param>
        /// <param name="canExecuteEvaluator">
        /// The can execute evaluator.
        /// </param>
        public RelayCommand(Action<object> methodToExecute, Func<object, bool> canExecuteEvaluator = null)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null)
            {
                return true;
            }
            
            var result = this.canExecuteEvaluator.Invoke(parameter);
            return result;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke(parameter);
        }
    }
}
