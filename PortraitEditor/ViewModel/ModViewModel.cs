using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class ModViewModel : ViewModelBase
    {
        #region Field
        string _Name;
        URLViewModel _Url;
        #endregion

        #region properties
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        public URLViewModel Url {
            get => _Url;
            set
            {
                _Url = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public ModViewModel(string name, URLViewModel url)
        {
            Name = name;
            Url = new URLViewModel(url);
        }
        #endregion

    }
}
