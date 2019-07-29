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
    public class SSFileUrl  : INotifyPropertyChanged
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
                RelativeUrl = Path.GetFileName(DiminishingUrl) + RelativeUrl;
                DiminishingUrl = DiminishedUrl;
            }
            throw new InvalidDataException();
        }
        //properties
        public event PropertyChangedEventHandler PropertyChanged;
        public string FullUrl { get { return this.FullUrl; } private set { this.FullUrl = value; NotifyPropertyChanged(); } }
        public string RelativeUrl { get { return this.RelativeUrl; } private set { this.RelativeUrl = value; NotifyPropertyChanged(); } }
        public string CommonUrl { get { return this.CommonUrl; } private set { this.CommonUrl = value; NotifyPropertyChanged(); } }
        public string FileName { get { return Path.GetFileName(this.FullUrl); } }

        //constructors
        public SSFileUrl(string url)
        {
            FullUrl = url;
            RelativeUrl = null;
            CommonUrl = url;
        }
        public SSFileUrl(string sSUrl,string relativeUrl)
        {
            FullUrl = Path.Combine(sSUrl, relativeUrl);
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
        public bool IsRelative()
        {
            if (RelativeUrl == null)
                return false;
            else
                return true;
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
        public override string ToString()
        {
            if (RelativeUrl != null)
                return RelativeUrl;
            else
                return FullUrl;
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
