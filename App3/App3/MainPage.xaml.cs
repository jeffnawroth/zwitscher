using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using App3.Services;
using System.Collections.ObjectModel;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Erstellen Sie eine Liste von Post-Objekten
            List<Post> posts = new List<Post>();
            // Fügen Sie Post-Objekte zur Liste hinzu

            // Erstellen Sie eine Instanz der PostList und übergeben Sie die Liste von Post-Objekten als Argument
            PostList postList = new PostList(posts);

            // Rufen Sie die Display-Methode auf, um die Postings anzuzeigen
            postList.Display();

        }


    }
}
