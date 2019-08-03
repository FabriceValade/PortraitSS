using PortraitEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class FactionViewModel : ViewModelBase
    {
        #region field
        Faction _Faction;
        URLViewModel _FactionUrl; 
        #endregion

        #region properties
        public Faction Faction
        {
            get => _Faction;
            set
            {
                _Faction = value;
                base.NotifyPropertyChanged();
                base.NotifyPropertyChanged("FactionUrl");
                base.NotifyPropertyChanged("Name");
            }
        }

        public string Name
        {
            get => Faction.Name;
            set
            {
                Faction.Name = value;
                base.NotifyPropertyChanged();
            }
        }

        public URLViewModel FactionUrl
        {
            get
            {
                if (_FactionUrl == null)
                    _FactionUrl = new URLViewModel() { PointedUrl = Faction.FactionFileUrl };
                return _FactionUrl;
            }
        } 
        #endregion

        #region constructors
        private FactionViewModel() { }
        public FactionViewModel(URLViewModel FactionPath)
        {
            Faction PotentialFaction = new Faction();
            PotentialFaction.FactionFileUrl = FactionPath.PointedUrl;
            PotentialFaction.ReadJson();
            Faction = PotentialFaction;
        } 
        #endregion
    }
}
