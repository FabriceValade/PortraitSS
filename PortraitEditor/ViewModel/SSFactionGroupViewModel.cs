using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class SSFactionGroupViewModel
    {
        public SSFactionGroup FactionGroupModel { get; }
        public SSFactionGroupViewModel(SSFactionGroup factionGroupModel)
        {
            FactionGroupModel = factionGroupModel;
        }

        #region Properties of the group
        string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName != null)
                    return _DisplayName;
                _DisplayName = FactionGroupModel.DisplayName;
                return _DisplayName;

            }
        }
        string _LogoPath;
        public string LogoPath
        {
            get
            {
                if (_LogoPath != null)
                    return _LogoPath;
                _LogoPath = FactionGroupModel.LogoPath;
                return _LogoPath;

            }
        }
        string _ColorRGB;
        public string ColorRGB
        {
            get
            {
                if (_ColorRGB != null)
                    return _ColorRGB;
                _ColorRGB = FactionGroupModel.ColorRGB;
                if (_ColorRGB == null)
                    _ColorRGB = "#FFFFFFFF";
                return _ColorRGB;

            }
        }
        #endregion
    }
}
