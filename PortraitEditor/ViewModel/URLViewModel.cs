using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortraitEditor.Model;

namespace PortraitEditor.ViewModel
{
    public enum URLstate { Exist, ExistNot, Unset }
    public class URLViewModel : ViewModelBase
    {
        
        URLRelative _PointedUrl;
        
        #region Properties
        public URLRelative PointedUrl
        {
            get
            {
                return _PointedUrl;
            }
            set
            {
                _PointedUrl = value;
                base.NotifyPropertyChanged();
                base.NotifyPropertyChanged("DisplayUrl");
                base.NotifyPropertyChanged("UrlState");
            }
        }

        public string DisplayUrl
        {
            get
            {
                return _PointedUrl.FullUrl;
            }
        }

        public string DisplayName { get; set; }

        public URLstate UrlState
        {
            get 
            {
                if (_PointedUrl.FullUrl == null)
                    return URLstate.Unset;
                if (_PointedUrl.Exist() == true)
                    return URLstate.Exist;
                else
                    return URLstate.ExistNot;
            }
        }
        #endregion

        #region Constructors
        public URLViewModel(string url, string displayName)
        {
            PointedUrl = new URLRelative(url,null,null);
            DisplayName = displayName;
        }
        #endregion

    }
}
