using System;
using System.Collections.Generic;
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
        public SSFileLister<F> FileList { get; } = new SSFileLister<F>();
        public List<string> AvailableLink { get; set; }
        #endregion

        #region constructor
        public SSFileGroup(F newFile, List<string> availableLink)
        {
            AvailableLink = availableLink;
            newFile.OwningGroup = this;
            this.FileName = newFile.FileName ?? throw new ArgumentNullException("The FileName cannot be null.");
            FileList.Files.Add(newFile);
        } 
        #endregion



        public virtual void  Add(F newFile)
        {
            if (newFile.FileName != this.FileName)
                throw (new InvalidOperationException("Cannot group file with differing filename"));
            newFile.OwningGroup = this;
            FileList.Files.Add(newFile);
            return;
        }

        

        public void DeleteGroup()
        {
            this.FileList.Delete();
            base.Delete();
        }
    }
}
