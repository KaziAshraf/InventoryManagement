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
using System.Data.Entity;
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
        public Role UserRole { get; set; }

        private NavigatePage NextPage { get; set; }
        private CreateUserViewModel UserPage { get; set; }
        private EditProductViewModel EditPage { get; set; }

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

        public CreateRoleViewModel(string title, Role role, Company company, IEventAggregator ea)
        {
            this.Title = title;
            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);
            
            this.UserCompany = company;

            this.UserRole = role ?? null;

            this.AllPermissions = new ObservableCollection<Permission>();
            this.ListPermissions = new ObservableCollection<CheckBoxListViewItem>();

            this.GetAllPermissions();

            this.ShowCurrentRole();
        }

        private void ShowCurrentRole()
        {
            if (UserRole != null) 
            {
                RoleName = UserRole.name;
                RoleDescription = UserRole.description;
                ConfigurePermissionAccordingtoRole();
            }
        }

        private void ConfigurePermissionAccordingtoRole()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities1()) 
            {
                var role_permissions = ctx.Role_Permission.Include(a => a.Role).Include(a => a.Permission).Where(a => a.roles_id == UserRole.id).ToList<Role_Permission>();
                foreach (var role_permission in role_permissions) 
                {
                    var permission = role_permission.Permission;
                    foreach (var item in ListPermissions) 
                    {
                        if (permission.menu == item.Text)
                        {
                            item.IsChecked = true;
                            break;
                        }
                    }
                }
            }
        }

        private void GetAllPermissions()
        {
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
                case "Create Role":
                    CreateRoleforCompany();
                    break;
                case "Edit Role":
                    EditRole();
                    break;
                case "Add Role":
                    CreateRoleforCompany();
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
                        if (this.NextPage == null && this.Title == "Create Role")
                        {
                            UserPage = new CreateUserViewModel("Create Admin", this.UserCompany, user_role, permissions, this.EventAggregator);
                            this.NextPage = new NavigatePage(UserPage);
                            this.EventAggregator.PublishOnUIThread(NextPage);
                        }
                        else if (this.NextPage == null && this.Title == "Add Role") 
                        {
                            ctx.Roles.Add(user_role);
                            CastPermissiontoRolePermission(user_role, permissions,ctx);
                            ctx.SaveChanges();
                            
                            var sa = ctx.SuperAdmins.Where(a => a.id == UserCompany.superadmin_id).SingleOrDefault<SuperAdmin>();
                            this.EventAggregator.PublishOnUIThread("Operation complete");
                            EditPage = new EditProductViewModel(sa, EventAggregator);
                            this.NextPage = new NavigatePage(EditPage);
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

        private void CastPermissiontoRolePermission(Role user_role, IList<Permission> permissions, Choukash_Revamp_DemoEntities1 ctx)
        {
            foreach (var permission in permissions)
            {
                ctx.Role_Permission.Add(new Role_Permission() 
                {
                    roles_id = user_role.id,
                    permissions_id = permission.id
                });
            }
        }

       
        

       

    }
}
