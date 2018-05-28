using Caliburn.Micro;
using ChoukashRevamp.Models;
using ChoukashRevamp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChoukashRevamp
{
    public class Bootstrapper:BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            //bool AdminExists = false;
            //using (var context = new Choukash_Revamp_DemoEntities1()) 
            //{
            //    if (context.SuperAdmins.Count<SuperAdmin>() > 0)
            //        AdminExists = true;
            //}
            //if (!AdminExists) {
            //    DisplayRootViewFor<SuperAdminViewModel>();
            //}
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
