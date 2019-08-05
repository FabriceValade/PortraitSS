using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel 
{
    public class SSFactionViewModel : ViewModelBase
    {
        SSFaction _FactionModel;
        public SSFaction FactionModel { get=>_FactionModel; }


        string _DisplayName; public string DisplayName
        {
            get
            {
                if (_DisplayName != null)
                    return _DisplayName;
                _DisplayName = ((FactionModel.OwningGroup) as SSFactionGroup).DisplayName;
                return _DisplayName;

            }
        }
        public SSFactionViewModel(SSFaction factionModel )
        {
            _FactionModel = factionModel;

        }

        public void NotifyGroupChanged()
        {
            NotifyPropertyChanged("DisplayName");
        }
    }
}
