using Caliburn.Micro;
using ChoukashRevamp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChoukashRevamp.ViewModels.Navigator;
using System.ComponentModel.Design;

namespace ChoukashRevamp.ViewModels
{
    public sealed class ShellViewModel:Conductor<object>,IHandle<NavigatePage>
    {
        private int _windowwidth;
        private int _windowheight;


        public IEventAggregator EventAggregator { get; private set; }
        public int WindowWidth
        {
            get { return _windowwidth; }
            set { 
                _windowwidth = value;
                NotifyOfPropertyChange(() => WindowWidth);
            }
        }

        public int WindowHeight
        {
            get { return _windowheight; }
            set { 
                _windowheight = value;
                NotifyOfPropertyChange(() => WindowHeight);
            }
        }
        
        
        public ShellViewModel()
        {
            this.WindowHeight = 500;
            this.WindowWidth = 600;

            using (var ctx = new Choukash_Revamp_DemoEntities1()) {
                int __count = ctx.SuperAdmins.Count<SuperAdmin>();
                if (__count == 0)
                {
                    ActivateItem(new SuperAdminCreateViewModel());
                }
                else {
                    
                    ActivateItem(new UserLoginViewModel());
                }
            }
            
        }
        public ShellViewModel(User user) 
        { 
            
            
        }

        public ShellViewModel(SuperAdmin sa) 
        {
            this.WindowHeight = 700;
            this.WindowWidth = 1200;
            EventAggregator = new EventAggregator();
            EventAggregator.Subscribe(this);
            EventAggregator.PublishOnUIThread(new NavigatePage(new SuperAdminMainViewModel(sa)));
            
        }

        public void CloseApplication() {
            base.TryClose();
        }
        public void Handle(NavigatePage message)
        {
            ActivateItem(message.Params[0]);
        }


    }
}
