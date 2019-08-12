using PortraitEditor.Model.SSFiles;
using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSPortraitExplorerViewModel
    {
        PortraitExplorerWindow View;

        public SSPortraitExplorerViewModel(SSFileDirectory<SSFactionGroup, SSFaction> factionDirectory)
        {
            DirectoryViewModel = new FactionDirectoryViewModel(factionDirectory);
            GeneralPortraitsViewModel = new AllPortraitsViewModel(factionDirectory);
        }

        #region Properties
        public FactionDirectoryViewModel DirectoryViewModel { get; set; }

        public AllPortraitsViewModel GeneralPortraitsViewModel { get; set; }
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
    }
}
