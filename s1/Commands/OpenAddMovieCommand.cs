using s1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace s1.Commands
{
    internal class OpenAddMovieCommand : ICommand
    {
        ViewModel viewModel;
        public OpenAddMovieCommand( ViewModel ViewModel)
        {
            viewModel = ViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.AddMovie(parameter);
        }
    }
}
