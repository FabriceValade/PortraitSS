using Ookii.Dialogs.Wpf;
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
    public class AllPortraitsViewModel : ViewModelBase
    {

        public AllPortraitsViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory, SSMod localMod)
        {
            FactionDirectory = factionDirectory;
            FactionDirectory.DirectoryChanged += FactionDirectory_DirectoryChanged;
            LocalMod = localMod;
        }

        #region EventHandler
        private void FactionDirectory_DirectoryChanged(object sender, EventArgs e)
        {
            Portraits.Clear();
            ObservableCollection<SSPortrait> temp = new ObservableCollection<SSPortrait>(FactionDirectory.GroupDirectory.SelectMany(x => x.Portraits).Distinct(new PortraitNoGenderEqualityComparer()).ToList());
            foreach (SSPortrait portrait in temp)
            {
                Portraits.Add(portrait);
            }
        } 
        #endregion

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

        SSMod LocalMod;


        #region properties for the view
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
        #region Button1
        public string Button1Text { get; set; }
        public Visibility Button1Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button1Command;
        public ICommand Button1Command
        {
            get => _Button1Command;
            set => _Button1Command = value;
        }
        #endregion
        #region Button2
        public string Button2Text { get; set; }
        public Visibility Button2Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button2Command;
        public ICommand Button2Command
        {
            get => _Button2Command;
            set => _Button2Command = value;
        }
        #endregion
        #region Button3
        public string Button3Text { get; set; } = "Add from file";
        public Visibility Button3Visibility { get; set; } = Visibility.Visible;
        ICommand _Button3Command;
        public ICommand Button3Command
        {
            get
            {
                if (_Button3Command == null)
                {
                    _Button3Command = new RelayCommand<object>(param => this.AddPortraitFromLocal());
                }
                return _Button3Command;
            }
            set => _Button3Command = value;
        }
        #endregion
        #endregion

        public void AddPortraitFromLocal()
        {
            VistaOpenFileDialog FileOpen = new VistaOpenFileDialog();
            FileOpen.Multiselect = false;
            
            URLRelative newUrl;
            if (FileOpen.ShowDialog() == true)
            {
                newUrl = new URLRelative()
                {
                    LinkingUrl = null,
                    RelativeUrl = null,
                    CommonUrl = FileOpen.FileName
                };
                SSPortrait newPortrait = new SSPortrait(newUrl, new Gender(), LocalMod, null);
                Portraits.Add(newPortrait);
            }
            return;
        }
   
    }
}
