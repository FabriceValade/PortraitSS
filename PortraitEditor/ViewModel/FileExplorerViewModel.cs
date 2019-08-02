using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    class FileExplorerViewModel : ViewModelBase
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
    }
}
