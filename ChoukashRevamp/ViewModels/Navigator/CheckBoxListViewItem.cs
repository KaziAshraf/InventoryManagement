using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoukashRevamp.ViewModels.Navigator
{
    public sealed class CheckBoxListViewItem : INotifyPropertyChanged 
    { 
        private bool isChecked; 
        private string text; 
        public bool IsChecked 
        {
            get { return isChecked; } 
            set { 
                if (isChecked == value) return; 
                isChecked = value; 
                RaisePropertyChanged("IsChecked"); 
            } 
        } 
        public String Text 
        { 
            get { return text; } 
            set { 
                if (text == value) return; 
                text = value; 
                RaisePropertyChanged("Text"); 
            }
        } 
        public CheckBoxListViewItem(string t, bool c) 
        { 
            this.Text = t; 
            this.IsChecked = c; 
        } 
        
        public event PropertyChangedEventHandler PropertyChanged; 
        private void RaisePropertyChanged(string propName) 
        { 
            PropertyChangedEventHandler eh = PropertyChanged; 
            if (eh != null) 
            { 
                eh(this, new PropertyChangedEventArgs(propName)); 
            } 
        }

        
    }
}
