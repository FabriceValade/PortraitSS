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
        SSFaction _FactionModel = null;
        public SSFaction FactionModel
        {
            get =>_FactionModel;
            set
            {
                if (_FactionModel !=null)
                    if (OwningGroup!=null)
                        OwningGroup.GroupChanged -= NotifyGroupChanged;
                _FactionModel = value;
                if (OwningGroup != null)
                    OwningGroup.GroupChanged += NotifyGroupChanged;
                NotifyGroupChanged(null,null);

            }
        }
        public SSFactionGroup OwningGroup
        {
            get
            {
                if (FactionModel!=null)
                    return FactionModel.OwningGroup as SSFactionGroup;
                return null;
            }
        }
        //public bool UseForGroup { get =>FactionModel.UseForGroup; set { FactionModel.UseForGroup = value; NotifyPropertyChanged(); } }

        #region Properties queried from the owning group
        public string DisplayName
        {
            get
            {
                if (FactionModel != null)
                    return ((FactionModel.OwningGroup) as SSFactionGroup).DisplayName;
                return "Null faction";
            }
        }
        public string LogoPath
        {
            get
            {
                if (FactionModel != null)
                    return ((FactionModel.OwningGroup) as SSFactionGroup).LogoPath;
                return null;

            }
        }
        public string ColorRGB
        {
            get
            {
                string _ColorRGB=null;
                if (FactionModel != null)
                {
                    _ColorRGB = ((FactionModel.OwningGroup) as SSFactionGroup).ColorRGB;    
                }
                if (_ColorRGB == null)
                    _ColorRGB = "#FFFFFFFF";
                return _ColorRGB;
            }
        }
        #endregion

        public SSFactionViewModel() { }
        public SSFactionViewModel(SSFaction factionModel )
        {
            FactionModel = factionModel;
        }

        public void NotifyGroupChanged(object sender, EventArgs a)
        {
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("ColorRGB");
            NotifyPropertyChanged("LogoPath");
        }
    }
}
