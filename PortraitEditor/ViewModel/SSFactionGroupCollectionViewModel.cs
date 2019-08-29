using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PortraitEditor.ViewModel
{
    public class SSFactionGroupCollectionViewModel : ViewModelBase
    {
        public SSFactionGroupCollectionViewModel()
        { }

        #region Loading/unloading h andling
        private bool _isLoaded = false;
        public void OnLoaded()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                ViewSource.View.Refresh();
            }
        }

        public void OnUnloaded()
        {
            if (_isLoaded)
            {
                // TODO: Add your cleanup/unloaded code here 
                _isLoaded = false;
            }
        }
        #endregion
        public SSFactionGroupCollectionViewModel(ObservableCollection<SSFactionGroup> collectionToHold)
        {
            HeldCollection = collectionToHold;
        }
        ObservableCollection<SSFactionGroup> _HeldCollection = new ObservableCollection<SSFactionGroup>();
        public ObservableCollection<SSFactionGroup> HeldCollection
        {
            get
            {
                return _HeldCollection;
            }
            set
            {
                _HeldCollection = value;
                ViewSource= new CollectionViewSource() { Source = _HeldCollection };
                NotifyPropertyChanged();
            }
        }
        System.Collections.IList _SelectedStuff;
        public System.Collections.IList SelectedStuff { get=>_SelectedStuff; set { _SelectedStuff = value; } }
        public List<SSFactionGroup> SelectedItems
        {
            get
            {
                return SelectedStuff.Cast<SSFactionGroup>().ToList();
            }
        }



        #region properties for the view
        CollectionViewSource _ViewSource;
        public CollectionViewSource ViewSource
        {
            get
            {
                if (_ViewSource == null)
                    _ViewSource = new CollectionViewSource() { Source = HeldCollection };
                return _ViewSource;
            }
            set
            {
                if (_ViewSource != null && CollectionFilter != null)
                    _ViewSource.Filter -= CollectionFilter;
                _ViewSource = value;
                _ViewSource.Filter += CollectionFilter;
                NotifyPropertyChanged();
            }
        }

        private void _ViewSource_Filter(object sender, FilterEventArgs e)
        {
            var portraitGroup = e.Item as SSFactionGroup;
            e.Accepted = portraitGroup.DisplayName == "Hegemony";
        }

        CollectionView _HeldView;
        public CollectionView HeldView
        {
            get
            {
                if (_HeldView == null)
                {
                    _HeldView = ViewSource.View as CollectionView;
                }
                return _HeldView;
            }
            set
            {
                _HeldView = value;
                if (_HeldView != null)
                {
                    //_HeldView.Filter = CollectionFilter;
                }
                NotifyPropertyChanged();

            }
        }
        

        FilterEventHandler _CollectionFilter;
        public FilterEventHandler CollectionFilter
        {
            get =>_CollectionFilter;
            set
            {
                if (ViewSource != null && _CollectionFilter != null)
                    ViewSource.Filter -= _CollectionFilter;
                _CollectionFilter = value;
                if (ViewSource != null)
                    ViewSource.Filter += _CollectionFilter;
            }
        }

        public PropertyGroupDescription GroupDescription { get; set; }
        #endregion
    }
}
