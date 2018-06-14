using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoukashRevamp.Views.Validation;
using System.Windows;

namespace ChoukashRevamp.ViewModels
{
    public sealed class CreateUserViewModel:Screen
    {
        #region Declaration
        private Role UserRole { get; set; }
        private Company Company { get; set; }
        private IList<Permission> UserPermissions { get; set; }

        public IEventAggregator EventAggregator { get; private set; }

        private string _title;
        private string _username;
        private string _useremail;
        private string _password;
        private string _confirmpassword;
        private string _errorusername;
        private string _erroruseremail;
        private string _errorpasssword;
        private string _errorconfirmpassword;
        private string _erroruserrole;

        public string ErrorUserRole
        {
            get { return _erroruserrole; }
            set
            {
                _erroruserrole = value;
                NotifyOfPropertyChange(() => ErrorConfirmPassword);
            }
        }


        public string ErrorConfirmPassword
        {
            get { return _errorconfirmpassword; }
            set
            {
                _errorconfirmpassword = value;
                NotifyOfPropertyChange(() => ErrorConfirmPassword);
            }
        }


        public string ErrorPassword
        {
            get { return _errorpasssword; }
            set
            {
                _errorpasssword = value;
                NotifyOfPropertyChange(() => ErrorPassword);
            }
        }


        public string ErrorUserEmail
        {
            get { return _erroruseremail; }
            set
            {
                _erroruseremail = value;
                NotifyOfPropertyChange(() => ErrorUserEmail);
            }
        }


        public string ErrorUserName
        {
            get { return _errorusername; }
            set
            {
                _errorusername = value;
                NotifyOfPropertyChange(() => ErrorUserName);
            }
        }


        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set
            {
                _confirmpassword = value;
                NotifyOfPropertyChange(() => ErrorConfirmPassword);
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



        public string UserEmail
        {
            get { return _useremail; }
            set
            {
                _useremail = value;
                NotifyOfPropertyChange(() => UserEmail);
            }
        }


        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        } 
        #endregion

        public CreateUserViewModel(string title, Company company, Role role, IList<Permission> permissions, IEventAggregator ea)
        {
           
            this.Title = title;
            this.UserRole = role;
            this.UserPermissions = new List<Permission>();
            this.UserPermissions = permissions;
            this.Company = company;

            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);
        }

        public CreateUserViewModel(string title, Company company, Role role, IEventAggregator ea)
        {
            
            this.Title = title;
            this.UserRole = role;
            this.Company = company;

            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);
        }
        public void Validation(string ControlName) 
        {
            switch (ControlName) 
            { 
                case "UserName":
                    if (String.IsNullOrWhiteSpace(UserName))
                    {   ErrorUserName = "Please enter username";
                        break;
                    }
                    else
                        ErrorUserName = "";
                        break;
                case "UserEmail":
                    if (String.IsNullOrWhiteSpace(UserEmail))
                    {
                        ErrorUserEmail = "Please enter email";
                        break;
                    }
                    else {
                       if(!ValidationClass.ValidateEmailID(UserEmail))
                       {
                           ErrorUserEmail = "This email id is not valid";
                           break;
                       } 
                       else
                       {
                           using (var ctx = new Choukash_Revamp_DemoEntities1())
                           {
                               var email = ctx.Users.Where(a => a.email == this.UserEmail).FirstOrDefault<User>();
                               if (email == null)
                               {
                                   this.ErrorUserEmail = "";
                               }
                               else
                                   this.ErrorUserEmail = "This email is alraedy associated with another user";
                           }
                           break;
                       }

                    }
                case "Password":
                    if(String.IsNullOrWhiteSpace(Password))
                    {
                        ErrorPassword = "Please enter password";
                        break;
                    }
                    else
                    {
                        if(!ValidationClass.ValidatePassword(Password))
                        {
                            ErrorPassword = "Password must be within 8-20 characters";
                            break;
                        }
                        else
                        {
                            ErrorPassword ="";
                            break;
                        }
                    }
                case "ConfirmPassword":
                    if (String.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        ErrorConfirmPassword = "Please retype your password";
                        break;
                    }
                    else
                    {
                        if (Password != ConfirmPassword)
                        {
                            ErrorConfirmPassword = "Passwords do not match";
                        }
                        else
                            ErrorConfirmPassword = "";

                        break;
                    }

            }
        }
        public void Proceed() 
        {
            switch (Title)
            {
                case "Create Admin":
                    ConfigureFullProduct();
                    break;
                
            }
        }
        public void ConfigureFullProduct()
        {
            if (String.IsNullOrWhiteSpace(ErrorUserName) && String.IsNullOrWhiteSpace(ErrorUserEmail)
                && String.IsNullOrWhiteSpace(ErrorPassword) && String.IsNullOrWhiteSpace(ErrorConfirmPassword))
            {
                using (var ctx = new Choukash_Revamp_DemoEntities1())
                {
                    ctx.Companies.Add(Company);
                    ctx.Roles.Add(UserRole);

                    foreach (var _permission in UserPermissions)
                    {
                        var rolepermission = new Role_Permission()
                        {
                            roles_id = UserRole.id,
                            permissions_id = _permission.id
                        };
                        ctx.Role_Permission.Add(rolepermission);
                    }
                    var admin = new User()
                    {
                        name = UserName,
                        email = UserEmail,
                        password = Password,
                        companies_id = Company.id,
                        roles_id = UserRole.id
                    };
                    ctx.Users.Add(admin);
                    ctx.SaveChanges();

                    MessageBox.Show("New User Created");

                    this.EventAggregator.PublishOnUIThread("Operation complete");
                }
            }
        }
    }
}
