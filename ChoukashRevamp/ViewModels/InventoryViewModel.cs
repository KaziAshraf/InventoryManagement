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
        private ObservableCollection<DataGridCategoryItem> _tests;

        public ObservableCollection<DataGridCategoryItem> Tests
        {
            get { return _tests; }
            set {
                _tests = value;
                NotifyOfPropertyChange(() => Tests);
            }
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

        public InventoryViewModel(Company company)
        {
            Company = company;
            Categories = LoadAllCategoriesofCompany;
            CreateCategory = false;
            Tests = new ObservableCollection<DataGridCategoryItem>() { new DataGridCategoryItem() { Name = "Milk" } };
          
         
        }

       

        private ObservableCollection<Category> LoadAllCategoriesofCompany => new ObservableCollection<Category>(Context.Categories.Include(a => a.SubCategories).ToList());

        public void AddNewCategory(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.EditAction == DataGridEditAction.Commit)
            {
                var m = e.Row.DataContext as DataGridCategoryItem;
                Console.WriteLine(m.Name);
            }
        }

        public void Test()
        {
            Console.WriteLine(Categories.Last().name);
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
