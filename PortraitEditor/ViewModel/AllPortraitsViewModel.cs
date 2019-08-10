using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class AllPortraitsViewModel : ViewModelBase
    {

        public AllPortraitsViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory)
        {
            FactionDirectory = factionDirectory;
        }

        SSFileDirectory<SSFactionGroup, SSFaction> FactionDirectory { get; set; }

        ObservableCollection<SSPortrait> _AllPortraits= new ObservableCollection<SSPortrait>();
        public ObservableCollection<SSPortrait> AllPortraits
        {
            get
            {
                ObservableCollection<SSPortrait> a = new ObservableCollection<SSPortrait>(FactionDirectory.GroupDirectory.SelectMany(x => x.Portraits).Distinct(new PortraitNoGenderEqualityComparer()).ToList());
                if (_AllPortraits.Count() > 0)
                {
                    _AllPortraits.Clear();
                    foreach (SSPortrait port in a)
                    {
                        _AllPortraits.Add(port);
                    }
                }else
                {
                    foreach (SSPortrait port in a)
                    {
                        _AllPortraits.Add(port);
                    }
                }
                return _AllPortraits;
            }
        }
    }
}
