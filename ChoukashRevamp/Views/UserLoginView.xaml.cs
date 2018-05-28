using ChoukashRevamp.Models;
using ChoukashRevamp.Views.Validation;
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

namespace ChoukashRevamp.Views
{
    /// <summary>
    /// Interaction logic for UserLoginView.xaml
    /// </summary>
    public partial class UserLoginView : UserControl
    {
        public UserLoginView()
        {
            InitializeComponent();
            this.Loaded += View_Load;
        }
        private void View_Load(object sender, RoutedEventArgs e)
        {
            this.UserName.Focus();

        }



        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UserName.Text))
            {
                this.ErrorUserName.Text = "Please Enter your Name";
            }
            else
                this.ErrorUserName.Text = "";
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Password.Password))
                ErrorPassword.Text = "Please enter password.";
            else
                ErrorPassword.Text = "";

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

        }


    }
       

}
