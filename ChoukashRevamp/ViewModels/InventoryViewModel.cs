using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ChoukashRevamp.ViewModels
{
    public class InventoryViewModel:Screen
    {
        private ObservableCollection<Category> _categories;
        private Choukash_Revamp_DemoEntities1 Context = new Choukash_Revamp_DemoEntities1();
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

          

        }

       

        private ObservableCollection<Category> LoadAllCategoriesofCompany => new ObservableCollection<Category>(Context.Categories.Include(a => a.SubCategories).ToList());

        public void AddNewCategory(Category category)
        {
            
        }
    }
}
