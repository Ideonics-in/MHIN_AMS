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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public event EventHandler<LoginArgs> LoginEvent;
        public Login()
        {
            InitializeComponent();
            UsernameTextBlock.Focus();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.LoginEvent?.Invoke(this,new LoginArgs { Username = UsernameTextBlock.Text, Password = tbPassword.Password });
        }
    }

    public class LoginArgs
    {
        public String Username { get; set; }
        public String Password { get; set; }
    }
}
