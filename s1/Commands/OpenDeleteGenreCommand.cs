using s1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace s1.Commands
{
    internal class OpenDeleteGenreCommand : ICommand
    {
        private readonly ViewModel viewModel;

        public OpenDeleteGenreCommand(ViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return viewModel.HasSelectedGenre;
        }

        public void Execute(object parameter)
        {
            viewModel.DeleteGenre();
        }
    }
}
