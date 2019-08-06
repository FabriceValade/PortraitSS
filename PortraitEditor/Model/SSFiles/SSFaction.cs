using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFaction : SSFile
    {
        #region Properties of this kind f file
        public string DisplayName { get; private set; }
        public string LogoPath { get; private set; }
        public string ColorRGB { get; private set; }
        #endregion

        #region constructor
        public SSFaction( URL url, string modsource, URL modUrl) : base( url, modsource, modUrl)
        {
            this.ParseJson();
        }
        #endregion

        #region Helper method
        public void ParseJson()
        {
            if (JsonContent == null)
                return;
            JToken DisplayNameToken;
            if (JsonContent.TryGetValue("displayName", out DisplayNameToken))
                DisplayName = DisplayNameToken.Value<string>();

            JToken LogoToken;
            if (JsonContent.TryGetValue("logo", out LogoToken))
                LogoPath = Path.Combine(base.ModUrl.FullUrl,LogoToken.Value<string>());

            ColorRGB = "#FFFFFFFF";
            JToken ColorToken;
            if (JsonContent.TryGetValue("color", out ColorToken))
            {
                List<int> ColorCode = ColorToken.Values<int>().ToList<int>();
                if (ColorCode.Count == 4)
                {
                    List<string> ColorArray = (from color in ColorCode
                                              select color.ToString("X2")).ToList<string>();
                    ColorRGB = "#" + ColorArray[3] + ColorArray[0] + ColorArray[1] + ColorArray[2];
                }
            }
            
        }
        #endregion
    }

    public class SSFactionGroup : SSFileGroup<SSFaction>
    {
        #region Properties of this kind of group
        string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName == null)
                    return "DisplayName not set";
                return _DisplayName;
            }
            private set => _DisplayName = value;
        }
        string _LogoPath;
        public string LogoPath
        {
            get
            {
                if (_LogoPath == null)
                    return "Logo not set";
                return _LogoPath;
            }
            private set =>_LogoPath=value;
        }
        string _ColorRGB;
        public string ColorRGB
        {
            get
            {
                if (_ColorRGB == null)
                    return "#FFFFFFFF";
                return _ColorRGB;
            }
            private set => _ColorRGB = value;
        }
        #endregion

        #region Constructors
        public SSFactionGroup(SSFaction newFile) : base(newFile)
        {
            SynchroniseGroup();
        }
        #endregion

        #region Overiden method
        public override void Add(SSFaction newFile)
        {
            base.Add(newFile);

            //synchronisation
            SynchroniseGroup();
            OnGroupChanged();
            return;
        }

        public void SynchroniseGroup()
        {
            DisplayName = (from file in base.GroupFileList
                           select file.DisplayName).Distinct().SingleOrDefault();
            LogoPath = (from file in base.GroupFileList
                           select file.LogoPath).Distinct().SingleOrDefault();
            ColorRGB = (from file in base.GroupFileList
                           select file.ColorRGB).Distinct().SingleOrDefault();

        }
        #endregion
    }
}
