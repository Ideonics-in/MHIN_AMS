using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using DataAccess;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Login login;
        AssetGrid AssetGrid;
        UserGrid UserGrid;
        AddAsset AddAssetControl;
        User User;

        ObservableCollection<Asset> Assets;
        



        public MainWindow()
        {
            InitializeComponent();

            login = new Login();
            login.LoginEvent += Login_LoginEvent;
            Keyboard.Focus(login.UsernameTextBlock);
            MainGrid.Children.Add(login);
        }

        private void Login_LoginEvent(object sender, LoginArgs e)
        {
            using (var db = new AMSDB())
            {
                var users = from u in db.Users
                            where u.Username.Equals(e.Username) && u.Password.Equals(e.Password)
                           select u;
                User = users.SingleOrDefault();

                if (User == null)
                {
                    MessageBox.Show("Invalid Username / Password", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                IEnumerable<Asset> assets = null;

                if (User.Department == "Admin")
                {
                    assets = from a in db.Assets
                             select a;
                }
                else
                {
                    assets = (from a in db.Assets
                             where a.Department == User.Department
                             select a).OrderBy(n => n.Name);
                }
                 

                Assets = new ObservableCollection<Asset>();

                foreach(Asset a in assets)
                {
                    a.Update();
                    Assets.Add(a);
                }

                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() =>
                            {
                                AssetGrid = new AssetGrid();
                                AssetGrid.AssetDataGrid.DataContext = Assets;
                                AssetGrid.AddEvent += AssetGrid_AddEvent;
                                AssetGrid.DeleteEvent += AssetGrid_DeleteEvent;
                                AssetGrid.SaveEvent += AssetGrid_SaveEvent;
                                MainGrid.Children.Clear();
                                MainGrid.Children.Add(AssetGrid);
                            }));
                
            }
        }

      
        #region AssetEventHandlers
        private void AssetGrid_AddEvent(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() =>
                          {
                              AddAssetControl = new AddAsset();
                              AddAssetControl.SaveEvent += AddAssetControl_SaveEvent;
                              AddAssetControl.CancelEvent += AddAssetControl_CancelEvent;
                              MainGrid.Children.Clear();
                              MainGrid.Children.Add(AddAssetControl);
                          }));
        }

        private void AssetGrid_SaveEvent(object sender, EventArgs e)
        {
            using (var db = new AMSDB())
            {
                var dbassets = (from a in db.Assets
                          where a.Department == User.Department
                          select a).OrderBy(n => n.Name);

                foreach (Asset a in dbassets)
                {
                    foreach(Asset b in Assets)
                    {
                        
                        if (a.ID == b.ID)
                        {
                            b.Update();
                            a.AttendedOn = b.AttendedOn;
                            break;
                        }
                           
                    }
                    
                    
                }
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() =>
                          {
                              AssetGrid.AssetDataGrid.DataContext = null;
                              AssetGrid.AssetDataGrid.DataContext = Assets;

                              MessageBox.Show("Assets Updated", "Update Info", MessageBoxButton.OK, MessageBoxImage.Information);
                          }));
                db.SaveChanges();

               
            }
            
        }

        private void AssetGrid_DeleteEvent(object sender, EventArgs e)
        {
            
        }



        #endregion


        #region AddAssetEventHandlers
        private void AddAssetControl_CancelEvent(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() =>
                          {
                             
                              MainGrid.Children.Clear();
                              MainGrid.Children.Add(AssetGrid);
                          }));

        }

        private void AddAssetControl_SaveEvent(object sender, Asset e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() =>
                          {
                              if(User != null)
                                  e.Department = User.Department;

                              using (var db = new AMSDB())
                              {
                                  if (db.Assets.Add(e) == null)
                                  {
                                      MessageBox.Show("Error Saving Asset. Please Retry", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  }
                                  else
                                  {
                                      db.SaveChanges();
                                      MessageBox.Show("Asset Saved", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                      Assets.Add(e);
                                  }


                              }
                          }));
        }
        #endregion
    }
}
