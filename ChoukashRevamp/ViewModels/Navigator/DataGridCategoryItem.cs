using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoukashRevamp.ViewModels.Navigator
{
    public class DataGridCategoryItem : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set {
                if (_name == value)
                    return;

                _name = value;
                RaisePropertyChanged("Name");
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            PropertyChangedEventHandler propertyChangedEventHandler = PropertyChanged;

            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
