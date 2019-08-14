using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSFileWritterViewModel : ViewModelBase
    {
        public SSMod L_PeSSMod { get; set; }
        public ObservableCollection<SSFactionGroupViewModel> FactionGroupList { get; set; }

        ObservableCollection<SSFactionGroupViewModel> _ModifiedFactionList;
        public ObservableCollection<SSFactionGroupViewModel> ModifiedFactionList
        {
            get
            {
                _ModifiedFactionList = new ObservableCollection<SSFactionGroupViewModel>(from faction in FactionGroupList
                                                                                         where faction.PortraitsParameterArrayChange.IsChanged
                                                                                         select faction);
                return _ModifiedFactionList;
            }
        }



        FileWriterWindow WindowView;

      

        public SSFileWritterViewModel( SSMod l_PeSSMod, ObservableCollection<SSFactionGroupViewModel> factionGroupList)
        {
            L_PeSSMod = l_PeSSMod;
            FactionGroupList = factionGroupList;
        }

        public void ShowDialog()
        {
            NotifyPropertyChanged("ModifiedFactionList");
            WindowView = new FileWriterWindow(this);
            WindowView.ShowDialog();
            return;
        }


        class ModifiedFactionGroup
        {
            public SSFileGroup<SSFaction> FileGroup { get; set; }
            public SSParameterArrayChangesViewModel<SSPortrait> Modification { get; set; }

            public ObservableCollection<SSPortrait> AddedPortrait
            {
                get
                {
                    return Modification.AddedList;
                }
            }
            public ObservableCollection<SSPortrait> RemovedPortrait
            {
                get
                {
                    return Modification.RemovedList;
                }
            }

        }

    }
}
