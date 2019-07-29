using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor
{
    public class SSFileUrl  : INotifyPropertyChanged, IEquatable<SSFileUrl>
    {
        //staticusefull
        public static string ExtractRelativeUrl(string CommonUrl, string FullUrl)
        {
            string DiminishingUrl = string.Copy(FullUrl);
            string RelativeUrl = null;
            while (DiminishingUrl != null)
            {
                if (DiminishingUrl == CommonUrl)
                    return RelativeUrl;
                string DiminishedUrl = Path.GetDirectoryName(DiminishingUrl);
                if (RelativeUrl == null)
                    RelativeUrl = Path.GetFileName(DiminishingUrl);
                else
                    RelativeUrl = Path.Combine(Path.GetFileName(DiminishingUrl), RelativeUrl);

                DiminishingUrl = DiminishedUrl;
            }
            throw new InvalidDataException();
        }
        //properties
        public event PropertyChangedEventHandler PropertyChanged;
        private string _FullUrl;
        private string _RelativeUrl;
        private string _CommonUrl;

        public string FullUrl { get { return this._FullUrl; } private set { this._FullUrl = value; NotifyPropertyChanged(); } }       
        public string RelativeUrl { get { return this._RelativeUrl; } private set { this._RelativeUrl = value; NotifyPropertyChanged(); } }
        public string CommonUrl { get { return this._CommonUrl; } private set { this._CommonUrl = value; NotifyPropertyChanged(); } }
        public string FileName { get { return Path.GetFileName(this.FullUrl); } }
        public bool IsRelative { get { if (RelativeUrl == null) return false; else return true; } }

        //constructors
        public SSFileUrl(string url)
        {
            FullUrl = url;
            RelativeUrl = null;
            CommonUrl = url;
        }
        public SSFileUrl(string sSUrl,string relativeUrl)
        {
            string Tempurl= Path.Combine(sSUrl, relativeUrl);
            FullUrl = Tempurl;
            RelativeUrl = relativeUrl;
            CommonUrl = sSUrl;
        }
        public SSFileUrl(SSFileUrl other)
        {
            FullUrl = string.Copy(other.FullUrl);
            RelativeUrl = string.Copy(other.RelativeUrl);
            CommonUrl = string.Copy(other.CommonUrl);
        }

        //method
        public bool IsAvailable()
        {
            if (File.Exists(FullUrl))
                return true;
            else
                return false;
        }
        public void ChangeUrl(string newUrl)
        {
            FullUrl = newUrl;
            RelativeUrl = null;
            CommonUrl = CommonUrl;
        }
        public void ChangeUrl(string sSUrl,string relativeUrl)
        {
            FullUrl = Path.Combine(sSUrl, relativeUrl);
            RelativeUrl = relativeUrl;
            CommonUrl = sSUrl;
        }

        //interface and overide methods
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public override string ToString()
        {
            if (RelativeUrl != null)
                return RelativeUrl;
            else
                return FullUrl;
        }
        public bool Equals(SSFileUrl other)
        {
            return (this.FullUrl.Equals(other.FullUrl) && this.RelativeUrl.Equals(other.RelativeUrl));
        }
    }
}
