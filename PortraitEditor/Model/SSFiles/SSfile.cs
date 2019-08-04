using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFile : SSFileBase
    {
        #region Properties
        public string FileName { get;}
        public URL Url { get; }
        public JObject JsonContent { get;}
        public string ModSource { get; }
        #endregion

        #region Constructors
        public SSFile(SSFileBase OwningGroup, URL url, string modSource) : base(OwningGroup)
        {
            ModSource = modSource;
            Url = url ?? throw new ArgumentNullException("The Url cannot be null.");
            FileInfo info = new FileInfo(Url.FullUrl);
            FileName = info.Name;
            string ReadResult = File.ReadAllText(Url.FullUrl);
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},[^,}]*$", "}");
            JsonContent = JObject.Parse(result2);
        }
        #endregion

        #region method

        #endregion
    }
}


