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
    public class UserMainViewModel:Conductor<object>.Collection.OneActive,IHandle<NavigatePage>
    {
        //private string _displayname;

        //public string DisplayName
        //{
        //    get { return _displayname; }
        //    set {
        //        _displayname = value;
        //        NotifyOfPropertyChange(() => DisplayName);
        //    }
        //}
        private readonly User _user;
        private readonly Choukash_Revamp_DemoEntities1 context = new Choukash_Revamp_DemoEntities1();
        public IEventAggregator EventAggregator { get; set; }

        public UserMainViewModel(User user)
        {
            this._user = user;
            EventAggregator = new EventAggregator();
            EventAggregator.Subscribe(this);
        }

        public void Handle(NavigatePage message)
        {
            throw new NotImplementedException();
            
        }

        public void Navigate(string menu)
        {
            switch (menu)
            {
                case "User":
                    ActivateItem(new CreateUserViewModel("Create User", _user.Company, _user.Role, null, EventAggregator) { DisplayName = "Create User" });
                    break;
            }
        }

        public void CloseItem(Object o)
        {
            DeactivateItem(o, true);
            Console.WriteLine(Items.Count);
        }

        

    }
}
