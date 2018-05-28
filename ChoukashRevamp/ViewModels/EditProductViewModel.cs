using Caliburn.Micro;
using ChoukashRevamp.Models;
using ChoukashRevamp.ViewModels.Navigator;
using ChoukashRevamp.Views.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;


namespace ChoukashRevamp.ViewModels
{
    public sealed class EditProductViewModel:Conductor<object>.Collection.OneActive
    {
        #region Declaration
        private ObservableCollection<Company> _companycollection;
        private ObservableCollection<User> _usercollection;
        private ObservableCollection<Role> _userroles;
        private bool _allowcreationofuser { get; set; }
        private SuperAdmin SA { get; set; }
        private User CurrentUser { get; set; }
        private CreateCompanyViewModel CreateCompanyPage { get; set; }
        private CreateCompanyViewModel EditCompanyPage { get; set; }
        private CreateRoleViewModel CreateRolePage { get; set; }
        private CreateRoleViewModel EditRolePage { get; set; }
        private NavigatePage NavigationTool { get; set; }
        private bool EditUserMode { get; set; }
        private Company UserCompany { get; set; }

        private User _selecteduser;

        private string _username;
        private string _errorusername;
        private string _useremail;
        private string _erroruseremail;
        private string _password;
        private string _errorpassword;
        private string _confirmpassword;
        private string _errorconfirmpassword;
        private string _erroruserrole;

        private uint _borderthickness;
        private bool _showdetail;
        private Role _currentuserrole;
        private string _userformtitle;
        private bool _cduser;

        public bool CDUser
        {
            get { return _cduser; }
            set {
                _cduser = value;
                NotifyOfPropertyChange(() => CDUser);
            }
        }
        

        public IEventAggregator EventAggregator { get; set; }

        public string ErrorUserRole
        {
            get { return _erroruserrole; }
            set
            {
                _erroruserrole = value;
                NotifyOfPropertyChange(() => ErrorUserRole);
            }
        }
        


        public User SelectedUser
        {
            get { return _selecteduser; }
            set
            {
                _selecteduser = value;
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }


        public bool AllowCreationOfUser
        {
            get { return _allowcreationofuser; }
            set { 
                _allowcreationofuser = value;
                NotifyOfPropertyChange(() => AllowCreationOfUser);
            }
        }

        public string UserFormTitle
        {
            get { return _userformtitle; }
            set
            {
                _userformtitle = value;
                NotifyOfPropertyChange(() => value);
            }
        }

        public Role CurrentUserRole
        {
            get { return _currentuserrole; }
            set
            {
                _currentuserrole = value;
                NotifyOfPropertyChange(() => CurrentUserRole);
            }
        }


        public ObservableCollection<Role> UserRoles
        {
            get { return _userroles; }
            set
            {
                _userroles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        public bool ShowDetail
        {
            get { return _showdetail; }
            set
            {
                _showdetail = value;
                NotifyOfPropertyChange(() => ShowDetail);
            }
        }


        public uint BorderThickness
        {
            get { return _borderthickness; }
            set
            {
                _borderthickness = value;
                NotifyOfPropertyChange(() => BorderThickness);
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


        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set
            {
                _confirmpassword = value;
                NotifyOfPropertyChange(() => ConfirmPassword);
            }
        }


        public string ErrorPassword
        {
            get { return _errorpassword; }
            set
            {
                _errorpassword = value;
                NotifyOfPropertyChange(() => ErrorPassword);
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


        public string ErrorUserEmail
        {
            get { return _erroruseremail; }
            set
            {
                _erroruseremail = value;
                NotifyOfPropertyChange(() => ErrorUserEmail);
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


        public string ErrorUserName
        {
            get { return _errorusername; }
            set
            {
                _errorusername = value;
                NotifyOfPropertyChange(() => ErrorUserName);
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


        public ObservableCollection<User> UserCollection
        {
            get { return _usercollection; }
            set
            {
                _usercollection = value;
                NotifyOfPropertyChange(() => UserCollection);
            }
        }


        public ObservableCollection<Company> CompanyCollection
        {
            get { return _companycollection; }
            set
            {
                _companycollection = value;
                NotifyOfPropertyChange(() => CompanyCollection);
            }
        } 
        #endregion
        public EditProductViewModel(SuperAdmin sa, IEventAggregator ea)
        {
            this.SA = sa;
            
            
            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);

            this.CompanyCollection = new ObservableCollection<Company>();
            this.BorderThickness = 0;
            this.ShowDetail = false;

            this.GetAllCompanies();
        }

        #region Companies
        private void GetAllCompanies()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities1())
            {
                var companies = ctx.Companies.Include(a => a.Roles).Include(a => a.Users);
                foreach (var company in companies)
                {
                    this.CompanyCollection.Add(company);
                }
            }
        }

        public void LoadAllUsersFromCompany(Company usercompany)
        {
            if (usercompany != null)
            {
                ShowDetail = false;
                this.UserCompany = usercompany;
                ReloadAllUserInfo();
                CDUser = true;
                using (var ctx = new Choukash_Revamp_DemoEntities1())
                {
                    if (this.UserCollection != null)
                    {
                        this.UserCollection.Clear();
                    }
                    var users = ctx.Users.Include(a => a.Role).Include(a => a.Company).Where(a => a.companies_id == usercompany.id).ToList();

                    //foreach (var user in users)
                    //{
                    //    user.Role = ctx.Roles.Where(a => a.id == user.roles_id).SingleOrDefault<Role>();
                    //}

                    this.UserCollection = new ObservableCollection<User>(users);
                    this.UserRoles = new ObservableCollection<Role>(usercompany.Roles);

                }
            }
            else
                this.UserCollection = null;

        }

        public void AddCompany()
        {
            if (NavigationTool == null || NavigationTool.Params[0] != CreateCompanyPage)
            {
                CreateCompanyPage = new CreateCompanyViewModel(this.SA, EventAggregator, "Create Company");
                NavigationTool = new NavigatePage(CreateCompanyPage);
                EventAggregator.PublishOnUIThread(NavigationTool);
            }
            else
                EventAggregator.PublishOnUIThread(NavigationTool);
        }

        public void EditCompany(Company currentCompany)
        {
            if (currentCompany != null)
            {
                if (NavigationTool == null || NavigationTool.Params[0] != EditCompanyPage)
                {
                    EditCompanyPage = new CreateCompanyViewModel(this.SA, currentCompany, EventAggregator, "Edit Company");
                    NavigationTool = new NavigatePage(EditCompanyPage);
                    EventAggregator.PublishOnUIThread(NavigationTool);
                }
                else
                    EventAggregator.PublishOnUIThread(NavigationTool);
            }
        }

        public void DeleteCompany(Company currentCompany)
        {
            if (currentCompany != null)
            {

                using (var ctx = new Choukash_Revamp_DemoEntities1())
                {
                    var company = ctx.Companies.Where(a => a.id == currentCompany.id).SingleOrDefault<Company>();
                    if (company != null)
                    {
                        var company_users = ctx.Users.Where(a => a.companies_id == company.id).ToList<User>();
                        var company_roles = ctx.Roles.Where(a => a.companies_id == company.id).ToList<Role>();


                        if (company_users.Count > 0 || company_roles.Count > 0)
                        {
                            ctx.Users.RemoveRange(company_users);
                            //ctx.SaveChanges();

                            foreach (var role in company_roles)
                            {
                                var role_permissions = ctx.Role_Permission.Where(a => a.roles_id == role.id).ToList<Role_Permission>();
                                ctx.Role_Permission.RemoveRange(role_permissions);
                            }

                            ctx.Roles.RemoveRange(company_roles);
                            ctx.SaveChanges();
                        }
                        ctx.Companies.Remove(company);
                        ctx.SaveChanges();
                        this.CompanyCollection.Clear();
                        GetAllCompanies();

                    }
                }
            }
        } 
        #endregion

        public void AddUser() 
        {
            ReloadAllUserInfo(); 
            ShowDetail = true;
            BorderThickness = 1;
                
            EditUserMode = false;
            SelectedUser = null;
            CurrentUserRole = null;
            
            if (!AllowCreationOfUser)
                AllowCreationOfUser = true;

        }
        public void DeleteUser(User user) 
        {
            using (var ctx = new Choukash_Revamp_DemoEntities1()) 
            {
                if (user != null) 
                {
                    var _user = ctx.Users.Include(a => a.Role).Include(a => a.Company).Where(a => a.id == user.id).FirstOrDefault<User>();
                    var _userCompany = ctx.Companies.Include(a => a.Roles).Where(a => a.id == _user.companies_id).FirstOrDefault<Company>();
                    ctx.Users.Remove(_user);
                    ctx.SaveChanges();
                    LoadAllUsersFromCompany(_userCompany);
                }
            }
        }
        private void EditUser()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities1())
            {
                var user = ctx.Users.Include(a => a.Role).Include(a => a.Company).Where(a => a.id == CurrentUser.id).SingleOrDefault<User>();
                var _user = this.UserCollection.Where(a => a.id == CurrentUser.id).SingleOrDefault<User>();
                if (user.name != UserName) 
                {
                    user.name = UserName;
                    _user.name = UserName;
                }
                else if (user.email != UserEmail) 
                {
                    user.email = UserEmail;
                    _user.email = UserEmail;
                }
                else if (user.password != Password) 
                {
                    user.password = Password;
                    _user.password = Password;
                }
                else if (user.roles_id != CurrentUserRole.id) 
                {
                    user.Role = CurrentUserRole;
                    _user.Role = CurrentUserRole;
                }
 
                ctx.SaveChanges();
                ReloadUserCollection(user.Company);
                
            }
        }

        private void ReloadUserCollection(Company company)
        {
            this.UserCollection.Clear();
            this.UserCollection = new ObservableCollection<User>(company.Users);
        }


        #region Validation
        public void Validation(string name)
        {
            switch (name)
            {
                case "UserName":
                    if (String.IsNullOrWhiteSpace(UserName))
                    {
                        ErrorUserName = "Please enter username";
                    }
                    else
                    {
                        ErrorUserName = "";
                        if (EditUserMode)
                        {
                            EditUser();
                        }
                    }

                    break;
                case "UserEmail":
                    if (String.IsNullOrWhiteSpace(UserEmail))
                    {
                        ErrorUserEmail = "Please enter email id";
                    }
                    else
                    {
                        if (!ValidationClass.ValidateEmailID(UserEmail))
                        {
                            ErrorUserEmail = "Email id is not valid";
                        }
                        else
                        {
                            if (!ValidationClass.UniqueTestforEmail(UserEmail,"User") && !EditUserMode)
                            {
                                ErrorUserEmail = "This Email ID is already associated with another user";
                            }
                            else if (EditUserMode && CurrentUser != null)
                            {
                                if (!ValidationClass.UniqueTestforEmail(CurrentUser.id,UserEmail,"User"))
                                {
                                    ErrorUserEmail = "This Email ID is already associated with another user"; 
                                }
                            }
                            else
                            {
                                ErrorUserEmail = "";
                                if (EditUserMode)
                                {
                                    EditUser();
                                }
                            }


                        }
                    }
                    break;
                case "Password":
                    if (String.IsNullOrWhiteSpace(Password))
                    {
                        ErrorPassword = "Please enter password";
                    }
                    else
                    {
                        if (!ValidationClass.ValidatePassword(Password))
                        {
                            ErrorPassword = "Password must be within 4-20 characters";
                        }
                        else
                        {
                            ErrorPassword = "";

                        }

                    }
                    break;
                case "ConfirmPassword":
                    if (String.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        ErrorConfirmPassword = "Please retype your password";
                    }
                    else
                    {
                        if (Password != ConfirmPassword)
                        {
                            ErrorConfirmPassword = "Password mismatched";
                        }
                        else
                        {
                            ErrorConfirmPassword = "";
                            if (EditUserMode)
                            {
                                EditUser();
                            }
                        }

                    }
                    break;
                case "Roles":
                    if (CurrentUserRole == null)
                    {
                        ErrorUserRole = "Please select arole for user.";
                    }
                    else 
                    {
                        ErrorUserRole = "";
                    }
                    break;
            }

        } 
        #endregion


        public void ToggleEditDetail() 
        {
            if (BorderThickness == 0)
            {
                BorderThickness = 1;
            }
           
            
        }
        private void ReloadAllUserInfo()
        {
            UserName = "";
            UserEmail = "";
            Password = "";
            ConfirmPassword = "";
            ErrorUserName = "";
            ErrorUserEmail = "";
            ErrorPassword = "";
            ErrorConfirmPassword = "";
            CurrentUserRole = null;

        }

        public void ReloadConfirmPassword() { ConfirmPassword = null; }

        public void LoadUserDetails(User user) 
        {
            if (user != null && SelectedUser != null)
            {
                ReloadAllUserInfo();
                CurrentUser = user;
                ShowDetail = true;
                EditUserMode = true;
                AllowCreationOfUser = false;
                BorderThickness = 0;
                
                
                using (var ctx = new Choukash_Revamp_DemoEntities1())
                {
                    if (this.UserRoles != null)
                    {
                        this.UserRoles.Clear();
                    }
                    var compnay_roles = ctx.Companies.Include(a => a.Roles).Where(a => a.id == user.companies_id).SingleOrDefault<Company>();


                    this.UserRoles = new ObservableCollection<Role>(compnay_roles.Roles);

                    UserName = user.name;
                    UserEmail = user.email;
                    Password = user.password;
                    ConfirmPassword = user.password;
                    CurrentUserRole = compnay_roles.Roles.Where(a => a.id == user.roles_id).SingleOrDefault<Role>();
                }    
            }
        }

        public void CreateUser() 
        {
            if (String.IsNullOrWhiteSpace(ErrorUserName) && String.IsNullOrWhiteSpace(ErrorUserEmail) 
                && String.IsNullOrWhiteSpace(ErrorPassword) && String.IsNullOrWhiteSpace(ErrorConfirmPassword) && String.IsNullOrWhiteSpace(ErrorUserRole)) 
            {
                using (var ctx = new Choukash_Revamp_DemoEntities1()) 
                {
                    ctx.Users.Add(new User()
                    {
                        name = UserName,
                        email = UserEmail,
                        password = Password,
                        companies_id = UserCompany.id,
                        roles_id = CurrentUserRole.id
                    });

                    ctx.SaveChanges();
                    LoadAllUsersFromCompany(UserCompany);
                    ReloadAllUserInfo();
                }
            }
            else if (CurrentUserRole == null) 
            {
                ErrorUserRole = "Please select a role";
            }
        }


        public void AddRole(Company company) 
        {
            if (NavigationTool == null || NavigationTool.Params[0] != CreateRolePage && company != null) 
            {
                this.CreateRolePage = new CreateRoleViewModel("Create Role", company, EventAggregator);
                this.NavigationTool = new NavigatePage(CreateRolePage);
                EventAggregator.PublishOnUIThread(NavigationTool);
            }
            else
                EventAggregator.PublishOnUIThread(NavigationTool);

        }

        public void EditRole(Role role) 
        {
            if (NavigationTool == null || NavigationTool.Params[0] != EditRolePage && role != null)
            {
                this.EditRolePage = new CreateRoleViewModel("Edit Role", UserCompany, EventAggregator);
                this.NavigationTool = new NavigatePage(EditRolePage);
                EventAggregator.PublishOnUIThread(NavigationTool);
            }
            else
                EventAggregator.PublishOnUIThread(NavigationTool);
            
        }
    }
}
