using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PortraitEditor.JsonHandling;

namespace PortraitEditor.Model
{
    public class SSPortrait : IEquatable<SSPortrait>, IJsonConvertable
    {
        

        public SSMod SourceMod { get; set; }

        public SSMod UsingMod { get; set; }
        public Gender ImageGender { get; set; }
        public URLRelative Url { get; set; }
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
            this.Url= other.Url;
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
        public static bool EqualsNoGender(SSPortrait other1, SSPortrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (object.ReferenceEquals(other1.Url, other2.Url));
        }



        public JObject JsonEquivalent()
        {
            string PropertyName = ImageGender.ToPropertyName();
            if (PropertyName == null)
                PropertyName = Gender.MalePropertyName;

            JObject Result = new JObject(new JProperty(PropertyName, Url.RelativeUrl));
            return Result;
        }
    }

    class PortraitNoGenderEqualityComparer : IEqualityComparer<SSPortrait>
    {
        public bool Equals(SSPortrait other1, SSPortrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (other1.Url.FullUrl == other2.Url.FullUrl);
        }

        public int GetHashCode(SSPortrait other)
        {
            int hashProductUrl = other.Url == null ? 0 : other.Url.FullUrl.GetHashCode();
            return hashProductUrl.GetHashCode();
        }
    }
}

