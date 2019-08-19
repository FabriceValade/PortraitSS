using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public abstract class SSFileBase
    {
        #region event
        public event EventHandler RequestClose;

        protected void OnRequestClose()
        {
            //OwningGroup = null;
            this.RequestClose?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        public SSFileBase OwningGroup { get; set; }

        public SSFileBase()
        { 

        }
        public virtual void Delete()
        {
            OnRequestClose();
        }
    }
}
