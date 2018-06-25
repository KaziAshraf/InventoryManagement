using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Specialized;
using System.Windows.Controls;
using ChoukashRevamp.ViewModels.Navigator;

namespace ChoukashRevamp.ViewModels
{
    public class InventoryViewModel:Screen
    {
        private ObservableCollection<Category> _categories;
        private Choukash_Revamp_DemoEntities1 Context = new Choukash_Revamp_DemoEntities1();
        private bool _createcategory;
        private Category _currentcategory;
        private bool CategoryCreationMode;

        private IEventAggregator EventAggregator { get; set; }
        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }


        public Category CurrentCategory
        {
            get { return _currentcategory; }
            set {
                _currentcategory = value;
                NotifyOfPropertyChange(() => CurrentCategory);
                
            }
        }


        public bool CreateCategory
        {
            get { return _createcategory; }
            set {
                _createcategory = value;
                NotifyOfPropertyChange(() => CreateCategory);
            }
        }

        public Company Company { get; set; }


        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        }

        public InventoryViewModel(User user, Company company, IEventAggregator ea)
        {
            User = user;
            Company = company;
            Categories = LoadAllCategoriesofCompany;
            CreateCategory = false;
            EventAggregator = ea;
        }

       

        private ObservableCollection<Category> LoadAllCategoriesofCompany => new ObservableCollection<Category>(Context.Categories.Include(a => a.SubCategories).ToList());

        public void AddNewCategory(object sender, int index)
        {
            if (CategoryCreationMode == true)
            {
                var datagrid = sender as DataGrid;
                var items = datagrid.ItemsSource as ObservableCollection<Category>;

                if (index == items.Count - 1)
                {
                    if (!String.IsNullOrWhiteSpace(items.Last().name))
                    {
                        Context.Categories.Add(new Category() { name = items.Last().name, companies_id = Company.id, users_id = User.id });
                        Context.SaveChanges();

                        CategoryCreationMode = false;
                    }
                } 
            }
        }

        public void TiggerCategoryCreationMode()
        {
            CategoryCreationMode = true;
        }
        public void EnableGridtoAddRow(string entity)
        {
            switch (entity)
            {
                case "Categories":
                    CreateCategory = true;
                    break;
            }
        }
    }
}
