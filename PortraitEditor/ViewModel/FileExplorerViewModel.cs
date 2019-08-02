using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class FileExplorerViewModel : ViewModelBase
    {
        EditableURLViewModel _EditableURLViewModel;
        public EditableURLViewModel EditableURLViewModel
        {
            get
            {
                if (_EditableURLViewModel == null)
                    _EditableURLViewModel = new EditableURLViewModel("Starsector folder", "Select path");
                return _EditableURLViewModel;
            }
        }
        public FileExplorerViewModel() { }

        public void ShowDialog()
        {
            FileExplorerWindow NewWindow = new FileExplorerWindow(this);
            NewWindow.ShowDialog();
        }
    }
}
