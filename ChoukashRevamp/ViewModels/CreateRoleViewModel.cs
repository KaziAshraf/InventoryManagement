using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ChoukashRevamp.ViewModels.Navigator;

namespace ChoukashRevamp.ViewModels
{
    public sealed class CreateRoleViewModel:Conductor<object>
    {
        #region Declaration

        private ObservableCollection<Permission> _allpermissions;
        private ObservableCollection<CheckBoxListViewItem> _listpermissions;

        private string _rolename;
        private string _roledescription;
        private string _errorrolename;
        private string _errorrledescription;
        private string _errorselectionpermission;
        private string _title;

        private Company UserCompany { get; set; }

        private NavigatePage NextPage { get; set; }
        private CreateUserViewModel UserPage { get; set; }

        public IEventAggregator EventAggregator { get; private set; }


        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
        


        public string ErrorSelectionPermission
        {
            get { return _errorselectionpermission; }
            set
            {
                _errorselectionpermission = value;
                NotifyOfPropertyChange(() => ErrorSelectionPermission);
            }
        }


        public string ErrorRoleDescription
        {
            get { return _errorrledescription; }
            set
            {
                _errorrledescription = value;
                NotifyOfPropertyChange(() => ErrorRoleDescription);
            }
        }


        public string ErrorRoleName
        {
            get { return _errorrolename; }
            set
            {
                _errorrolename = value;
                NotifyOfPropertyChange(() => ErrorRoleName);
            }
        }


        public string RoleDescription
        {
            get { return _roledescription; }
            set
            {
                _roledescription = value;
                NotifyOfPropertyChange(() => RoleDescription);
            }
        } 
        
        

        public string RoleName
        {
            get { return _rolename; }
            set { 
                _rolename = value;
                NotifyOfPropertyChange(() => RoleName);
            }
        }
        

        public ObservableCollection<CheckBoxListViewItem> ListPermissions
        {
            get { return _listpermissions; }
            set { 
                _listpermissions = value;
                
            }
        }
        
        
        public ObservableCollection<Permission> AllPermissions
        {
            get { return _allpermissions; }
            set { 
                _allpermissions = value;
                NotifyOfPropertyChange(() => AllPermissions);
            }
        }

        #endregion

        public CreateRoleViewModel(string title, Company company, IEventAggregator ea)
        {
            this.Title = title;
            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);
            
            this.UserCompany = company;
           
            this.AllPermissions = new ObservableCollection<Permission>();
            this.ListPermissions = new ObservableCollection<CheckBoxListViewItem>();
            
            using (var ctx = new Choukash_Revamp_DemoEntities1()) 
            {
                foreach (var p in ctx.Permissions) 
                {
                    AllPermissions.Add(p);
                    ListPermissions.Add(new CheckBoxListViewItem(p.menu, false));
                }
            }
        }
        public void Validation(string name) 
        {
            if (name == "RoleName")
            {
                if (String.IsNullOrWhiteSpace(RoleName))
                {
                    ErrorRoleName = "Please Provide Role Name";
                }
                else
                    ErrorRoleName = "";
            }
            else if(name == "RoleDescription") 
            {
                if (String.IsNullOrWhiteSpace(RoleDescription))
                {
                    ErrorRoleDescription = "Please Provide Role Description";
                }
                else
                    ErrorRoleDescription = "";
            }
        }
        public bool CheckAllPermissions() 
        {
            bool granted = false;
            foreach (var permission in ListPermissions) 
            {
                if (permission.IsChecked == true)
                {
                    granted = true;
                    break;
                }       
            }

            return granted;
        }

        public void Proceed() 
        {
            switch (Title) 
            {
                case "Create Company":
                    CreateRoleforCompany();
                    break;
                case "Edit Role":
                    EditRole();
                    break;
            }

        }

        private void EditRole()
        {
            
        }

        private void CreateRoleforCompany()
        {
            if (String.IsNullOrWhiteSpace(ErrorRoleName) && String.IsNullOrWhiteSpace(ErrorRoleDescription))
            {
                if (this.CheckAllPermissions())
                {
                    using (var ctx = new Choukash_Revamp_DemoEntities1())
                    {
                        IList<Permission> permissions = new List<Permission>();
                        Role user_role = new Role()
                        {
                            name = RoleName,
                            description = RoleDescription,
                            companies_id = UserCompany.id
                        };
                        for (int i = 0; i < ListPermissions.Count(); i++)
                        {
                            if (ListPermissions[i].IsChecked)
                            {
                                permissions.Add(AllPermissions[i]);
                            }
                        }
                        if (this.NextPage == null)
                        {
                            UserPage = new CreateUserViewModel("Create Admin", this.UserCompany, user_role, permissions, this.EventAggregator);
                            this.NextPage = new NavigatePage(UserPage);
                            this.EventAggregator.PublishOnUIThread(NextPage);
                        }
                        else
                            this.EventAggregator.PublishOnUIThread(NextPage);
                    }
                }
                else
                    ErrorSelectionPermission = "Please Provide Permission for Role";
            }
        }

    }
}
