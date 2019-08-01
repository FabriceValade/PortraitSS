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
    public class SSFileUrl : INotifyPropertyChanged, IEquatable<SSFileUrl>
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
        private string _RelativeUrl;
        private string _CommonUrl;
        private string _LinkFolder;

        //returning value, target for binding
        //one way
        public string FullUrl
        {
            get
            {
                string result = CommonUrl;
                if (LinkFolder != null)
                    result = Path.Combine(result, LinkFolder);
                if (RelativeUrl != null)
                    result = Path.Combine(result, RelativeUrl);
                return result;
            }
        }
        public string LinkedUrl
        {
            get
            {
                string result = CommonUrl;
                if (LinkFolder != null)
                    result = Path.Combine(result, LinkFolder);
                return result;
            }
        }

        //two way
        public string RelativeUrl 
        {
            get { return _RelativeUrl; }
            set { _RelativeUrl = value; NotifyPropertyChanged("FullUrl"); NotifyPropertyChanged(); }
        }
        public string LinkFolder
        {
            get { return _LinkFolder; }
            set { _LinkFolder = value; NotifyPropertyChanged("FullUrl"); NotifyPropertyChanged("LinkedUrl"); NotifyPropertyChanged(); }
        }
        public string CommonUrl
        {
            get { return _CommonUrl; }
            set { _CommonUrl = value; NotifyPropertyChanged("FullUrl"); NotifyPropertyChanged("LinkedUrl"); NotifyPropertyChanged(); }
        }

        //usefull properties that should no be bound due to having no property changed
        public string FileName { get { return Path.GetFileName(this.FullUrl); } }
        public bool IsRelative { get { if (RelativeUrl == null) return false; else return true; } }

        //constructors
        public SSFileUrl()
        {
            LinkFolder = null;
            RelativeUrl = null;
            CommonUrl = null;
        }
        public SSFileUrl(string commonUrl)
        {
            
            RelativeUrl = null;
            LinkFolder = null;
            CommonUrl = commonUrl;
        }
        public SSFileUrl(string commonUrl,string linkFolder)
        {

            RelativeUrl = null;
            LinkFolder = linkFolder;
            CommonUrl = commonUrl;
        }

        public SSFileUrl(string commonUrl, string linkFolder, string relativeUrl)
        {

            RelativeUrl = relativeUrl;
            LinkFolder = linkFolder;
            CommonUrl = commonUrl;
        }
        public SSFileUrl(SSFileUrl other)
        {
            LinkFolder = string.Copy(other.LinkFolder);
            RelativeUrl = string.Copy(other.RelativeUrl);
            CommonUrl = string.Copy(other.CommonUrl);
        }
        public SSFileUrl(SSFileUrl other, string linkfolder,  string relativeFromOther)
        {
            LinkFolder = linkfolder;
            RelativeUrl = string.Copy(relativeFromOther);
            CommonUrl = string.Copy(other.FullUrl);
        }
        //method
        public bool IsAvailable()
        {
            if (File.Exists(FullUrl))
                return true;
            else
                return false;
        }

        public void Clear()
        {
            LinkFolder = null;
            RelativeUrl = null;
            CommonUrl = null;
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
                return "Url wrongly bound";
        }
        public bool Equals(SSFileUrl other)
        {
            return (this.FullUrl.Equals(other.FullUrl));
        }
        public override bool Equals(object other)
        {
            if (other == null) return false;
            SSFileUrl objAsSSFileUrl = other as SSFileUrl;
            if (objAsSSFileUrl == null) return false;
            else return Equals(objAsSSFileUrl);
        }
    }
}
