using Caliburn.Micro;
using ChoukashRevamp.Models;
using ChoukashRevamp.ViewModels.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChoukashRevamp.ViewModels
{
    public sealed class SuperAdminMainViewModel:Conductor<object>.Collection.OneActive,IHandle<NavigatePage>,IHandle<string>
    {
        private string _welcomemessage;
        public IEventAggregator EventAggregator { get; private set; }
        private SuperAdmin SA { get; set; }
        private NavigatePage NextPageCompany { get; set; }
        private NavigatePage NextPageEdit { get; set; }

        private CreateCompanyViewModel CompanyPage { get; set; }
        private EditProductViewModel EditPage { get; set; }
        public string WelcomeMessage
        {
            get { return _welcomemessage; }
            set { 
                _welcomemessage = value;
                NotifyOfPropertyChange(() => WelcomeMessage);
            }
        }

        
        public SuperAdminMainViewModel(SuperAdmin sa)
        {
            this.WelcomeMessage = "Welcome " + sa.name;
  
            this.EventAggregator = new EventAggregator();
            this.EventAggregator.Subscribe(this);

            this.SA = sa;
            
            using (var ctx = new Choukash_Revamp_DemoEntities())
            {
                if (ctx.Permissions.Count<Permission>() == 0) 
                {
                    IList<Permission> permission = new List<Permission>() 
                    {
                        new Permission(){ menu = "Category Create" },
                        new Permission(){ menu = "Categroy Delete"},
                        new Permission(){ menu = "Subcategory Create"},
                        new Permission(){ menu = "Subcategory Delete"},
                        new Permission(){ menu = "Item Create" },
                        new Permission(){ menu = "Item Delete" }
                    };
                    
                    ctx.Permissions.AddRange(permission);
                    ctx.SaveChanges();
                }
            }
            
        }

        public void ConfigureNewProduct() {
            if (this.NextPageCompany == null)
            {
                CompanyPage = new CreateCompanyViewModel(SA, EventAggregator, "Next");
                this.NextPageCompany = new NavigatePage(CompanyPage);
                this.EventAggregator.PublishOnUIThread(NextPageCompany);
            }
            else 
            {
                this.EventAggregator.PublishOnUIThread(NextPageCompany);
            }
            
        }

        public void EditExistingProduct() 
        {
            if (this.NextPageEdit == null) 
            {
                EditPage = new EditProductViewModel(SA, EventAggregator);
                this.NextPageEdit = new NavigatePage(EditPage);
                this.EventAggregator.PublishOnUIThread(NextPageEdit);
            }
            else
            {
                this.EventAggregator.PublishOnUIThread(NextPageEdit);
            }
        }
        public void Handle(NavigatePage message)
        {
            ActivateItem(message.Params[0]);
            Console.WriteLine(Items.Count());
        }



        public void Handle(string message)
        {
            if (message != null)
            {
                for (int i = Items.Count() - 1; i >= 0; i--) 
                {
                    DeactivateItem(Items[i], true);
                }
                this.NextPageCompany = null;
                this.CompanyPage = null;
                this.NextPageEdit = null;
                this.EditPage = null;
                Console.WriteLine(Items.Count());
            }
        }
    }
}
