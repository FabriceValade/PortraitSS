using Ookii.Dialogs.Wpf;
using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSPortraitExplorerViewModel : ViewModelBase
    {
        PortraitExplorerWindow View;
        public SSMod L_PeSSMod;
        public SSPortraitExplorerViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory, SSMod l_PessMod)
        {
            L_PeSSMod = l_PessMod ?? throw new ArgumentNullException("Local mod cannot be null");
            DirectoryViewModel = new FactionDirectoryViewModel(factionDirectory);
            GeneralPortraitsViewModel = new AllPortraitsViewModel(factionDirectory, L_PeSSMod);
            GeneralPortraitsViewModel.Button1Command = new RelayCommand<object>(param => this.AddPortraitFromGeneral(param, Gender.GenderValue.Male));
            GeneralPortraitsViewModel.Button1Text = "Add Male";
            GeneralPortraitsViewModel.Button1Visibility = Visibility.Visible;
            GeneralPortraitsViewModel.Button2Command = new RelayCommand<object>(param => this.AddPortraitFromGeneral(param, Gender.GenderValue.Female));
            GeneralPortraitsViewModel.Button2Text = "Add Female";
            GeneralPortraitsViewModel.Button2Visibility = Visibility.Visible;
            PortraitEdit = DirectoryViewModel.PortraitEdit;
        }


        #region Properties
        public FactionDirectoryViewModel DirectoryViewModel { get; set; }

        public AllPortraitsViewModel GeneralPortraitsViewModel { get; set; }

        public ObservableCollection<SSParameterArrayChangesViewModel<SSPortrait>> PortraitEdit {get;}
        #endregion

        #region method
        public void ShowDialog()
        {
            View = new PortraitExplorerWindow(this);
            View.ShowDialog();
            return;
        }

        public void CloseWindow()
        {
            View.Close();
        } 
        #endregion
        public void AddPortraitFromGeneral(Object obj, Gender.GenderValue NewGender)
        {
            if (obj == null)
                return;
            SSPortrait portrait = new SSPortrait(obj as SSPortrait);
            portrait.ImageGender = new Gender() { Value = NewGender };
            portrait.UsingMod = L_PeSSMod;
            if (DirectoryViewModel.SelectedItem!=null)
                DirectoryViewModel.SelectedItem.AddPortrait(portrait);
        }

        
    }
}
