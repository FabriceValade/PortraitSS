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
    public class AllPortraitsViewModel : ViewModelBase
    {

        public AllPortraitsViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory)
        {
            FactionDirectory = factionDirectory;
            FactionDirectory.DirectoryChanged += FactionDirectory_DirectoryChanged;
        }

        private void FactionDirectory_DirectoryChanged(object sender, EventArgs e)
        {
            Portraits.Clear();
            ObservableCollection<SSPortrait> temp= new ObservableCollection<SSPortrait>(FactionDirectory.GroupDirectory.SelectMany(x => x.Portraits).Distinct(new PortraitNoGenderEqualityComparer()).ToList());
            foreach (SSPortrait portrait in temp)
            {
                Portraits.Add(portrait);
            }
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

        #region properties for the view
        public string Button1Text { get; set; }
        public Visibility Button1Visibility { get; set; } = Visibility.Collapsed;
        ICommand _Button1Command;
        public ICommand Button1Command
        {
            get => _Button1Command;
            set => _Button1Command=value;
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
    }
}
