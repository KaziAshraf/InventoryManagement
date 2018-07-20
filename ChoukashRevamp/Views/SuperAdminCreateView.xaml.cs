using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ChoukashRevamp.Views.Validation;
using ChoukashRevamp.Models;

namespace ChoukashRevamp.Views
{
    /// <summary>
    /// Interaction logic for SuperAdmin.xaml
    /// </summary>
    public partial class SuperAdminCreateView : UserControl
    {
        public SuperAdminCreateView()
        {
            InitializeComponent();

            this.Name.PreviewTextInput += ValidationClass.ValidateTextWithSpace;
            this.Loaded += View_Load;
            
        }

        private void View_Load(object sender, RoutedEventArgs e)
        {
            this.Name.Focus();
            this.ConfirmPassword.IsEnabled = false;
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.Email.Text))
            {
                this.ErrorEmail.Text = "Please Enter an Email Id.";
            }
            else {
                if (!ValidationClass.ValidateEmailID(this.Email.Text))
                    this.ErrorEmail.Text = "This is not a valid Email Id.";
                else
                {
                    using (var ctx = new Choukash_Revamp_DemoEntities()) {
                        var email = ctx.SuperAdmins.Where(a => a.email == this.Email.Text).FirstOrDefault<SuperAdmin>();
                        if (email == null) {
                            this.ErrorEmail.Text = "";
                        }
                        else
                            this.ErrorEmail.Text = "This email is alraedy associated with another user";
                    }
                }
            }
            
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Name.Text))
            {
                this.ErrorName.Text = "Please Enter your Name";
            }
            else
                this.ErrorName.Text = "";
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Password.Password))
                ErrorPassword.Text = "Please enter password.";
            else {
                if (!ValidationClass.ValidatePassword(Password.Password)) {
                    ErrorPassword.Text = "Password must be within 8-20 characters";
                }
                else
                    ErrorPassword.Text = "";
            }
        }
        
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.ConfirmPassword.IsEnabled = true;
        }
        
        private void ConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ConfirmPassword.Password))
                ErrorConfirmPassword.Text = "Please retype your password.";
            else if (Password.Password != ConfirmPassword.Password)
            {
                ErrorConfirmPassword.Text = "Password does not match original password.";
            }
            else
                ErrorConfirmPassword.Text = "";
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password != ConfirmPassword.Password) {
                ErrorConfirmPassword.Text = "Password does not match original password.";
            }
            else
                ErrorConfirmPassword.Text = "";
        }

       
      
    }
}
