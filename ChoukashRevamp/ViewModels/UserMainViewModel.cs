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
        private readonly User _user;
        private readonly Choukash_Revamp_DemoEntities1 context = new Choukash_Revamp_DemoEntities1();
        private NavigatePage tool { get; set; }
        private EditProductViewModel userrolepage { get; set; }
        public IEventAggregator EventAggregator { get; set; }

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
                    if (userrolepage == null || tool.Params[0] != userrolepage)
                    {
                        userrolepage = new EditProductViewModel(_user, EventAggregator) { DisplayName = "Add User" };
                        tool = new NavigatePage(userrolepage);
                        Handle(tool);
                    }
                    else
                        Handle(tool);
                    break;
            }
        }

        public void CloseItem(Object viewModel)
        {
            DeactivateItem(viewModel, true);
            tool.Params[0] = null;
        }

        public void Handle(object message)
        {
            DeactivateItem(message, true);
        }
    }
}
