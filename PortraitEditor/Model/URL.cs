using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model
{
    public class URLRelative
    {
        #region Properties
        public string CommonUrl { get; set; } = null;
        public string LinkingUrl { get; set; } = null;
        public string RelativeUrl { get; set; } = null;

        public string FullUrl
        {
            get
            {
                if (this.CommonUrl == null)
                    return null;
                string result = this.CommonUrl;
                if (this.LinkingUrl != null)
                    result = Path.Combine(result, this.LinkingUrl);
                if (this.RelativeUrl != null)
                    result = Path.Combine(result, this.RelativeUrl);
                return result;
            }
        }


        #endregion //Properties

        #region Contructors
        public URLRelative() { }

        public URLRelative(string commonUrl, string linkingUrl, string relativeUrl)
        {
            this.CommonUrl = commonUrl;
            this.LinkingUrl = linkingUrl;
            this.RelativeUrl = relativeUrl;
        }

        #endregion

        #region method
        public bool Exist()
        {
            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(this.FullUrl);
            if (!CoreFactionDirectory.Exists)
                return false;
            return true;
        }

        public override string ToString()
        {
            return FullUrl;
        }
        #endregion
    }
}
