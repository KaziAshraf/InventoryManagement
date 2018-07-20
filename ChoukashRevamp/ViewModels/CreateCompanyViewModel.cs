using Caliburn.Micro;
using ChoukashRevamp.Models;
using ChoukashRevamp.ViewModels.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChoukashRevamp.ViewModels
{
    public sealed class CreateCompanyViewModel:Screen
    {
        private string _name;
        private string _email;
        private string _location;
        private string _description;
        
        private string _errorname;
        private string _erroremail;
        private string _errorlocation;
        private string _errordescription;
        private string _actioname;
        
        private SuperAdmin _sa{ get; set; }
        private Company _company { get; set; }

        private NavigatePage NextPage { get; set; }
        private CreateRoleViewModel RolePage { get; set; }

        public IEventAggregator EventAggregator { get; private set; }
        

        public string ActionName
        {
            get { return _actioname; }
            set { _actioname = value; }
        }

        public CreateCompanyViewModel(SuperAdmin sa, Company company, IEventAggregator ea, string actionname)
        {
            this.ActionName = actionname;
            this._company = company;

            this.Name = _company.name;
            this.Email = _company.email;
            this.Location = _company.location;
            this.Description = _company.description;
            this.SA = sa;
            

            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);

        }
        

        public CreateCompanyViewModel(SuperAdmin sa, IEventAggregator ea, string actionname)
        {
            this.ActionName = actionname;

            this.Name = null;
            this.Email = null;
            this.Location = null;
            this.Description = null;

            this.SA = sa;

            this.EventAggregator = ea;
            this.EventAggregator.Subscribe(this);

        }

        private SuperAdmin SA
        {
            get { return _sa; }
            set { _sa = value; }
        }
        

        public string ErrorDescription
        {
            get { return _errordescription; }
            set { 
                _errordescription = value;
                NotifyOfPropertyChange(() => ErrorDescription);
            }
        }
        

        public string ErrorLocation
        {
            get { return _errorlocation; }
            set { 
                _errorlocation = value;
                NotifyOfPropertyChange(() => ErrorLocation);
            }
        }
        

        public string ErrorEmail
        {
            get { return _erroremail; }
            set { 
                _erroremail = value;
                NotifyOfPropertyChange(() => ErrorEmail);
            }
        }
        

        public string ErrorName
        {
            get { return _errorname; }
            set { 
                _errorname = value;
                NotifyOfPropertyChange(() => ErrorName);
            }
        }
        

        public string Description
        {
            get { return _description; }
            set {
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }
        
        public string Location
        {
            get { return _location; }
            set { 
                _location = value;
                NotifyOfPropertyChange(() => Location);
            }
        }
        

        public string Email
        {
            get { return _email; }
            set { 
                _email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }
        
        public string Name
        {
            get { return _name; }
            set     { 
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        
        public void Next() {
            if (String.IsNullOrWhiteSpace(ErrorName) && String.IsNullOrWhiteSpace(ErrorEmail)
               && String.IsNullOrWhiteSpace(ErrorLocation) && String.IsNullOrWhiteSpace(ErrorDescription))
            {
                switch (ActionName)
                {
                    case "Next":
                        ConfigureInitialRole();
                        break;
                    case "Create Company":
                        CreateCompanyOnly();
                        break;
                    case "Edit Company":
                        EditCompanyOnly();
                        break;
                }
            }
            else if (String.IsNullOrWhiteSpace(Name))
            {
                ErrorName = "Please enter name of company";
            }
            else if (String.IsNullOrWhiteSpace(Email))
            {
                ErrorEmail = "Please enter email";
            }
            else if (String.IsNullOrWhiteSpace(Location))
            {
                ErrorLocation = "Please enter location";
            }
            else
            {
                ErrorDescription = "Please enter description";
            }

           
        }

        private void EditCompanyOnly()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities()) 
            {
                var company = ctx.Companies.Where(a => a.id == _company.id).SingleOrDefault<Company>();
               
                company.name = Name;
                company.email = Email;
                company.location = Location;
                company.description = Description;

                ctx.SaveChanges();

                this.EventAggregator.PublishOnUIThread("Operation complete");

                var editpage = new EditProductViewModel(this.SA, this.EventAggregator);
                var tool = new NavigatePage(editpage);
                this.EventAggregator.PublishOnUIThread(tool);

            }
        }

        private void CreateCompanyOnly()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities()) 
            {
                ctx.Companies.Add(new Company()
                {
                    name = Name,
                    email = Email,
                    location = Location,
                    description = Description,
                    superadmin_id = this.SA.id
                });
                ctx.SaveChanges();

                this.EventAggregator.PublishOnUIThread("Operation complete");

                var editpage = new EditProductViewModel(this.SA, this.EventAggregator);
                var tool = new NavigatePage(editpage);
                this.EventAggregator.PublishOnUIThread(tool);

            }
            
        }

        private void ConfigureInitialRole()
        {
            using (var ctx = new Choukash_Revamp_DemoEntities())
            {
                var company = new Company()
                {
                    name = Name,
                    email = Email,
                    location = Location,
                    description = Description,
                    superadmin_id = SA.id
                };
                if (this.NextPage == null)
                {
                    RolePage = new CreateRoleViewModel("Create Role", null, company, this.EventAggregator);
                    this.NextPage = new NavigatePage(RolePage);
                    this.EventAggregator.PublishOnUIThread(NextPage);
                }
                else
                {
                    this.EventAggregator.PublishOnUIThread(NextPage);
                }

            }   
        }
        
    }
}
