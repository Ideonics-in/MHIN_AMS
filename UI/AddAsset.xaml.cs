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
using DataAccess;
namespace UI
{
    /// <summary>
    /// Interaction logic for AddAsset.xaml
    /// </summary>
    public partial class AddAsset : UserControl
    {
        public event EventHandler<Asset> SaveEvent;
        public event EventHandler CancelEvent;
        public AddAsset()
        {
            InitializeComponent();
        }

        private void AssetSaveButton_Click(object sender, RoutedEventArgs e)
        {
            int MInterval = 0, AInterval = 0;
            
            int.TryParse(MIntervalTextBox.Text, out MInterval);
            int.TryParse(AlertTextBox.Text, out AInterval);
            SaveEvent?.Invoke(this,
                new Asset
                {
                    Name = NameTextBox.Text,
                    Tag = TagTextBox.Text,
                    Vendor = VendorTextBox.Text,
                    AttentionInterval = MInterval,
                    AlertInterval = AInterval,
                    AttendedOn = OnDatePicker.SelectedDate.Value,
                    Location = LocationTextBox.Text,
                    AttentionDue = OnDatePicker.SelectedDate.Value.AddDays(MInterval)
                  
                    
                });
                    

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelEvent?.Invoke(this, new EventArgs());
        }
    }
}
