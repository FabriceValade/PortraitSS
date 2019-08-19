using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Newtonsoft.Json.Linq;
using PortraitEditor.JsonHandling;
using PortraitEditor.Model.SSParameters.Interfaces;

namespace PortraitEditor.Model.SSParameters
{
    public class SSPortrait : IEquatable<SSPortrait>, IJsonConvertable, INotifyPropertyChanged, ISSExternal
    {


        public SSMod SourceMod { get; set; }

        public SSMod UsingMod { get; set; }
        public Gender ImageGender { get; set; }
        public string GenderString { get => ImageGender.ToString(); }
        public URLRelative Url { get; set; }
        public string FullUrl { get => Url.FullUrl; }
        public bool ImageFound
        {
            get
            {
                FileInfo fileInfo = new FileInfo(Url.FullUrl);
                if (!fileInfo.Exists)
                    return false;
                return true;
            }
        }

        //constructors
        public SSPortrait(URLRelative url)
        {

            Url = url;
        }
        public SSPortrait(URLRelative url, Gender gender, SSMod sourcemod, SSMod usingmod)
            : this(url)
        {
            SourceMod = sourcemod;
            ImageGender = new Gender(gender);
            UsingMod = usingmod;
        }

        public SSPortrait(SSPortrait other)
        {
            this.SourceMod = other.SourceMod;
            this.UsingMod = other.UsingMod;
            this.Url = other.Url;
        }



        //methods
        public override bool Equals(object other)
        {
            if (other == null) return false;
            SSPortrait objAsPortrait = other as SSPortrait;
            if (objAsPortrait == null) return false;
            else return Equals(objAsPortrait);
        }
        public bool Equals(SSPortrait other)
        {
            if (other == null) return false;
            bool ReferenceEqual = object.ReferenceEquals(this.Url, other.Url);
            bool GenderEqual = this.ImageGender == other.ImageGender;
            return (ReferenceEqual && GenderEqual);
        }




        public JObject JsonEquivalent()
        {
            string PropertyName = ImageGender.ToPropertyName();
            if (PropertyName == null)
                PropertyName = Gender.MalePropertyName;

            JObject Result = new JObject(
                                   new JProperty("portraits",
                                        new JObject(
                                                new JProperty(PropertyName,new JArray( Url.RelativeUrl)))));
            return Result;
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion

        public override string ToString()
        {
            return this.Url.FullUrl;
        }
    }

    class PortraitNoGenderEqualityComparer : EqualityComparer<SSPortrait>
    {
        public override bool Equals(SSPortrait other1, SSPortrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (other1.Url.FullUrl == other2.Url.FullUrl);
        }

        public override int GetHashCode(SSPortrait other)
        {
            int hashProductUrl = other.Url == null ? 0 : other.Url.FullUrl.GetHashCode();
            return hashProductUrl.GetHashCode();
        }
    }
    class PortraitGenderEqualityComparer : EqualityComparer<SSPortrait>
    {
        public override bool Equals(SSPortrait other1, SSPortrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (other1.Url.FullUrl == other2.Url.FullUrl && other1.ImageGender.Value == other2.ImageGender.Value);
        }

        public override int GetHashCode(SSPortrait other)
        {
            int hashProductUrl = other.Url == null ? 0 : other.Url.FullUrl.GetHashCode();
            return hashProductUrl.GetHashCode();
        }
    }

    class PortraitModToGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is SSMod))
                return null;
            return (value as SSMod).Name;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SSMod(new URLRelative(), "NotMod");
        }
    }
}

