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
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string LogoPath { get; private set; }
        public string ColorRGB { get; private set; }

        public URLRelative FactionFileUrl { get; private set; }
        public JObject JsonFile { get; private set; }

        public Faction()
        {
           
        }

        public void ReadJson()
        {
            string ReadResult = File.ReadAllText(FactionFileUrl.FullUrl);
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},[^,}]*$", "}");
            JsonFile = JObject.Parse(result2);
            JToken DisplayNameToken;
            if (JsonFile.TryGetValue("displayName", out DisplayNameToken))
                DisplayName = JsonFile["displayName"].Value<string>();
            
        }
    }
}
