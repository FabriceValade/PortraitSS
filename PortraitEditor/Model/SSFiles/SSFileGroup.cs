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
        protected virtual void OnGroupChanged(EventArgs e)
        {
            GroupChanged?.Invoke(this, e);
        } 
        #endregion

        #region properties
        public string FileName { get; set; }
        public List<F> GroupFileList { get; set; } = new List<F>();
        public SSFileGroup(URL url, string modSource)
        {
            this.Add(url, modSource);
        } 
        #endregion

        public F BaseAdd(URL url, string modSource)
        {
            F newfile = Activator.CreateInstance(typeof(F), new Object[] {this,url,modSource }) as F;
            GroupFileList.Add(newfile);
            if (FileName == null)
                FileName = newfile.FileName;
            else if (newfile.FileName != FileName)
                throw new NotSupportedException("Cannot add to group with wrongly named file");
            return newfile;
        }

        public virtual  F Add(URL url, string modSource)
        {
            
            return BaseAdd(url, modSource); 
        }
    }
}
