﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace arcmap_layer_search.Utilities
{
    /// <summary>
    /// RelayCommand is used to generate commands in the MVVM design pattern
    /// </summary>
    /// <example>
    /// <code>
    /// private RelayCommand _commandStart;
    /// 
    /// public RelayCommand CommandStart
    /// {
    ///     get
    ///     {
    ///         if (_commandStart == null)
    ///             _commandStart = new RelayCommand(ExecuteStart, CanExecuteStart);
    ///         return _commandStart;
    ///     }
    /// }
    /// 
    /// private void ExecuteStart(object parameter)
    /// {
    ///     // Some other methods
    /// }
    ///
    /// private bool CanExecuteStart(object parameter)
    /// {
    ///     return true;
    /// }
    /// </code>
    /// </example>
    /// <remarks></remarks>
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExcute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("Execute method must exist!");
            }
            this.execute = execute;
            this.canExcute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExcute != null)
            {
                return canExcute(parameter);
            }
            return true;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
