using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortraitEditor.JsonHandling;
using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSFileWritterViewModel : ViewModelBase
    {
        #region Constructor
        public SSFileWritterViewModel(SSMod l_PeSSMod, ObservableCollection<SSFactionGroupViewModel> factionGroupList)
        {
            L_PeSSMod = l_PeSSMod;
            FactionGroupList = factionGroupList;
        } 
        #endregion


        #region field
        FileWriterWindow WindowView;
        ObservableCollection<SSFactionGroupViewModel> _ModifiedFactionList;
        string FactionFolderPath;
        #endregion


        #region properties
        public SSMod L_PeSSMod { get; set; }
        public ObservableCollection<SSFactionGroupViewModel> FactionGroupList { get; set; }
        public ObservableCollection<SSFactionGroupViewModel> ModifiedFactionList
        {
            get
            {
                if (_ModifiedFactionList == null)
                    _ModifiedFactionList = new ObservableCollection<SSFactionGroupViewModel>(from faction in FactionGroupList
                                                                                             where faction.PortraitsParameterArrayChange.IsChanged
                                                                                             select faction);
                return _ModifiedFactionList;
            }
        }
        #endregion


        #region Show Associated view
        public void ShowDialog()
        {
            ObservableCollection<SSFactionGroupViewModel> TempList = new ObservableCollection<SSFactionGroupViewModel>(from faction in FactionGroupList
                                                                                                                       where faction.PortraitsParameterArrayChange.IsChanged
                                                                                                                       select faction);
            ModifiedFactionList.Clear();
            foreach (SSFactionGroupViewModel vm in TempList)
            {
                ModifiedFactionList.Add(vm);
            }
            WindowView = new FileWriterWindow(this);
            WindowView.ShowDialog();
            return;
        }
        #endregion

        #region Commands
        RelayCommand<object> _AppendFilesCommand;
        public ICommand AppendFilesCommand
        {
            get
            {
                if (_AppendFilesCommand == null)
                {
                    _AppendFilesCommand = new RelayCommand<object>(param => this.WriteAppend());
                }
                return _AppendFilesCommand;
            }
        }
        #endregion
        public void ClearModFolder()
        {
            DirectoryInfo LPeSSFactionDirectory = new DirectoryInfo(L_PeSSMod.Url.FullUrl);
            if (LPeSSFactionDirectory.Exists)
                LPeSSFactionDirectory.Delete(true);
            LPeSSFactionDirectory.Create();
            FactionFolderPath = Path.Combine(new string[4] { L_PeSSMod.Url.FullUrl, "data", "world", "factions" });
            DirectoryInfo FactionFolderInfo = new DirectoryInfo(FactionFolderPath);
            FactionFolderInfo.Create();
        }
        public void WriteAppend()
        {
            ClearModFolder();
            foreach (SSFactionGroupViewModel vm in ModifiedFactionList)
            {
                JObject AppendPortrait = vm.AddedPortraits.FlattenToJson();
                using (StreamWriter file = File.CreateText(Path.Combine(FactionFolderPath, vm.FactionGroupModel.FileName)))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(file))
                    {
                        writer.Formatting = Formatting.Indented;
                        AppendPortrait.WriteTo(writer);
                    }
                }
            }
        }

    }
    public class CollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is ICollection))
                return Visibility.Collapsed;
            if ((value as ICollection).Count > 0)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
