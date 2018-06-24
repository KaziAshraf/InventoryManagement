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

namespace ChoukashRevamp.ViewModels
{
    public class InventoryViewModel:Screen
    {
        private ObservableCollection<Category> _categories;
        private Choukash_Revamp_DemoEntities1 Context = new Choukash_Revamp_DemoEntities1();
        private bool _createcategory;
        private Category _currentcategory;

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

          
         
        }

       

        private ObservableCollection<Category> LoadAllCategoriesofCompany => new ObservableCollection<Category>(Context.Categories.Include(a => a.SubCategories).ToList());

        public void AddNewCategory(object sender, DataGridCellEditEndingEventArgs e)
        {
            var m = sender as DataGrid;

            var b = m.ItemsSource as ObservableCollection<Category>;

            Console.WriteLine(b.LastOrDefault().name);
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
