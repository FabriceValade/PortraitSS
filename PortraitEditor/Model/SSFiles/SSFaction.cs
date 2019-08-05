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
        #region Properties of this kind f file
        string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
        }
        #endregion

        #region constructor
        public SSFaction( URL url, string modsource) : base( url, modsource)
        {
            this.ParseJson();
        }
        #endregion

        #region Helper method
        public void ParseJson()
        {
            JToken DisplayNameToken;
            if (JsonContent.TryGetValue("displayName", out DisplayNameToken))
                _DisplayName = JsonContent["displayName"].Value<string>();
        }
        #endregion
    }

    public class SSFactionGroup : SSFileGroup<SSFaction>
    {
        #region Properties of this kind of group
        string _DisplayName;
        public string DisplayName { get => _DisplayName;}
        #endregion

        #region Constructors
        public SSFactionGroup(SSFaction newFile) : base(newFile)
        {
            _DisplayName = (from file in base.GroupFileList
                            select file.DisplayName).Distinct().SingleOrDefault();
        }
        #endregion

        #region Overiden method
        public override void Add(SSFaction newFile)
        {
            base.Add(newFile);

            //synchronisation
            _DisplayName = (from file in base.GroupFileList
                            select file.DisplayName).Distinct().SingleOrDefault();
            return;
        }
        #endregion
    }
}
