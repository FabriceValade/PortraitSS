using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
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
        public SSFactionGroup FactionGroupModel { get; }
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
                if (_ColorRGB != null)
                    return _ColorRGB;
                _ColorRGB = FactionGroupModel.ColorRGB;
                if (_ColorRGB == null)
                    _ColorRGB = "#FFFFFFFF";
                return _ColorRGB;

            }
        }

        SSParameterArrayChangesViewModel<SSPortrait> _PortraitsParameterArrayChange;
        public SSParameterArrayChangesViewModel<SSPortrait> PortraitsParameterArrayChange
        {
            get
            {
                if (_PortraitsParameterArrayChange == null)
                    _PortraitsParameterArrayChange = new SSParameterArrayChangesViewModel<SSPortrait>(FactionGroupModel.Portraits, new PortraitGenderEqualityComparer());

                return _PortraitsParameterArrayChange;
            }
        }
        public ObservableCollection<SSPortrait> AddedPortraits
        {
            get
            {
                return PortraitsParameterArrayChange.AddedList;
            }
        }
        public ObservableCollection<SSPortrait> RemovedPortraits
        {
            get
            {
                return PortraitsParameterArrayChange.RemovedList;
            }
        }
        public ObservableCollection<SSPortrait> Portraits
        {
            get
            {
                //if (_PortraitsParameterArrayChange != null)
                    return PortraitsParameterArrayChange.ResultingList;

                //_PortraitsParameterArrayChange = new SSParameterArrayChangesViewModel<SSPortrait>(new ObservableCollection<SSPortrait>(FactionGroupModel.Portraits), new PortraitGenderEqualityComparer());
               // return _PortraitsParameterArrayChange.ResultingList;

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
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("UsingMod", new PortraitModToGroupConverter());
                    _PortraitsView.GroupDescriptions.Add(groupDescription);
                }
                return _PortraitsView;
            }
        }
        public List<string> ContributingMods
        {
            get
            {
                List<string> result = (from faction in FactionGroupModel.FileList.Files
                                       select faction.ModSource.Name).Distinct().ToList();
                return result;
            }
        }
        #endregion

        #region properties for the view
        #region Button1
        public string Button1Text { get; } = "Remove";
        public Visibility Button1Visibility { get; } = Visibility.Visible;
        ICommand _Button1Command;
        public ICommand Button1Command
        {
            get
            {
                if (_Button1Command == null)
                {
                    _Button1Command = new RelayCommand<object>(param => this.RemoveSelectedPortrait(param));
                }
                return _Button1Command;
            }
        }
        #endregion

        #region Button2
        public string Button2Text { get; set; } = "Reset";
        public Visibility Button2Visibility { get; set; } = Visibility.Visible;
        ICommand _Button2Command;
        public ICommand Button2Command
        {
            get
            {
                if (_Button2Command == null)
                {
                    _Button2Command = new RelayCommand<object>(param => this.ResetPortraits());
                }
                return _Button2Command;
            }
            set => _Button2Command = value;
        }
        #endregion

        #region Button3
        public string Button3Text { get; set; }
        public Visibility Button3Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button3Command;
        public ICommand Button3Command
        {
            get => _Button3Command;
            set => _Button3Command = value;
        } 
        #endregion
        #endregion
        private void FactionGroupModel_GroupChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("LogoPath");
            NotifyPropertyChanged("ColorRGB");
            NotifyPropertyChanged("ContributingMods");
            NotifyPropertyChanged("PortraitsView");
            NotifyPropertyChanged("Portraits");
            NotifyPropertyChanged("PortraitsParameterArrayChange");
        }
        private void RemoveSelectedPortrait(object obj)
        {
            if (obj == null)
                return;
            SSPortrait portrait = obj as SSPortrait;
            PortraitsParameterArrayChange.Remove(portrait);
        }
        public void AddPortrait(object obj)
        {
            PortraitsParameterArrayChange.Add(obj as SSPortrait);
        }
        public void ResetPortraits()
        {
            PortraitsParameterArrayChange.ResetChange();
        }
    }
}
