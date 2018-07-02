using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace ChoukashRevamp.ViewModels
{
    public class InventoryViewModel:Screen
    {
        #region Declaration
        private ObservableCollection<Category> _categories;
        private ObservableCollection<SubCategory> _subcategories;

        private Choukash_Revamp_DemoEntities1 Context = new Choukash_Revamp_DemoEntities1();
        private bool _createcategory;
        private bool _createsubcategory;

        private Category _currentcategory;
        private bool CategoryCreationMode;
        private bool SubCategoryCreationMode;

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
            set
            {
                _currentcategory = value;
                NotifyOfPropertyChange(() => CurrentCategory);

            }
        }
        public bool CreateSubCategory
        {
                get { return _createsubcategory; }
                set {
                    _createsubcategory = value;
                    NotifyOfPropertyChange(() => CreateSubCategory);
                }
        }

        public bool CreateCategory
        {
            get { return _createcategory; }
            set
            {
                _createcategory = value;
                NotifyOfPropertyChange(() => CreateCategory);
            }
        }

        public Company Company { get; set; }

        public ObservableCollection<SubCategory> SubCategories
        {
            get { return _subcategories; }
            set {
                _subcategories = value;
                NotifyOfPropertyChange(() => SubCategories);
            }
        }
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        } 
        #endregion

        public InventoryViewModel(User user, Company company, IEventAggregator ea)
        {
            User = user;
            Company = company;
            Categories = LoadAllCategoriesofCompany;
            
            CreateCategory = false;
            CreateSubCategory = false;
            EventAggregator = ea;
        }
        #region Category

        private ObservableCollection<Category> LoadAllCategoriesofCompany => new ObservableCollection<Category>(Context.Categories.Include(a => a.SubCategories).ToList());

        public void TiggerCategoryCreationMode()
        {
            CategoryCreationMode = true;
        }
        public void AddEditCategory(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox textBox = e.EditingElement as TextBox;
            var currentRow = e.Row.DataContext as Category;
            if (!CategoryCreationMode)
            {
                if (String.IsNullOrWhiteSpace(textBox.Text))
                {
                    e.Cancel = true;
                }
                else
                {
                    var editedrow = Context.Categories.Where(a => a.id == currentRow.id).SingleOrDefault();
                    editedrow.name = textBox.Text;
                    Context.SaveChanges();
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(textBox.Text))
                {
                    var rowIndex = e.Row.GetIndex();
                    if (rowIndex == Categories.Count - 1)
                    {
                        Context.Categories.Add(new Category() { name = textBox.Text, companies_id = Company.id, users_id = User.id });
                        Context.SaveChanges();

                    }
                    else
                    {
                        var editedrow = Context.Categories.Where(a => a.id == currentRow.id).SingleOrDefault();
                        editedrow.name = textBox.Text;
                        Context.SaveChanges();
                    }
                    Categories = LoadAllCategoriesofCompany;
                }
                else
                    e.Cancel = true;
            }
        }

        public void DeleteCategory(object sender, KeyEventArgs e)
        {
            var datagrid = sender as DataGrid;
            var row = datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex) as DataGridRow;
            if (Key.Delete == e.Key && !row.IsEditing && !row.IsNewItem)
            {
                var result = MessageBox.Show("Do You want to delete this category?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Context.Categories.Remove(datagrid.SelectedItem as Category);
                    Context.SaveChanges();
                    Categories = LoadAllCategoriesofCompany;
                }
                else
                    e.Handled = true;
            }
            else if (Key.Escape == e.Key)
            {
                Categories = LoadAllCategoriesofCompany;
                CreateCategory = false;

            }
        }
        #endregion

        #region SubCategory
        public void LoadSubCategoryFromCategory(Category category)
        {
            SubCategories = new ObservableCollection<SubCategory>(Context.SubCategories.Where(a => a.categories_id == category.id).ToList());
        }
        public void TiggerSubCategoryCreationMode()
        {
            SubCategoryCreationMode = true;
        }
        public void AddEditSubCategory(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox textBox = e.EditingElement as TextBox;
            var currentRow = e.Row.DataContext as SubCategory;
            if (!SubCategoryCreationMode)
            {
                if (String.IsNullOrWhiteSpace(textBox.Text))
                {
                    e.Cancel = true;
                }
                else
                {
                    var editedrow = Context.SubCategories.Where(a => a.id == currentRow.id).SingleOrDefault();
                    editedrow.name = textBox.Text;
                    Context.SaveChanges();
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(textBox.Text))
                {
                    var rowIndex = e.Row.GetIndex();
                    if (rowIndex == Categories.Count - 1)
                    {
                        Context.Categories.Add(new Category() { name = textBox.Text, companies_id = Company.id, users_id = User.id });
                        Context.SaveChanges();

                    }
                    else
                    {
                        var editedrow = Context.Categories.Where(a => a.id == currentRow.id).SingleOrDefault();
                        editedrow.name = textBox.Text;
                        Context.SaveChanges();
                    }
                    Categories = LoadAllCategoriesofCompany;
                }
                else
                    e.Cancel = true;
            }
        }

        public void DeleteSubCategory(object sender, KeyEventArgs e)
        {
            var datagrid = sender as DataGrid;
            var row = datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex) as DataGridRow;
            if (Key.Delete == e.Key && !row.IsEditing && !row.IsNewItem)
            {
                var result = MessageBox.Show("Do You want to delete this category?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Context.Categories.Remove(datagrid.SelectedItem as Category);
                    Context.SaveChanges();
                    Categories = LoadAllCategoriesofCompany;
                }
                else
                    e.Handled = true;
            }
            else if (Key.Escape == e.Key)
            {
                Categories = LoadAllCategoriesofCompany;
                CreateCategory = false;

            }
        }
        #endregion

        public void EnableGridtoAddRow(string entity)
        {
            switch (entity)
            {
                case "Categories":
                    CreateCategory = true;
                    break;
                case "SubCategories":
                    CreateSubCategory = true;
                    break;
            }
        }
    }
}
