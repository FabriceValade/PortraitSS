using PortraitEditor.Model;
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
    public class AllPortraitsViewModel : ViewModelBase
    {

        public AllPortraitsViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory)
        {
            FactionDirectory = factionDirectory;
        }

        SSFileDirectory<SSFactionGroup, SSFaction> FactionDirectory { get; set; }

        ObservableCollection<SSPortrait> _Portraits;
        public ObservableCollection<SSPortrait> Portraits
        {
            get
            {
                if (_Portraits == null)
                {
                    _Portraits = new ObservableCollection<SSPortrait>(FactionDirectory.GroupDirectory.SelectMany(x => x.Portraits).Distinct(new PortraitNoGenderEqualityComparer()).ToList());

                }
                
                return _Portraits;
            }
        }

        CollectionView _PortraitsView;
        public CollectionView PortraitsView
        {
            get
            {
                if (_PortraitsView == null)
                {
                    _PortraitsView = (CollectionView)CollectionViewSource.GetDefaultView(Portraits);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("SourceMod", new PortraitModToGroupConverter());
                    _PortraitsView.GroupDescriptions.Add(groupDescription);
                }
                return _PortraitsView;
            }
        }


    }
}
