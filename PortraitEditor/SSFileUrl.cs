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
    class SSFileUrl  : INotifyPropertyChanged
    {
        //properties
        public event PropertyChangedEventHandler PropertyChanged;
        private string FullUrl { get { return this.FullUrl; } set { this.FullUrl = value; NotifyPropertyChanged(); } }
        private string RelativeUrl { get { return this.RelativeUrl; } set { this.RelativeUrl = value; NotifyPropertyChanged(); } }

        //constructors
        public SSFileUrl(string url)
        {
            FullUrl = url;
            RelativeUrl = null;
        }
        public SSFileUrl(string SSurl,string relativeUrl)
        {
            FullUrl = Path.Combine(SSurl, relativeUrl);
            RelativeUrl = relativeUrl;
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
        }
        public void ChangeUrl(string SSurl,string relativeUrl)
        {
            FullUrl = Path.Combine(SSurl, relativeUrl);
            RelativeUrl = relativeUrl;
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
