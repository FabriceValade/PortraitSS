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
        string _FileName;
        public string FileName { get => _FileName; }

        URL _Url;
        public URL Url { get=>_Url; }

        JObject _JsonContent;
        public JObject JsonContent { get=>_JsonContent;}

        string _ModSource;
        public string ModSource { get=>_ModSource; }
        #endregion

        #region Constructors
        public SSFile( URL url, string modSource)
        {
            _ModSource = modSource;
            _Url = url ?? throw new ArgumentNullException("The Url cannot be null.");
            FileInfo info = new FileInfo(Url.FullUrl);
            _FileName = info.Name ?? throw new ArgumentNullException("The FileName cannot be null.");
            string ReadResult = File.ReadAllText(Url.FullUrl);
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},[^,}]*$", "}");
            _JsonContent = JObject.Parse(result2);
        }
        #endregion
        

    }
}


