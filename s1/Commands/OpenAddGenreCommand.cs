using s1.Classes;
using s1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace s1.Commands
{
    internal class OpenAddGenreCommand : ICommand
    {
        ViewModel viewModel;
        public OpenAddGenreCommand(ViewModel ViewModel)
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
            viewModel.AddGenre();
        }
    }
}
