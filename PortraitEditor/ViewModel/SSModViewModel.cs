using PortraitEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class SSModViewModel : ViewModelBase
    {
        public SSModViewModel()
        {
            _ModModel.FileList.CollectionChanged += FileList_CollectionChanged;
        }
        SSMod _ModModel = new SSMod();
        public SSMod ModModel
        {
            get => _ModModel;
            set
            {
                _ModModel.FileList.CollectionChanged -= FileList_CollectionChanged;
                _ModModel = value;
                _ModModel.FileList.CollectionChanged += FileList_CollectionChanged;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Name");
                NotifyPropertyChanged("Url");
            }
        }

        private void FileList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FactionCount");
        }

        public string Name
        {
            get => ModModel.Name;
        }

        public URLRelative Url
        {
            get => ModModel.Url;
        }

        bool _AllowExplore = true;
        public bool AllowExplore
        {
            get => _AllowExplore;
            set { _AllowExplore = value; NotifyPropertyChanged(); }
        }

        
        public int FactionCount
        {
            get => ModModel.FileList.Count();

        }
    }
}
