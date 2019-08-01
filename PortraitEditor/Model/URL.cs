using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model
{
    public class URL
    {
        #region Properties
        public string CommonUrl { get; set; } = null;

        public virtual string FullUrl
        {
            get
            {
                return CommonUrl;
            }
        }
        #endregion //Properties

        #region Contructors
        public URL() { }

        public URL(string url)
        {
            CommonUrl = url;
        }
        #endregion

        #region method
        public override string ToString()
        {
            return FullUrl;
        }

        public bool Exist()
        {
            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(this.FullUrl);
            if (!CoreFactionDirectory.Exists)
                return false;
            return true;
        }
        #endregion

    }

    public class URLLinked : URL
    {
        #region Properties
        public string LinkingUrl { get; set; } = null;

        public override string FullUrl
        {
            get
            {
                string result = base.CommonUrl;
                if (this.LinkingUrl != null)
                    result = Path.Combine(result, this.LinkingUrl);
                return result;
            }

        }
        #endregion //Properties

        #region Contructors
        public URLLinked() { }

        public URLLinked(string commonUrl, string linkingUrl)
        {
            base.CommonUrl = commonUrl;
            this.LinkingUrl = linkingUrl;
        }

        public URLLinked(URL url, string linkingUrl)
        {
            base.CommonUrl = url.CommonUrl;
            this.LinkingUrl = linkingUrl;
        }
        #endregion

        #region method
        #endregion
    }
    public class URLRelative : URLLinked
    {
        #region Properties
        public string RelativeUrl { get; set; } = null;

        public override string FullUrl
        {
            get
            {
                string result = base.CommonUrl;
                if (base.LinkingUrl != null)
                    result = Path.Combine(result, base.LinkingUrl);
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
            base.CommonUrl = commonUrl;
            base.LinkingUrl = linkingUrl;
            this.RelativeUrl = relativeUrl;
        }

        public URLRelative(URL url, string linkingUrl, string relativeUrl)
        {
            base.CommonUrl = url.CommonUrl;
            base.LinkingUrl = linkingUrl;
            this.RelativeUrl = relativeUrl;
        }

        public URLRelative(URLLinked urlLinked, string relativeUrl)
        {
            base.CommonUrl = urlLinked.CommonUrl;
            base.LinkingUrl = urlLinked.LinkingUrl;
            this.RelativeUrl = relativeUrl;
        }
        #endregion
    }
}
