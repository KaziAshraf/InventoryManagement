using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoukashRevamp.ViewModels.Navigator
{
    public static class Navigator
    {
        public static void Navigate<T>(params Object[] param) where T: class{
            var windowManager = new WindowManager();
            T viewModel = Activator.CreateInstance(typeof(T),param) as T;
            windowManager.ShowWindow(viewModel);
        }
    }
}
