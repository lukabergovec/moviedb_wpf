using GalaSoft.MvvmLight.Command;
using Microsoft.JScript;
using Microsoft.Win32;
using s1.Classes;
using s1.Commands;
using s1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using System.Text.Json;


namespace s1.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Movie> movies;
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set
            {
                movies = value;
                OnPropertyChanged(nameof(Movies));
            }
        }

        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                selectedMovie = value;
                if (selectedMovie != null)
                {
                    SetNewMovieForEditing();
                }
                OnPropertyChanged(nameof(SelectedMovie));
                OnPropertyChanged(nameof(HasSelectedMovie));
            }
        }
        public bool HasSelectedMovie => SelectedMovie != null;

        private ObservableCollection<Genre> movieGenres;
        public ObservableCollection<Genre> MovieGenres
        {
            get { return movieGenres; }
            set
            {
                movieGenres = value;
                OnPropertyChanged(nameof(MovieGenres));
            }
        }

        private Genre selectedGenre;
        public Genre SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                selectedGenre = value;
                NewGenre = selectedGenre?.Name;
                OnPropertyChanged(nameof(SelectedGenre));
                OnPropertyChanged(nameof(HasSelectedGenre));
            }
        }
        public bool HasSelectedGenre => SelectedGenre != null;

        private string newGenre;
        public string NewGenre
        {
            get { return newGenre; }
            set
            {
                newGenre = value;
                OnPropertyChanged(nameof(NewGenre));
            }
        }

        private Movie newMovie;
        public Movie NewMovie
        {
            get { return newMovie; }
            set
            {
                if (newMovie != value)
                {
                    newMovie = value;
                    OnPropertyChanged(nameof(NewMovie));
                }
            }
        }

        private string newActor;
        public string NewActor
        {
            get { return newActor; }
            set
            {
                if (newActor != value)
                {
                    newActor = value;
                    OnPropertyChanged(nameof(NewActor));
                }
            }
        }

        private ObservableCollection<string> actorsInputList;
        public ObservableCollection<string> ActorsInputList
        {
            get { return actorsInputList; }
            set
            {
                if (actorsInputList != value)
                {
                    actorsInputList = value;
                    OnPropertyChanged(nameof(actorsInputList));
                }
            }
        }

        private string selectedActor;
        public string SelectedActor
        {
            get { return selectedActor; }
            set
            {
                selectedActor = value;
                OnPropertyChanged(nameof(SelectedActor));
            }
        }

        private EditMovieWindow editMovieWindow = null;

        public ICommand AddMovieCommand { get; set; }
        public ICommand DeleteMovieCommand { get; set; }
        public ICommand EditMovieCommand { get; set; }
        public ICommand OpenSettingsWindowCommand { get; set; }
        public ICommand AddGenreCommand { get; set; }
        public ICommand AddActorCommand { get; set; }
        public ICommand AddToFavoritesCommand { get; set; }
        public ICommand EditGenreCommand { get; set; }
        public ICommand DeleteGenreCommand { get; set; }
        public ICommand OpenAddMovieWindowCommand { get; set; }
        public ICommand OpenEditMovieWindowCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }


        public ViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            MovieGenres = new ObservableCollection<Genre>();
            ActorsInputList = new ObservableCollection<string>();

            var movie1 = new Movie
            {
                Title = "Fight Club",
                Year = 1999,
                Genres = new string[] { "Action", "Adventure" },
                Actors = new string[] { "Brad Pitt", "Edward Norton", "Helena Bonham Carter" },
                ImagePath = "\\images\\fightclub.jpg",
                Description = "An insomniac office worker and a devil-may-care soap maker form an underground fight club that evolves into much more."
            };
            Console.Write (Environment.CurrentDirectory);
            var movie2 = new Movie
            {
                Title = "Uncharted",
                Year = 2022,
                Genres = new string[] { "Action" },
                Actors = new string[] { "Tom Holland", "Mark Wahlberg", "Antonio Banderas" },
                ImagePath = "\\images\\uncharted.jpg",
                Description = "Street-smart Nathan Drake is recruited by seasoned treasure hunter Victor \"Sully\" Sullivan to recover a fortune amassed by Ferdinand Magellan, and lost 500 years ago by the House of Moncada."
            };

            var movie3 = new Movie
            {
                Title = "Cocaine bear",
                Year = 2023,
                Genres = new string[] { "Comedy", "Thriller" },
                Actors = new string[] { "Keri Russell", "Alden Ehrenreich", "O'shea Jackson Jr." },
                ImagePath = "\\images\\cb.jpg",
                Description = "An oddball group of cops, criminals, tourists and teens converge on a Georgia forest where a huge black bear goes on a murderous rampage after unintentionally ingesting cocaine."
            };

            Movies.Add(movie1);
            Movies.Add(movie2);
            Movies.Add(movie3);

            LoadGenres();

            AddMovieCommand = new OpenAddMovieCommand(this);
            DeleteMovieCommand = new OpenDeleteMovieCommand(this);
            EditMovieCommand = new OpenEditMovieCommand(this);
            OpenSettingsWindowCommand = new RelayCommand(OpenSettingsWindow);
            OpenAddMovieWindowCommand = new RelayCommand(OpenAddMovieWindow);
            OpenEditMovieWindowCommand = new RelayCommand(OpenEditMovieWindow);
            AddToFavoritesCommand = new RelayCommand(AddToFavorites);
            AddGenreCommand = new OpenAddGenreCommand(this);
            AddActorCommand = new RelayCommand(AddActor);
            EditGenreCommand = new OpenEditGenreCommand(this);
            DeleteGenreCommand = new OpenDeleteGenreCommand(this);
            SelectImageCommand = new OpenSelectImageCommand(this);
        }

        internal void AddMovie(object obj)
        {
            var selectedGenres = MovieGenres.Where(g => g.IsSelected);
            var selectedGenresString = selectedGenres.Select(g => g.Name).ToArray();
            NewMovie.Genres = selectedGenresString;
            NewMovie.Actors = ActorsInputList.ToArray();
            Movies.Add(NewMovie);

            ClearNewMovie();
            Window win = obj as Window;
            win.Close();
        }

        internal void DeleteMovie()
        {
            Movies.Remove(SelectedMovie);
            SelectedMovie = null;
        }

        internal void EditMovie(object obj)
        {
            var selectedGenres = MovieGenres.Where(g => g.IsSelected);
            var selectedGenresString = selectedGenres.Select(g => g.Name).ToArray();
            NewMovie.Genres = selectedGenresString;
            NewMovie.Actors = ActorsInputList.ToArray();
            var index = Movies.IndexOf(SelectedMovie);
            Movies[index] = NewMovie;
            SelectedMovie = Movies[index];

            NewMovie = new Movie { ImagePath = "..\\..\\images\\clickToAdd.png" };
            ActorsInputList.Clear();
            ICollectionView view = CollectionViewSource.GetDefaultView(Movies);
            view.Refresh();
            Window win = obj as Window;
            win.Close();
        }

        internal void AddGenre()
        {

            if (!string.IsNullOrEmpty(NewGenre))
            {
                Genre addGenre = new Genre { Name = NewGenre };
                MovieGenres.Add(addGenre);
                NewGenre = "";
                SaveGenres();
            }
        }

        internal void EditGenre()
        {
            if (!string.IsNullOrEmpty(NewGenre))
            {
                Genre editGenre = new Genre { Name = NewGenre };
                var index = MovieGenres.IndexOf(SelectedGenre);
                MovieGenres[index] = editGenre;
                SelectedGenre = editGenre;
                NewGenre = "";
                SaveGenres();
            }
        }

        internal void DeleteGenre()
        {
            MovieGenres.Remove(SelectedGenre);
            SelectedGenre = null;
            SaveGenres();
        }

        private void AddActor()
        {
            if (!string.IsNullOrEmpty(NewActor))
            {
                ActorsInputList.Add(NewActor);
                Debug.WriteLine("gj " + ActorsInputList);
                NewActor = "";
            }
        }

        internal void SelectImage(object sender)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                Image image = sender as Image;
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                NewMovie.ImagePath = openFileDialog.FileName;
            }
        }
        internal void SetNewMovieForEditing()
        {
            ClearNewMovie();
            NewMovie = SelectedMovie;
            foreach (var genre in NewMovie.Genres)
            {
                var genreObj = MovieGenres.FirstOrDefault(g => g.Name == genre);
                if (genreObj != null)
                {
                    Debug.WriteLine("gnr " + genre + "gnrobj " + genreObj.Name);

                    genreObj.IsSelected = true;
                }
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(MovieGenres);
            view.Refresh();
            foreach (var actor in SelectedMovie.Actors)
            {
                ActorsInputList.Add(actor);
            }
        }
        internal void ClearNewMovie()
        {
            NewMovie = new Movie { ImagePath = "..\\..\\images\\clickToAdd.png", };
            var selectedGenres = MovieGenres.Where(g => g.IsSelected);
            foreach (var genre in selectedGenres)
            {
                genre.IsSelected = false;
            }
            ActorsInputList.Clear();
        }
        internal void AddToFavorites()
        {
            var editedMovie = SelectedMovie;
            editedMovie.Favorite = !editedMovie.Favorite;
            var index = Movies.IndexOf(SelectedMovie);
            Movies[index] = editedMovie;
            SelectedMovie = editedMovie;
            ICollectionView view = CollectionViewSource.GetDefaultView(Movies);
            view.Refresh();
        }
        internal void SaveGenres()
        {
            StringCollection Genres = new StringCollection();
            foreach (var genre in MovieGenres)
            {
                Genres.Add(genre.Name);
            }
            Properties.Settings.Default.Genres = Genres;
            Properties.Settings.Default.Save();
        }
        internal void LoadGenres()
        {
            MovieGenres.Clear();

            if (Properties.Settings.Default.Genres == null)
            {
                Properties.Settings.Default.Genres = new System.Collections.Specialized.StringCollection();
            }
            else
            {
                StringCollection Genres = Properties.Settings.Default.Genres;
                foreach (var genre in Genres)
                {
                    MovieGenres.Add(new Genre { Name = genre });
                }
            }
        }
        internal void OpenSettingsWindow()
        {
            SettingsWindow settingsWindow = new SettingsWindow(this);
            settingsWindow.Owner = App.Current.MainWindow;
            settingsWindow.ShowDialog();
        }
        internal void OpenAddMovieWindow()
        {
            ClearNewMovie();
            AddMovieWindow addMovieWindow = new AddMovieWindow(this);
            addMovieWindow.Owner = App.Current.MainWindow;
            addMovieWindow.ShowDialog();
        }
        internal void OpenEditMovieWindow()
        {
            if (editMovieWindow == null)
            {
                editMovieWindow = new EditMovieWindow(this);
                editMovieWindow.Owner = App.Current.MainWindow;
                editMovieWindow.Show();
            }
            else
            {
                editMovieWindow.Activate();
            }
        }
        internal void SaveMoviesToXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Movie>));
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                serializer.Serialize(stream, Movies);
            }
        }
        internal void LoadMoviesFromXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Movie>));
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                Movies = serializer.Deserialize(stream) as ObservableCollection<Movie>;
            }
        }
    }
}
