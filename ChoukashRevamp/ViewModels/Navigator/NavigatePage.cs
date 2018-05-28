using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoukashRevamp.ViewModels.Navigator
{
    public sealed class NavigatePage
    {
        public readonly Object[] Params;
        public NavigatePage(params Object[] param)
        {
            this.Params = param;
        }
    }
}
