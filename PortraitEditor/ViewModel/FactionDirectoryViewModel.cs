﻿using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PortraitEditor.ViewModel 
{
    public class FactionDirectoryViewModel : ViewModelBase
    {
        SSFileDirectory<SSFactionGroup, SSFaction> FactionDirectory { get; set; }

        ObservableCollection<SSFactionGroupViewModel> _FactionGroupList;
        public ObservableCollection<SSFactionGroupViewModel> FactionGroupList
        {
            get
            {
                if(_FactionGroupList==null)
                {
                    var FactionQuery = from factionGroup in FactionDirectory.GroupDirectory
                                       select new SSFactionGroupViewModel(factionGroup);
                    _FactionGroupList = new ObservableCollection<SSFactionGroupViewModel>(FactionQuery);
                    FactionDirectory.GroupDirectory.CollectionChanged += OnFactionsGroupChanged;
                }
                return _FactionGroupList;
            }
        }

        ObservableCollection<SSParameterArrayChangesViewModel<SSPortrait>> _PortraitEdit;
        public ObservableCollection<SSParameterArrayChangesViewModel<SSPortrait>> PortraitEdit
        {
            get
            {
                if (_PortraitEdit == null)
                {
                    _PortraitEdit = new ObservableCollection<SSParameterArrayChangesViewModel<SSPortrait>>((from factionViewModel in FactionGroupList
                                                                                                            select factionViewModel.PortraitsParameterArrayChange));
                    FactionGroupList.CollectionChanged += FactionGroupList_CollectionChanged;
                }
                return _PortraitEdit;
            }
            set
            {
                _PortraitEdit = value;
                NotifyPropertyChanged("PortraitEdit");
            }
        }

        

        CollectionView _FactionGroupView;
        public CollectionView FactionGroupView
        {
            get
            {
                if (_FactionGroupView == null)
                {
                    _FactionGroupView = (CollectionView)CollectionViewSource.GetDefaultView(FactionGroupList);

                }
                return _FactionGroupView;;
            }
        }

        public SSFactionGroupViewModel SelectedItem
        {
            get
            {
                return FactionGroupView.CurrentItem as SSFactionGroupViewModel;
            }
        }

        public FactionDirectoryViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory)
        {
            FactionDirectory = factionDirectory;
        }

        #region event handler
        void OnFactionsGroupChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SSFactionGroup file in e.NewItems)
                {
                    FactionGroupList.Add(new SSFactionGroupViewModel(file as SSFactionGroup));
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SSFactionGroup file in e.OldItems)
                {
                    SSFactionGroupViewModel RemovedViewModel = (from viewModel in FactionGroupList
                                                                where viewModel.FactionGroupModel == file
                                                                select viewModel).SingleOrDefault();
                    FactionGroupList.Remove(RemovedViewModel);
                }
        }

        private void FactionGroupList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SSFactionGroupViewModel file in e.NewItems)
                {
                    PortraitEdit.Add((file as SSFactionGroupViewModel).PortraitsParameterArrayChange);
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SSFactionGroupViewModel file in e.OldItems)
                {
                    SSParameterArrayChangesViewModel<SSPortrait> RemovedViewModel = (from viewModel in PortraitEdit
                                                                         where viewModel == file.PortraitsParameterArrayChange
                                                                         select viewModel).SingleOrDefault();
                    PortraitEdit.Remove(RemovedViewModel);
                }
        }
        #endregion
    }
}
