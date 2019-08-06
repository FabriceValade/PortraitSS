using System;
using System.Collections.Generic;
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
        public List<F> GroupFileList { get; set; } = new List<F>();
        public SSFileGroup(F newFile)
        {
            newFile.OwningGroup = this;
            this.FileName = newFile.FileName ?? throw new ArgumentNullException("The FileName cannot be null.");
            GroupFileList.Add(newFile);
        } 
        #endregion



        public virtual void  Add(F newFile)
        {
            if (newFile.FileName != this.FileName)
                throw (new InvalidOperationException("Cannot group file with differing filename"));
            newFile.OwningGroup = this;
            GroupFileList.Add(newFile);
            return;
        }
    }
}
