using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }
                return _FactionCollection;
            }
            set
            {
                _FactionCollection = value;
                NotifyPropertyChanged("FactionCollection");
            }
        }
        
        
        #endregion

        #region Constructor
        public ModFactionViewModel(SSMod mod)
        {
            Mod = mod;
            Mod.FileAdded += Mod_FileAdded;
            Mod.FileRemoved += Mod_FileRemoved;
        }

        private void Mod_FileRemoved(object sender, SSFile e)
        {
            if (e is SSFaction)
            {
                SSFactionViewModel toRemove = (from factionViewModel in FactionCollection
                                               where factionViewModel.FactionModel == e
                                               select factionViewModel).SingleOrDefault();
                FactionCollection.Remove(toRemove);
                NotifyPropertyChanged("FactionCollection");
            }
        }

        private void Mod_FileAdded(object sender, SSFile e)
        {
            if (e is SSFaction)
            {
                FactionCollection.Add(new SSFactionViewModel(e as SSFaction));
                NotifyPropertyChanged("FactionCollection");
            }
        }
        #endregion

        #region event handler
        #endregion

        #region method
        #endregion
    }
}
