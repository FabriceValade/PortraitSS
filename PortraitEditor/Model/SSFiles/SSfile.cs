using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
        #region event
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            base.OwningGroup = null;
            this.RequestClose?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Properties
        string _FileName;
        public string FileName { get => _FileName; }

        URLRelative _Url;
        public URLRelative Url { get=>_Url; }

        JObject _JsonContent;
        public JObject JsonContent { get=>_JsonContent;}

        SSMod _ModSource;
        public SSMod ModSource { get=>_ModSource; }
        #endregion

        #region Constructors
        public SSFile( URLRelative url, SSMod modSource)
        {
            _ModSource = modSource;          
            _Url = url ?? throw new ArgumentNullException("The Url cannot be null.");
            if (!Url.IsComplete)
                throw new ArgumentNullException("The Url must be complete.");
            FileInfo info = new FileInfo(Url.FullUrl);
            _FileName = info.Name ?? throw new ArgumentNullException("The FileName cannot be null.");
            string ReadResult = File.ReadAllText(Url.FullUrl);
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},[^,}]*$", "}");
            //_JsonContent = JObject.Parse(result2);
            _JsonContent = JsonConvert.DeserializeObject(result2, new JsonSerializerSettings { Error = HandleDeserializationError }) as JObject;
        }
        #endregion

        public void RemoveFromMod()
        {
            if (ModSource != null)
            {
                ModSource.RemoveFile(this);
            }
        }
        public void Remove()
        {
            OnRequestClose();
        }
        public void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }

    }
}


