using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PortraitEditor
{
    class JavaRessourceExtractor
    {
        public dynamic JavaRessource{get; set;}

        public JavaRessourceExtractor(string path)
        {

            string ReadResult = File.ReadAllText(path);

            
            var result = Regex.Replace(ReadResult, "#.*", "");
            var result2 = Regex.Replace(result, "},$", "}");

            JavaRessource = new DynamicJson(result2);
            //string lol = JavaRessource.portraits.standard_male[0];
            return;
        }
        
    }

    public class DynamicJson : DynamicObject
    {
        Dictionary<string, object> _Dict;

        public DynamicJson(string json)
        {
            _Dict = (Dictionary<string, object>)JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        DynamicJson(Dictionary<string, object> dict)
        {
            _Dict = dict;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            object obj;
            if (!_Dict.TryGetValue(binder.Name, out obj)) return false;

            if (obj is Dictionary<string, object>)
            {
                result = new DynamicJson((Dictionary<string, object>)obj);
            }
            else
            {
                result = obj;
            }
            return true;
        }
    }
}
