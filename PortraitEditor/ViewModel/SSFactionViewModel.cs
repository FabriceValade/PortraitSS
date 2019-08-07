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
        public SSFactionGroup OwningGroup { get => _FactionModel.OwningGroup as SSFactionGroup; }
        public bool UseForGroup { get =>FactionModel.UseForGroup; set { FactionModel.UseForGroup = value; NotifyPropertyChanged(); } }

        #region Properties queried from the owning group
        string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName != null)
                    return _DisplayName;
                _DisplayName = ((FactionModel.OwningGroup) as SSFactionGroup).DisplayName;
                return _DisplayName;

            }
        }
        string _LogoPath;
        public string LogoPath
        {
            get
            {
                if (_LogoPath != null)
                    return _LogoPath;
                _LogoPath = ((FactionModel.OwningGroup) as SSFactionGroup).LogoPath;
                return _LogoPath;

            }
        }
        string _ColorRGB;
        public string ColorRGB
        {
            get
            {
                if (_ColorRGB != null)
                    return _ColorRGB;
                _ColorRGB = ((FactionModel.OwningGroup) as SSFactionGroup).ColorRGB;
                if (_ColorRGB==null)
                    _ColorRGB="#FFFFFFFF";
                return _ColorRGB;

            }
        }
        #endregion

        public SSFactionViewModel(SSFaction factionModel )
        {
            _FactionModel = factionModel;
            OwningGroup.GroupChanged += NotifyGroupChanged;

        }

        public void NotifyGroupChanged(object sender, EventArgs a)
        {
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("ColorRGB");
            NotifyPropertyChanged("LogoPath");
        }
    }
}
