using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChoukashRevamp.ViewModels.Navigator;

namespace ChoukashRevamp.ViewModels
{
    public sealed class SuperAdminCreateViewModel:Conductor<object>
    {
        private string _name;
        private string _email;
        private string _password;
        private string _confirmpassword;
        private string _errorname;
        private string _erroremail;
        private string _errorpassword;
        private string _errorconfirmpassword;


        public string ErrorConfirmPassword
        {
            get { return _errorconfirmpassword; }
            set { 
                _errorconfirmpassword = value;
                NotifyOfPropertyChange(() => ErrorConfirmPassword);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        

        public string ErrorPassword
        {
            get { return _errorpassword; }
            set { 
                _errorpassword = value;
                NotifyOfPropertyChange(() => ErrorPassword);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        

        public string ErrorEmail
        {
            get { return _erroremail; }
            set { 
                _erroremail = value;
                NotifyOfPropertyChange(() => ErrorEmail);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        

        public string ErrorName
        {
            get { return _errorname; }
            set { 
                _errorname = value;
                NotifyOfPropertyChange(() => ErrorName);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        

        public SuperAdminCreateViewModel() {
            this.Name = null;
            this.Email = null;
            this.Password = null;
            this.ConfirmPassword = null;
        }

        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set { 
                _confirmpassword = value;
                NotifyOfPropertyChange(() => ConfirmPassword);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        

        public string Password
        {
            get { return _password; }
            set { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        
        public string Email
        {
            get { return _email; }
            set { 
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }
        
        public string Name
        {
            get { return _name; }
            set {
                _name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanCreateSuperAdmin);
            }
        }

        public bool CanCreateSuperAdmin {
            get {
                if (!String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(ConfirmPassword) && !String.IsNullOrWhiteSpace(Name) &&
                !String.IsNullOrWhiteSpace(Email) && Password == ConfirmPassword && String.IsNullOrWhiteSpace(ErrorEmail) 
                && String.IsNullOrWhiteSpace(ErrorPassword) && String.IsNullOrWhiteSpace(ErrorConfirmPassword))
                {
                    return true;

                }
                else
                    return false;
            } 
        }
        
        public void CreateSuperAdmin()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities1())
            {
                var sa = new SuperAdmin()
                {
                    name = Name,
                    email = Email,
                    password = Password,
                    is_active = true
                };
                ctx.SuperAdmins.Add(sa);
                ctx.SaveChanges();
                
                MessageBox.Show("Admin Created Successfully");
                

            }

            DeactivateItem(this, true);
            base.TryClose();
            
            Navigator.Navigator.Navigate<ShellViewModel>();
            Application.Current.Windows[0].Close();
            
        }
        
    }
}
