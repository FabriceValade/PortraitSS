using PortraitEditor.Model.SSParameters;
using PortraitEditor.Model.SSParameters.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortraitEditor.ViewModel
{
    public class SSExternalObjectCollectionViewModel<T> : ViewModelBase where T:ISSExternal
    {
        public SSExternalObjectCollectionViewModel()
        { }
        public SSExternalObjectCollectionViewModel(ObservableCollection<T> collectionToHold)
        {
            HeldCollection = collectionToHold;
        }
        ObservableCollection<T> _HeldCollection = new ObservableCollection<T>();
        public ObservableCollection<T> HeldCollection
        {
            get
            {
                return _HeldCollection;
            }
            set
            {
                _HeldCollection = value;
                _HeldView = (CollectionView)CollectionViewSource.GetDefaultView(_HeldCollection);
                _HeldView.GroupDescriptions.Add(GroupDescription);
                NotifyPropertyChanged("HeldView");
            }
        }

        #region properties for the view
        CollectionView _HeldView;
        public CollectionView HeldView
        {
            get
            {
                if (_HeldView == null)
                {
                    _HeldView = (CollectionView)CollectionViewSource.GetDefaultView(HeldCollection);
                    _HeldView.GroupDescriptions.Add(GroupDescription);
                }
                return _HeldView;
            }
        }
        public PropertyGroupDescription GroupDescription { get; set; }
        //= new PropertyGroupDescription();

        #region Button1
        string _Button1Text = null;
        public string Button1Text { get=>_Button1Text; set { _Button1Text = value; NotifyPropertyChanged(); NotifyPropertyChanged("Button1Visibility"); } }
        public Visibility Button1Visibility { get { if (Button1Text != null) return Visibility.Visible; else return Visibility.Collapsed; } }
        ICommand _Button1Command;
        public ICommand Button1Command
        {
            get => _Button1Command;
            set { _Button1Command = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region Button2
        string _Button2Text = null;
        public string Button2Text { get => _Button2Text; set { _Button2Text = value; NotifyPropertyChanged(); NotifyPropertyChanged("Button2Visibility"); } }
        public Visibility Button2Visibility { get { if (Button2Text != null) return Visibility.Visible; else return Visibility.Collapsed; } }
        ICommand _Button2Command;
        public ICommand Button2Command
        {
            get => _Button2Command;
            set { _Button2Command = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region Button3
        string _Button3Text = null;
        public string Button3Text { get => _Button3Text; set { _Button3Text = value; NotifyPropertyChanged(); NotifyPropertyChanged("Button3Visibility"); } }
        public Visibility Button3Visibility { get { if (Button3Text != null) return Visibility.Visible; else return Visibility.Collapsed; } }
        ICommand _Button3Command;
        public ICommand Button3Command
        {
            get => _Button3Command;
            set { _Button3Command = value; NotifyPropertyChanged(); }
        }
        #endregion
        #endregion

    }
    public class SSExternalPortraitCollectionViewModel :SSExternalObjectCollectionViewModel<SSPortrait>
    {

    }
}
