using PortraitEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class SSModTreeViewModel : ViewModelBase
    {
        public SSModTreeViewModel() { }
        ObservableCollection<SSMod> _ModCollection= new ObservableCollection<SSMod>();
        public ObservableCollection<SSMod> ModCollection
        {
            get=> _ModCollection;
            set { _ModCollection=value; NotifyPropertyChanged(); }
        } 
    }
}
