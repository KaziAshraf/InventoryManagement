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
    /// Interaction logic for CreateCompanyView.xaml
    /// </summary>
    public partial class CreateCompanyView : UserControl
    {
        public CreateCompanyView()
        {
            InitializeComponent();
            this.Loaded += CreateCompanyView_Loaded;
        }

        private void CreateCompanyView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Name.Focus();
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.Name.Text))
            {
                this.ErrorName.Text = "";
            }
            else
                this.ErrorName.Text = "Please enter name of comapny";
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.Email.Text))
            {
                if (!ValidationClass.ValidateEmailID(this.Email.Text))
                {
                    ErrorEmail.Text = "Email Id is not valid";
                }
                else
                {
                    using (var ctx = new Choukash_Revamp_DemoEntities1())
                    {
                        var email = ctx.Companies.Where(a => a.email == this.Email.Text).FirstOrDefault<Company>();
                        if (email == null)
                        {
                            this.ErrorEmail.Text = "";
                        }
                        else
                            this.ErrorEmail.Text = "This email is alraedy associated with another user";
                    }
                }
                    
            }
            else
                this.ErrorEmail.Text = "Please enter email of company";
        }

        private void Location_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.Location.Text))
            {
                this.ErrorLocation.Text = "";
            }
            else
                this.ErrorLocation.Text = "Please enter location of company";
        }

        private void Description_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.Description.Text))
            {
                this.ErrorDescription.Text = "";
            }
            else
                this.ErrorDescription.Text = "Please enter description of company";
        }

        
    }
}
