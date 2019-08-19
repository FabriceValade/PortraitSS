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
    public class SSExternalObjectCollectionViewModel<T> where T:ISSExternal
    {
        public SSExternalObjectCollectionViewModel(ObservableCollection<T> collectionToHold)
        {
            HeldCollection = collectionToHold;
        }
        public ObservableCollection<T> HeldCollection { get; }

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
        public string Button1Text { get; set; }
        public Visibility Button1Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button1Command;
        public ICommand Button1Command
        {
            get => _Button1Command;
            set => _Button1Command = value;
        }
        #endregion
        #region Button2
        public string Button2Text { get; set; }
        public Visibility Button2Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button2Command;
        public ICommand Button2Command
        {
            get => _Button2Command;
            set => _Button2Command = value;
        }
        #endregion
        #region Button3
        public string Button3Text { get; set; }
        public Visibility Button3Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button3Command;
        public ICommand Button3Command
        {
            get => _Button3Command;
            set => _Button3Command = value;
        }
        #endregion
        #endregion

    }
}
