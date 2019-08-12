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

        SSParameterArrayChangesViewModel<SSPortrait> _PortraitsViewModel;
        public ObservableCollection<SSPortrait> Portraits
        {
            get
            {
                if (_PortraitsViewModel != null)
                    return _PortraitsViewModel.ResultingList;

                _PortraitsViewModel = new SSParameterArrayChangesViewModel<SSPortrait>(new ObservableCollection<SSPortrait>(FactionGroupModel.Portraits));
                return _PortraitsViewModel.ResultingList;

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

        public string Button2Text { get; set; }
        public Visibility Button2Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button2Command;
        public ICommand Button2Command
        {
            get => _Button2Command;
            set => _Button2Command = value;
        }

        #endregion
        private void FactionGroupModel_GroupChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("LogoPath");
            NotifyPropertyChanged("ColorRGB");
            NotifyPropertyChanged("ContributingMods");
            NotifyPropertyChanged("PortraitsView");
            NotifyPropertyChanged("Portraits");
        }
        private void RemoveSelectedPortrait(object obj)
        {
            SSPortrait portrait = obj as SSPortrait;
            _PortraitsViewModel.Remove(portrait);
        }
        public void AddPortrait(object obj)
        {
            _PortraitsViewModel.Add(obj as SSPortrait);
        }
    }
}
