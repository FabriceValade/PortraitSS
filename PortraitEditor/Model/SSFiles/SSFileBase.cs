using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public abstract class SSFileBase
    {
        public SSFileBase OwningGroup { get; set; }

        public SSFileBase()
        { 

        }
        
    }
}
