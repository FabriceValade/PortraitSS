using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortraitEditor.Model
{
    public class Faction
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string ColorRGB { get; set; }

        public URLRelative FactionFileUrl { get; set; }
        public JObject JsonFile { get; set; }

        public Faction() { }

        public void ReadJson()
        {
            string ReadResult = File.ReadAllText(FactionFileUrl.FullUrl);
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},[^,}]*$", "}");
            JsonFile = JObject.Parse(result2);
            JToken a = JsonFile["displayName"];
            Name = a.Value<string>();
            //LogoPath = Path.Combine(FactionFileUrl.CommonUrl,FactionFileUrl.LinkingUrl, JsonFile.logo);
            //LogoPath = Url.CommonUrl + FileRessource.logo;

            //ColorRGB = "#FFFFFFFF";
            //if (FileRessource.HasProperty("color"))
            //{
            //    var ColorCode = FileRessource.color;
            //    if (ColorCode.Count == 4)
            //    {
            //        List<string> ColorArray = new List<string>(4);
            //        foreach (string oneCode in ColorCode)
            //        {
            //            string oneRgb = Int32.Parse(oneCode).ToString("X2");
            //            ColorArray.Add(oneRgb);
            //        }
            //        ColorRGB = "#" + ColorArray[3] + ColorArray[0] + ColorArray[1] + ColorArray[2];
            //    }
            //}
        }
    }
}
