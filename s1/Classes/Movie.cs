using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace s1.Classes
{
    public class Movie
    {
        public Movie()
        {
            Favorite = false;
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    title = value;
                }
                else
                {
                    throw new ArgumentException("Title must not be empty.");
                }
            }
        }
        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value >= 1900 && value <= DateTime.Now.Year)
                {
                    year = value;
                }
                else
                {
                    throw new ArgumentException("Invalid year.");
                }
            }
        }
        private string[] genres;
        public string[] Genres
        {
            get { return genres; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    genres = value;
                }
                else
                {
                    throw new ArgumentException("Genres must not be empty.");
                }
            }
        }
        private string[] actors;
        public string[] Actors
        {
            get { return actors; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    actors = value;
                }
                else
                {
                    throw new ArgumentException("Actors must not be empty.");
                }
            }
        }
        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (!File.Exists(System.IO.Path.GetFullPath(@"..\..\") + value))
                {
                }
                else { imagePath = value; }
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    description = value;
                }
                else
                {
                    throw new ArgumentException("Description must not be empty.");
                }
            }
        }
        public bool Favorite { get; set; }

    }
}
