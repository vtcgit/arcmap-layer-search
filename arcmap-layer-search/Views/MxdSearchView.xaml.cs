using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using arcmap_layer_search.Models;

namespace arcmap_layer_search.Views
{
    /// <summary>
    /// Interaction logic for MxdSearchView.xaml
    /// </summary>
    public partial class MxdSearchView : UserControl
    {
        public MxdSearchView()
        {
            InitializeComponent();
        }

        public bool FilterLayerItems(object o)
        {
            var item = o as LayerEntry;
            if (item == null) return false;

            string searchText = FilterText.Text;
            if (searchText.Trim().Length == 0) return true;

            if (item.LayerName.ToLower().Contains(searchText.ToLower()) || item.Location.ToLower().Contains(searchText.ToLower()))
                return true;
            return false;
        }

        private void FilterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(Known_layers.ItemsSource);
            if (view != null)
            {
                view.Filter = null;
                view.Filter = new Predicate<object>(FilterLayerItems);
            }

        }
    }
}
