using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PortraitEditor.ViewModel
{
    public class ModFactionViewModel : ViewModelBase
    {

        #region Field
        public SSMod Mod;
        
        #endregion

        #region properties
        public string Name
        {
            get => Mod.Name;          
        }

        URLViewModel _Url;
        public URLViewModel Url
        {
            get
            {
                if (_Url == null)
                {
                    _Url = new URLViewModel(Mod.Url);
                }
                return _Url;
            }
            set
            {
                _Url = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<SSFactionViewModel> _FactionCollection;
        public ObservableCollection<SSFactionViewModel> FactionCollection
        {
            get
            {
                if (_FactionCollection== null)
                {
                    var FactionQuery = from file in Mod.FileList
                                       where file is SSFaction
                                       select new SSFactionViewModel(file as SSFaction);
                    _FactionCollection = new ObservableCollection<SSFactionViewModel>(FactionQuery);
                    Mod.FileList.CollectionChanged += OnFactionsChanged;
                }
                return _FactionCollection;
            }
            set
            {
                _FactionCollection = value;
                NotifyPropertyChanged("FactionCollection");
            }
        }

        bool _UseForGroup = true;
        public bool UseForGroup
        {
            get=>_UseForGroup;
            set
            {
                _UseForGroup = value;
                foreach (SSFactionViewModel FactionViewModel in FactionCollection)
                {
                    FactionViewModel.UseForGroup = value;
                }
            } }

        bool _AllowExplore = true;
        public bool AllowExplore
        {
            get => _AllowExplore;
            set
            {
                _AllowExplore = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public ModFactionViewModel(SSMod mod)
        {
            Mod = mod;
        }

        #endregion

        #region event handler
        void OnFactionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SSFaction file in e.NewItems)
                {
                    FactionCollection.Add(new SSFactionViewModel(file as SSFaction));
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SSFaction file in e.OldItems)
                {
                    SSFactionViewModel RemovedViewModel = (from viewModel in FactionCollection
                                                           where viewModel.FactionModel == file
                                                           select viewModel).SingleOrDefault();
                    FactionCollection.Remove(RemovedViewModel);
                }
        }
        #endregion



    }
}
