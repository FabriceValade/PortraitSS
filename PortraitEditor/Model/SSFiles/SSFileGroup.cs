using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFileGroup<F> : SSFileBase where F:SSFile
    {
        #region Event
        public event EventHandler GroupChanged;
        protected virtual void OnGroupChanged()
        {
            GroupChanged?.Invoke(this, null);
        } 
        #endregion

        #region properties
        public string FileName { get; set; }
        private SSFileLister<F> FileLister { get; } = new SSFileLister<F>();
        public ObservableCollection<F> FileList
        {
            get => FileLister.Files;
        }
        public List<string> AvailableLink { get; set; } = new List<string>();
        #endregion

        #region constructor
        public SSFileGroup() { }
        public SSFileGroup(F newFile, List<string> availableLink)
        {
            AvailableLink = availableLink;
            newFile.OwningGroup = this;
            this.FileName = newFile.FileName ?? throw new ArgumentNullException("The FileName cannot be null.");
            FileList.Add(newFile);
        } 
        #endregion



        public virtual void  Add(F newFile)
        {
            if (newFile.FileName != this.FileName)
                throw (new InvalidOperationException("Cannot group file with differing filename"));
            newFile.OwningGroup = this;
            FileList.Add(newFile);
            return;
        }

        

        public override void Delete()
        {
            this.FileLister.Delete();
            OnRequestClose();
        }
    }
}
