using System;
using System.Collections.Generic;
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


namespace UI
{
    /// <summary>
    /// Interaction logic for TabGrid.xaml
    /// </summary>
    public partial class AssetGrid : UserControl
    {
        public event EventHandler AddEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SaveEvent;
        public AssetGrid()
        {
            InitializeComponent();
        }



        private void AddAssetButton_Click(object sender, RoutedEventArgs e)
        {
            AddEvent?.Invoke(this, new EventArgs());
        }

        private void SaveAssetButton_Click(object sender, RoutedEventArgs e)
        {
            
            SaveEvent?.Invoke(this, new EventArgs());
        }
    }

  
}
