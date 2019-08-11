using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    class PortraitExplorerViewModel
    {
        PortraitExplorerWindow View;

        public PortraitExplorerViewModel() { }

        #region Properties
        public FactionDirectoryViewModel DirectoryViewModel { get; set; }

        public AllPortraitsViewModel test
        {
            get; set;
        }
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
