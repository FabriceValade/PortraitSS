﻿using Ookii.Dialogs.Wpf;
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

        #region Properties

        #endregion

        //#region method
        public void ShowDialog()
        {
            //DirectoryViewModel.PurgeMod(LocalMod);
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
            SSPortrait portrait = obj as SSPortrait;
            SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            SelectedGroup.BufferPortraitRemove(obj as SSPortrait);
        }
        private void ResetPortraitFromGroup()
        {
            SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            SelectedGroup.BufferReset();
        }
        public void AddPortraitToGroupFromGeneral(object obj, Gender.GenderValue newGender)
        {
            SSFactionGroup SelectedGroup = View.FactionGroupCollectionView.ViewModel.HeldView.CurrentItem as SSFactionGroup;
            if (obj == null)
                return;
            SSPortrait portrait = new SSPortrait(obj as SSPortrait);
            portrait.ImageGender = new Gender() { Value = newGender };
            portrait.UsingMod = LocalMod;
            SelectedGroup.BufferPortraitAdd(portrait);
        }
        #endregion


    }
}
