using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortraitEditor
{
    public class Portrait : IEquatable<Portrait>, IComparable
    {
        public string Url { get; set; }
        public bool IsCore { get; set; }
        public string Name { get; set; }
        public string RelativePathSource { get; set; }
        public string FormatedSource { get; set; }
        public string FullUrl { get; set; }
        public string ImageGender { get; set; }

        public Portrait(string relativePathSource, string url)
        {
            Url = url;
            Regex FindCore = new Regex("starsector-core");
            Match FoundCore = FindCore.Match(relativePathSource);
            if (FoundCore.Success)
                IsCore = true;
            else
                IsCore = false;
            Regex ExtractFileName = new Regex(@"(?:.*/)(.*)(?:\.)");
            Name = ExtractFileName.Match(url).Groups[1].ToString();
            RelativePathSource = relativePathSource;
            FormatedSource = relativePathSource.Replace("\\", "/");
            FullUrl = FormatedSource + '/' + Url;
            ImageGender = Gender.Male;
        }
        public Portrait(string relativePathSource, string url, string imageGender)
            : this(relativePathSource, url)
        {
            ImageGender = imageGender;
        }

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
            return (this.Url.Equals(other.Url));
        }
        public static bool EqualsWithGender(Portrait other1, Portrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (other1.Url.Equals(other2.Url) && other1.ImageGender.Equals(other2.ImageGender));
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Portrait otherPortrait)
            {
                if (this.ImageGender == otherPortrait.ImageGender)
                {
                    return this.Name.CompareTo(otherPortrait.Name);
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
    }
}
