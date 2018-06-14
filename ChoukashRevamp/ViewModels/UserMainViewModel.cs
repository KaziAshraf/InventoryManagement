using Caliburn.Micro;
using ChoukashRevamp.Models;
using ChoukashRevamp.ViewModels.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoukashRevamp.ViewModels
{
    public class UserMainViewModel:Conductor<object>.Collection.OneActive,IHandle<NavigatePage>,IHandle<Object>
    {
        #region Decleration
        private readonly User _user;
        private readonly Choukash_Revamp_DemoEntities1 context = new Choukash_Revamp_DemoEntities1();
        private EditProductViewModel userrolepage { get; set; }
        private InventoryViewModel inventorypage { get; set; }
        public IEventAggregator EventAggregator { get; set; } 
        #endregion

        public UserMainViewModel(User user)
        {
            this._user = user;
            EventAggregator = new EventAggregator();
            EventAggregator.Subscribe(this);
        }

        public void Handle(NavigatePage message)
        {
            ActivateItem(message.Params[0]);
            
        }

        public void Navigate(string menu)
        {
            switch (menu)
            {
                case "UsersRoles":
                    if (userrolepage == null)
                    {
                        userrolepage = new EditProductViewModel(_user, EventAggregator) { DisplayName = "Add User" };
                        ActivateItem(userrolepage);
                    }
                    else
                        ActivateItem(userrolepage);
                    break;
                case "Inventory":
                    if (inventorypage == null)
                    {
                        inventorypage = new InventoryViewModel(_user.Company) { DisplayName = "Inventory" };
                        ActivateItem(inventorypage);
                    }
                    else
                        ActivateItem(inventorypage);
                    break;

            }
        }

        public void CloseItem(Object viewModel)
        {
            DeactivateItem(viewModel, true);
            
        }

        public void Handle(object message)
        {
            DeactivateItem(message, true);
        }
    }
}
