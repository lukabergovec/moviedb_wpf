using s1.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace s1.Views
{
    /// <summary>
    /// Interaction logic for EditMovieWindow.xaml
    /// </summary>
    public partial class EditMovieWindow : Window
    {
        private ViewModel viewModel;
        public EditMovieWindow(ViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
        }
        internal void DeleteActor(object sender, MouseButtonEventArgs e)
        {
            viewModel.ActorsInputList.Remove(viewModel.SelectedActor);
            viewModel.SelectedActor = null;
            Debug.WriteLine("brisi");
        }
    }
}
