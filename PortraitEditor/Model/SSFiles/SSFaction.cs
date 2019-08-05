using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFaction : SSFile
    {
        string _DisplayName; public string DisplayName
        {
            get
            {
                if (_DisplayName != null)
                    return _DisplayName;

                JToken DisplayNameToken;
                if (JsonContent.TryGetValue("displayName", out DisplayNameToken))
                    _DisplayName = JsonContent["displayName"].Value<string>();

                return _DisplayName;
            }
        }
        #region constructor
        public SSFaction(SSFile owningGroup, URL url, string modsource) : base(owningGroup, url, modsource) { } 
        #endregion
    }

    public class SSFactionGroup : SSFileGroup<SSFaction>
    {
        string _DisplayName; public string DisplayName { get => _DisplayName;}
        public SSFactionGroup(URL url, string modSource) : base(url, modSource) { }

        public override SSFaction Add(URL url, string modSource)
        {
            SSFaction newfaction = base.BaseAdd(url, modSource);

            _DisplayName = (from file in base.GroupFileList
                            select file.DisplayName).Distinct().SingleOrDefault();
            return newfaction;
        }
    }
}
