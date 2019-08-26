using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.Model.SSParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortraitEditor.ViewModel
{
    public class SSFactionGroupViewModel : ViewModelBase
    {
        SSFactionGroup _FactionGroupModel = new SSFactionGroup();
        public SSFactionGroup FactionGroupModel
        {
            get
            {
                //if (_FactionGroupModel == null)
                //    throw new InvalidOperationException();
                return _FactionGroupModel;
            }
            set
            {
                if (value == null)
                    _FactionGroupModel = new SSFactionGroup();
                else
                    _FactionGroupModel = value;

                NotifyPropertyChanged("DisplayName");
                NotifyPropertyChanged("LogoPath");
                NotifyPropertyChanged("ColorRGB");
                NotifyPropertyChanged("ContributingMods");

            }
        }
        public SSFactionGroupViewModel() { }

        public SSFactionGroupViewModel(SSFactionGroup factionGroupModel)
        {
            FactionGroupModel = factionGroupModel;
            FactionGroupModel.GroupChanged += FactionGroupModel_GroupChanged;
        }

        
        #region Properties of the group
        string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName != null)
                    return _DisplayName;
                _DisplayName = FactionGroupModel.DisplayName;
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
                _LogoPath = FactionGroupModel.LogoPath;
                return _LogoPath;

            }
        }

        string _ColorRGB;
        public string ColorRGB
        {
            get
            {
                _ColorRGB = FactionGroupModel.ColorRGB;
                return _ColorRGB;

            }
        }
        public bool IsSelected { get; set; }
        public List<string> ContributingMods
        {
            get
            {
                List<string> result = (from faction in FactionGroupModel.FileList
                                       select faction.ModSource.Name).Distinct().ToList();
                return result;
            }
        }
        #endregion

        private void FactionGroupModel_GroupChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("LogoPath");
            NotifyPropertyChanged("ColorRGB");
            NotifyPropertyChanged("ContributingMods");
        }

    }
}
