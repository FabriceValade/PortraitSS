using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PortraitEditor.Model;
using System.Collections.ObjectModel;
using PortraitEditor.Model.SSParameters;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFaction : SSFile
    {
        

        #region Properties of this kind f file
        public string DisplayName { get; private set; }
        public string LogoPath { get; private set; }
        public string ColorRGB { get; private set; }
        public bool UseForGroup { get; set; } = true;
        public List<SSPortrait> Portraits { get; set; } = new List<SSPortrait>();
        #endregion

        #region constructor
        public SSFaction( URLRelative url, SSMod modsource, List<SSMod> availableMods) : base( url, modsource)

        {
            this.ParseJson(availableMods);
        }
        #endregion

        #region Helper method
        public void ParseJson(List<SSMod> availableMods)
        {
            if (JsonContent == null)
                return;
            JToken DisplayNameToken;
            if (JsonContent.TryGetValue("displayName", out DisplayNameToken))
                DisplayName = DisplayNameToken.Value<string>();

            JToken LogoToken;
            if (JsonContent.TryGetValue("logo", out LogoToken))
            {
                string path = LogoToken.Value<string>();
                SSMod LogoSourceMod = CheckModSourceOfPath(path, availableMods);
                LogoPath = Path.Combine(LogoSourceMod.Url.CommonUrl, LogoSourceMod.Url.LinkingUrl, path);
            }
                

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

            JToken portraitGendered;
            if (JsonContent.TryGetValue("portraits", out portraitGendered))
            {
                if (portraitGendered.Type == JTokenType.Object)
                {
                    List<string> subproperty = new List<string> { Gender.MalePropertyName, Gender.FemalePropertyName };
                    foreach (string sub in subproperty)
                    {
                        JToken portraitToken;
                        if((portraitGendered as JObject).TryGetValue(sub,out portraitToken))
                        {
                            List<string> paths = portraitToken.Values<string>().ToList<string>();
                            foreach (string path in paths)
                            {
                                SSMod PortraitSourceMod = CheckModSourceOfPath(path, availableMods);
                                URLRelative newPortraitUrl = new URLRelative(PortraitSourceMod.Url.CommonUrl, PortraitSourceMod.Url.LinkingUrl, path);
                                Gender newGender=new Gender();
                                if (sub == Gender.MalePropertyName)
                                    newGender.Value = Gender.GenderValue.Male;
                                else
                                    newGender.Value = Gender.GenderValue.Female;

                                Portraits.Add(new SSPortrait(newPortraitUrl,newGender,PortraitSourceMod,this.ModSource));
                            }
                        }
                    }
                }
            }


        }

        public SSMod CheckModSourceOfPath(string relativePath, List<SSMod> availableMods)
        {
            List<string> availableLink=(from mod in availableMods
                                       select mod.Url.LinkingUrl).ToList();
            List<string> PossibleLink = URLRelative.CheckFileLinkingExist(this.Url.CommonUrl, availableLink, relativePath);
            string matchingSourceLink = (from link in PossibleLink
                                         where link == base.ModSource.Url.LinkingUrl
                                         select link).SingleOrDefault();
            if (matchingSourceLink != null)
                return base.ModSource;
            SSMod PossibleMod = (from mod in availableMods
                                 where mod.Url.LinkingUrl == PossibleLink[0]
                                 select mod).FirstOrDefault();
            return PossibleMod;
        }


        #endregion

        public override string ToString()
        {
            return base.FileName;
        }
    }

    
}
