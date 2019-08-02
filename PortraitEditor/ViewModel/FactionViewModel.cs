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
        Faction _Faction;
        public Faction Faction
        {
            get => _Faction;
            set
            {
                _Faction = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FactionUrl");
                NotifyPropertyChanged("Name");
            }
        }

        public string Name
        {
            get=>Faction.Name;
            set
            {
                Faction.Name = value;
                NotifyPropertyChanged();
            }
        }
        URLViewModel _FactionUrl;
        public URLViewModel FactionUrl
        {
            get
            {
                if (_FactionUrl == null)
                    _FactionUrl = new URLViewModel() { PointedUrl=Faction.FactionFileUrl};
                return _FactionUrl;
            }
        }

        public FactionViewModel() { }
        public FactionViewModel(URLViewModel FactionPath)
        {
            Faction PotentialFaction = new Faction();
            PotentialFaction.FactionFileUrl= FactionPath.PointedUrl;
            PotentialFaction.ReadJson();
            Faction = PotentialFaction;
        }
    }
}
