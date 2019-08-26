using Ookii.Dialogs.Wpf;
using PortraitEditor.Extensions;
using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.Model.SSParameters;
using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSPortraitExplorerViewModel : ViewModelBase
    {
        PortraitExplorerWindow View;
        public SSFactionDirectory FactionDirectory { get; set; }
        public SSMod LocalMod;
        public SSPortraitExplorerViewModel(SSFactionDirectory factionDirectory, SSMod LocalMod)
        {
            LocalMod = LocalMod ?? throw new ArgumentNullException("Local mod cannot be null");
            FactionDirectory = factionDirectory;
        }
        ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand<object>(param => View.Close());
                }
                return _CloseCommand;
            }
        }

        #region Commands for portrait editing
        //command that get input from the selected factiong group are here
        ICommand _RemoveSelectedPortraitFromGroupCommand;
        public ICommand RemoveSelectedPortraitFromGroupCommand
        {
            get
            {
                if (_RemoveSelectedPortraitFromGroupCommand == null)
                    _RemoveSelectedPortraitFromGroupCommand = new RelayCommand<object>(param => this.RemoveSelectedPortraitFromGroup(param));
                return _RemoveSelectedPortraitFromGroupCommand;
            }
        }
        ICommand _ResetPortraitFromGroupCommand;
        public ICommand ResetPortraitFromGroupCommand
        {
            get
            {
                if (_ResetPortraitFromGroupCommand == null)
                    _ResetPortraitFromGroupCommand = new RelayCommand<object>(param => this.ResetPortraitFromGroup());
                return _ResetPortraitFromGroupCommand;
            }
        }
        //comands that get input from general portrait are here
        ICommand _AddPortraitToGroupFromGeneralMale;
        public ICommand AddPortraitToGroupFromGeneralMale
        {
            get
            {
                if (_AddPortraitToGroupFromGeneralMale == null)
                    _AddPortraitToGroupFromGeneralMale = new RelayCommand<object>(param => this.AddPortraitToGroupFromGeneral(param, Gender.GenderValue.Male));
                return _AddPortraitToGroupFromGeneralMale;
            }
        }
        ICommand _AddPortraitToGroupFromGeneralFemale;
        public ICommand AddPortraitToGroupFromGeneralFemale
        {
            get
            {
                if (_AddPortraitToGroupFromGeneralFemale == null)
                    _AddPortraitToGroupFromGeneralFemale = new RelayCommand<object>(param => this.AddPortraitToGroupFromGeneral(param, Gender.GenderValue.Female));
                return _AddPortraitToGroupFromGeneralFemale;
            }
        }
        ICommand _AddPortraitFromLocalCommand;
        public ICommand AddPortraitFromLocalCommand
        {
            get
            {
                if (_AddPortraitFromLocalCommand == null)
                    _AddPortraitFromLocalCommand = new RelayCommand<object>(param => AddPortraitFromLocal());
                return _AddPortraitFromLocalCommand;
            }
        }
        #endregion

        #region property group description for grouping
        public List<ComboboxContent<PropertyGroupDescription>> GroupDescriptionComboBoxGeneral { get; } = new List<ComboboxContent<PropertyGroupDescription>>
        {
            new ComboboxContent<PropertyGroupDescription>() {Content=new PropertyGroupDescription("SourceMod"), DisplayName="Group by mod adding"},
            new ComboboxContent<PropertyGroupDescription>() {Content=new PropertyGroupDescription("ImageGender",new PortraitGenderToGroupConverter()), DisplayName="Group by gender"},
            new ComboboxContent<PropertyGroupDescription>() {Content=null, DisplayName="No grouping"},
        };
        public List<ComboboxContent<PropertyGroupDescription>> GroupDescriptionComboBoxGroup { get; } = new List<ComboboxContent<PropertyGroupDescription>>
        {
            new ComboboxContent<PropertyGroupDescription>() {Content=new PropertyGroupDescription("UsingMod"), DisplayName="Group by mod adding"},
            new ComboboxContent<PropertyGroupDescription>() {Content=new PropertyGroupDescription("SourceMod"), DisplayName="Group by source of image"},
            new ComboboxContent<PropertyGroupDescription>() {Content=new PropertyGroupDescription("ImageGender",new PortraitGenderToGroupConverter()), DisplayName="Group by gender"},
            new ComboboxContent<PropertyGroupDescription>() {Content=null, DisplayName="No grouping"},
        };
        #endregion

        #region Properties
        public System.Collections.IList SelectedGroupsB { get; set; }
       public System.Collections.IList SelectedGroups
        {
            get
            {
                if (View == null)
                    return null;
                return View.FactionGroupCollectionView.ViewModel.SelectedStuff;
            }
        }
        #endregion

        //#region method
        public void ShowDialog()
        {
            //DirectoryViewModel.PurgeMod(LocalMod);
            View = null;
            View = new PortraitExplorerWindow(this);
            View.ShowDialog();
            return;
        }

        #region command backing method
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
                FactionDirectory.GlobalAvailablePortrait.Add(newPortrait);
            }
            return;
        }
        private void RemoveSelectedPortraitFromGroup(object obj)
        {
            if (obj == null)
                return;
            System.Collections.IList items = (System.Collections.IList)obj;
            var removeCollection = items.Cast<SSPortrait>().ToList();
            SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            foreach (SSPortrait portrait in removeCollection)
            {
                SelectedGroup.BufferPortraitRemove(portrait);
            }

        }
        private void ResetPortraitFromGroup()
        {
            SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            SelectedGroup.BufferReset();
        }
        public void AddPortraitToGroupFromGeneral(object obj, Gender.GenderValue newGender)
        {
            
            if (obj == null)
                return;
            System.Collections.IList items = (System.Collections.IList)obj;
            var AddCollection = items.Cast<SSPortrait>().ToList();
            //SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            var receivingGroup = SelectedGroups.Cast<SSFactionGroup>().ToList();
            foreach (SSFactionGroup selectedGroup in receivingGroup)
            {
                foreach (SSPortrait addPortrait in AddCollection)
                {
                    SSPortrait portrait = new SSPortrait(addPortrait);
                    portrait.ImageGender = new Gender() { Value = newGender };
                    portrait.UsingMod = LocalMod;
                    selectedGroup.BufferPortraitAdd(portrait);
                }
            }
        }
        #endregion


    }
}
