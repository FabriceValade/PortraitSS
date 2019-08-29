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
        public SSFactionGroupCollectionViewModel() { }

        #region Loading/unloading handling
        public void OnLoaded()
        {
            ViewSource.View.Refresh();
            if (ViewSource.View.CurrentPosition < 0)
                ViewSource.View.MoveCurrentToFirst();
        }

        public void OnUnloaded()
        {


        }
        #endregion

        #region properties populated throught the view
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
                ViewSource = new CollectionViewSource() { Source = _HeldCollection };
                NotifyPropertyChanged();
            }
        }

        FilterEventHandler _CollectionFilter;
        public FilterEventHandler CollectionFilter
        {
            get => _CollectionFilter;
            set
            {
                if (ViewSource != null && _CollectionFilter != null)
                    ViewSource.Filter -= _CollectionFilter;
                _CollectionFilter = value;
                if (ViewSource != null)
                    ViewSource.Filter += _CollectionFilter;
            }
        } 
        #endregion

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

        #endregion
    }
}

