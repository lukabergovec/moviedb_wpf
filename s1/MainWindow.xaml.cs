using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using s1.Classes;
using s1.Commands;
using s1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace s1
{
    public partial class MainWindow : Window
    {

        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;
        }
        private void izhodClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void doubleClick(object sender, MouseButtonEventArgs e)
        {
            Movie selectedMovie = (Movie)moviesList.SelectedItem;
            if (selectedMovie != null)
            {
                MessageBox.Show("Ime filma: " + selectedMovie.Title);
            }
        }
        private void UvoziClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml";
            if (dialog.ShowDialog() == true)
            {
                viewModel.LoadMoviesFromXml(dialog.FileName);
            }
        }
        private void IzvoziClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml";
            if (dialog.ShowDialog() == true)
            {
                viewModel.SaveMoviesToXml(dialog.FileName);
            }
        }
    }
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string[] array)
            {
                return string.Join(", ", array);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}