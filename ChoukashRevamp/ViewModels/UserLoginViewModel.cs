using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;

namespace ChoukashRevamp.ViewModels
{
    
    public sealed class UserLoginViewModel:Conductor<object>
    {
        private string _username;
        private string _password;
        private string _errorpassword;
        private string _errorusername;

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);
                
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                
            }
        }
        public string ErrorUserName
        {
            get { return _errorusername; }
            set { 
                _errorusername = value;
                NotifyOfPropertyChange(() => ErrorUserName);
                
            }
        }
        

        public string ErrorPassword
        {
            get { return _errorpassword; }
            set { 
                _errorpassword = value;
                NotifyOfPropertyChange(() => ErrorPassword);
                
            }
        }
        


        public UserLoginViewModel() {
            this.UserName = null;           
            this.Password = null;
          
        }
        
        public void Login()
        {
            if (!String.IsNullOrWhiteSpace(UserName) && !String.IsNullOrWhiteSpace(Password))
            {

                using (var ctx = new Choukash_Revamp_DemoEntities1())
                {
                    var sa = ctx.SuperAdmins.Where(a => (a.name == UserName) && (a.password == Password)).FirstOrDefault<SuperAdmin>();
                    var user = ctx.Users.Include(a => a.Company).Include(a => a.Role).Where(a => (a.name == UserName) && (a.password == Password)).FirstOrDefault<User>();

                    if (sa == null && user == null)
                    {
                        ErrorUserName = "No such user exists, please enter your username correctly.";
                    }
                    else if (sa == null && user != null)
                    {
                        DeactivateItem(this, true);
                        base.TryClose();
                        Navigator.Navigator.Navigate<ShellViewModel>(user);

                        Application.Current.Windows[0].Close();
                    }
                    else if (sa != null && user == null)
                    {
                        DeactivateItem(this, true);
                        base.TryClose();
                        Navigator.Navigator.Navigate<ShellViewModel>(sa);

                        Application.Current.Windows[0].Close();
                    }

                }
            }
            else if(String.IsNullOrWhiteSpace(UserName)){
                ErrorUserName = "Please enter username";
            }
            else
            {
                ErrorPassword = "Please enter password";
            }
            
          
            
        }
        
    }

}

