using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using PortraitEditor.Model;

namespace PortraitEditor.ViewModel
{
    public enum URLstate { Exist, ExistNot, Unset }
    public class URLViewModel : ViewModelBase
    {
        #region Field
        URLRelative _PointedUrl;
        #endregion

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
        public string CommonUrl
        {
            get { return PointedUrl.CommonUrl; }
            set { PointedUrl.CommonUrl = value; NotifyPropertyChanged(); base.NotifyPropertyChanged("DisplayUrl"); base.NotifyPropertyChanged("UrlState"); }
        }
        public string LinkingUrl
        {
            get { return PointedUrl.LinkingUrl; }
            set { PointedUrl.LinkingUrl = value; NotifyPropertyChanged(); base.NotifyPropertyChanged("DisplayUrl"); base.NotifyPropertyChanged("UrlState"); }
        }
        public string RelativeUrl
        {
            get { return PointedUrl.RelativeUrl; }
            set { PointedUrl.RelativeUrl = value; NotifyPropertyChanged(); base.NotifyPropertyChanged("DisplayUrl"); base.NotifyPropertyChanged("UrlState"); }
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
        public URLViewModel(string displayName)
        {
            PointedUrl = new URLRelative();
            DisplayName = displayName;
        }
        #endregion

    }

    public class EditableURLViewModel :  URLViewModel
    {
        #region Command properties
        RelayCommand<object> _SelectNewUrlCommand;
        public ICommand SelectNewUrlCommand
        {
            get
            {
                if (_SelectNewUrlCommand == null)
                {
                    _SelectNewUrlCommand = new RelayCommand<object>(param => this.SelectNewUrl_Execute());
                }
                return _SelectNewUrlCommand;
            }
        }
        #endregion

        #region Properties
        public string ButtonName { get; set; }
        #endregion

        #region Constructors
        public EditableURLViewModel(string displayName,string buttonName)
            :base(displayName)
        {
            ButtonName = buttonName;
        }
        #endregion

        #region helper method
        private void SelectNewUrl_Execute()
        {
            VistaFolderBrowserDialog OpenRootFolder = new VistaFolderBrowserDialog();
            if (OpenRootFolder.ShowDialog() == true)
            {
                base.LinkingUrl = null;
                base.RelativeUrl = null;
                base.CommonUrl = OpenRootFolder.SelectedPath;

            }
            return;
        }
        #endregion

    }
}
