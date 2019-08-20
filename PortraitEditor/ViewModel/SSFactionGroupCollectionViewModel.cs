using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PortraitEditor.ViewModel
{
    public class SSFactionGroupCollectionViewModel : ViewModelBase
    {
        public SSFactionGroupCollectionViewModel()
        { }
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
        #endregion
    }
}
