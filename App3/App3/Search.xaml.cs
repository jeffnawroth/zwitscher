using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class Search : ContentPage
    {
        private List<string> searchData; // Daten für die Suche
        private List<string> searchResults; // Gefundene Suchergebnisse

        public Search()
        {
            InitializeComponent();

            // Beispielhafte Daten für die Suche
            searchData = new List<string>
            {
                "Apfel",
                "Banane",
                "Orange",
                "Erdbeere",
                "Ananas",
                "Kiwi"
            };
        }

        private void ResultsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
            {
            string searchText = searchEntry.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchResults = searchData
                    .Where(data => data.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                resultsListView.ItemsSource = searchResults;
            }
            else
            {
                resultsListView.ItemsSource = null;
            }
        }
    }
}