using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PortraitEditor
{
    public class Portrait : IEquatable<Portrait>, IComparable, IJsonConvertable
    {
        public static string MalePropertyName = "Standard_male";
        public static string FemalePropertyName = "Standard_female";

        //properties
        //public string Url { get; set; }
        public bool IsCore { get; }
        //public string Name { get; set; }
        //public string RelativePathSource { get; set; }
        //public string FormatedSource { get; set; }
        //public string FullUrl { get; set; }
        public string ImageGender { get; set; }
        public SSFileUrl ImageUrl { get; set; }

        //constructors
        public Portrait(string Url)
        {
            ImageUrl = new SSFileUrl(Url);
            IsCore = false;
            ImageGender = Gender.Male;
        }
        public Portrait(string sSUrl, string relativeUrl)
        {
            //Url = url;
            ImageUrl = new SSFileUrl(sSUrl, relativeUrl);
            Regex FindCore = new Regex("starsector-core");
            Match FoundCore = FindCore.Match(relativeUrl);
            if (FoundCore.Success)
                IsCore = true;
            else
                IsCore = false;
            //Regex ExtractFileName = new Regex(@"(?:.*/)(.*)(?:\.)");
            //Name = ExtractFileName.Match(url).Groups[1].ToString();
            //RelativePathSource = relativePathSource;
            //FormatedSource = relativePathSource.Replace("\\", "/");
            //FullUrl = FormatedSource + '/' + Url;
            ImageGender = Gender.Male;
        }
        public Portrait(string sSUrl, string relativeUrl, string imageGender)
            : this(sSUrl,relativeUrl)
        {
            ImageGender = imageGender;
        }
        public Portrait(Portrait other)
        {
            this.ImageUrl = other.ImageUrl;
            this.IsCore = other.IsCore;
            this.ImageGender = string.Copy(other.ImageGender);
        }
        public Portrait(Portrait other, string imageGender)
            :this(other)
        {
            ImageGender = string.Copy(imageGender);
        }
        public Portrait(SSFileUrl fileUrl)
        {
            ImageUrl = fileUrl;
            IsCore = false;
            ImageGender = Gender.Male;
        }
        public Portrait(SSFileUrl fileUrl, string imageGender)
            :this(fileUrl)
        {
            ImageGender = string.Copy(imageGender);
        }


        //methods
        public void FlipGender()
        {
            if (ImageGender == Gender.Male)
                ImageGender = Gender.Female;
            else
                ImageGender = Gender.Male;
            return;
        }
        public override bool Equals(object other)
        {
            if (other == null) return false;
            Portrait objAsPortrait = other as Portrait;
            if (objAsPortrait == null) return false;
            else return Equals(objAsPortrait);
        }
        public bool Equals(Portrait other)
        {
            if (other == null) return false;
            return (object.ReferenceEquals(this.ImageUrl,other.ImageUrl));
        }
        public static bool EqualsWithGender(Portrait other1, Portrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (object.ReferenceEquals(other1.ImageUrl, other2.ImageUrl) && other1.ImageGender.Equals(other2.ImageGender));
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Portrait otherPortrait)
            {
                if (this.ImageGender == otherPortrait.ImageGender)
                {
                    return this.ImageUrl.FileName.CompareTo(otherPortrait.ImageUrl.FileName);
                }
                else
                {
                    if (this.ImageGender == Gender.Male) return -1;
                    else return 1;
                }
            }
            else
                throw new ArgumentException("Object is not a Portrait");
        }

        public JObject JsonEquivalent()
        {
            //if (ImageUrl.IsRelative() == false)
            //    return null;
            string PropertyName;
            if (ImageGender == Gender.Male)
                PropertyName = Portrait.MalePropertyName;
            else
                PropertyName = Portrait.FemalePropertyName;

            JObject Result = new JObject(new JProperty(PropertyName, ImageUrl.ToString()));
            return Result;
        }
    }
}
