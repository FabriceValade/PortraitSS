using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public SSFaction( URL url, SSMod modsource) : base( url, modsource)
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
                LogoPath = Path.Combine(base.ModSource.Url.FullUrl,LogoToken.Value<string>());

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
                    return FileName;
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

        public bool IsIncomplete
        {
            get
            {
                if (LogoPath == "Logo not set" || DisplayName == FileName)
                    return true;
                return false;
            }
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
            //DisplayName = (from file in base.GroupFileList
            //               where file.DisplayName!=null
            //               select file.DisplayName).Distinct().SingleOrDefault();
            SynchroniseParameter("DisplayName");
            LogoPath = (from file in base.GroupFileList
                        where file.LogoPath != null
                        select file.LogoPath).Distinct().SingleOrDefault();
            ColorRGB = (from file in base.GroupFileList
                        where file.ColorRGB != null
                        select file.ColorRGB).Distinct().SingleOrDefault();

        }
        public void SynchroniseParameter(string ParameterName)
        {
            SSFaction CoreModFile = (from file in base.GroupFileList
                                     where file.ModSource.Name == PortraitEditorConfiguration.CoreModName
                                     select file).SingleOrDefault();
            string CoreProperty=null;
            if (CoreModFile != null)
                CoreProperty = CoreModFile.GetType().GetProperty(ParameterName).GetValue(CoreModFile) as string;

            List<SSFaction> ContributingModsFaction = (from file in base.GroupFileList
                                                       where file.ModSource.Name != PortraitEditorConfiguration.CoreModName && file.GetType().GetProperty(ParameterName).GetValue(file) != null
                                                       select file).ToList<SSFaction>();
            string ModProperty=null;
            if (ContributingModsFaction.Count() > 0)
            {
                List<string> modsName = (from faction in ContributingModsFaction
                                        select faction.ModSource.Name).ToList<string>();
                modsName.Sort();
                string winningmodname = modsName.Last();
                ModProperty = (from faction in ContributingModsFaction
                               where faction.ModSource.Name== winningmodname
                               select faction.GetType().GetProperty(ParameterName).GetValue(faction) as string).SingleOrDefault();
            }
            PropertyInfo PropertyInfoThis = this.GetType().GetProperty(ParameterName);
            if (CoreProperty == null && ModProperty==null)
            { return; }
            else
            {
                if (ModProperty != null)
                {
                    PropertyInfoThis.SetValue(this, ModProperty);
                    return;
                }
                if (CoreProperty != null)
                {
                    PropertyInfoThis.SetValue(this, CoreProperty);
                    return;
                }
            }
        }
        #endregion
    }
}
