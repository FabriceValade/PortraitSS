using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFileLister<F> where F:SSFile
    {

        public ObservableCollection<F> _Files;
        public ObservableCollection<F> Files
        {
            get
            {
                if (_Files == null)
                {
                    _Files = new ObservableCollection<F>();
                    _Files.CollectionChanged += this.OnFilesChanged;
                }
                return _Files;
            }
        }

        void OnFilesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (F file in e.NewItems)
                    file.RequestClose += this.OnFileRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (F file in e.OldItems)
                    file.RequestClose -= this.OnFileRequestClose;
        }

        void OnFileRequestClose(object sender, EventArgs e)
        {
            F file = sender as F;
            this.Files.Remove(file);
        }
    }
}
