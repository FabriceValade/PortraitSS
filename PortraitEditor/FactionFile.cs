using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace PortraitEditor
{
    public class FactionFile
    {
        //properties
        public SSFileUrl Url { get; set; }
        public string DisplayName { get; set; }
        public string LogoPath { get; set; }
        public string ColorRGB { get; set; }
        public ObservableCollection<Portrait> Portraits { get; set; } = new ObservableCollection<Portrait>();
        public ObservableCollection<Portrait> OriginalPortraits { get; set; } = new ObservableCollection<Portrait>();
        dynamic FileRessource { get; set; }

        //constructor
        public FactionFile() { }
        public FactionFile(SSFileUrl sSFileUrl)
        {
            Url = new SSFileUrl(sSFileUrl);
            SetupFromUrl();
            ReadPortraits(new ObservableCollection<Portrait>());
            return;
        }
        public FactionFile(SSFileUrl sSFileUrl, ICollection<Portrait> alreadyReferenced)
        {
            Url = new SSFileUrl(sSFileUrl);
            SetupFromUrl();
            ReadPortraits(alreadyReferenced);
            return;
        }

        //private method
        private void SetupFromUrl()
        {
            FileRessource = new JavaRessourceExtractor(Url.FullUrl).JavaRessource;
            DisplayName = FileRessource.displayName;

            LogoPath=Path.Combine(Url.LinkedUrl, FileRessource.logo);
            //LogoPath = Url.CommonUrl + FileRessource.logo;

            ColorRGB = "#FFFFFFFF";
            if (FileRessource.HasProperty("color"))
            {
                var ColorCode = FileRessource.color;
                if (ColorCode.Count == 4)
                {
                    List<string> ColorArray = new List<string>(4);
                    foreach (string oneCode in ColorCode)
                    {
                        string oneRgb = Int32.Parse(oneCode).ToString("X2");
                        ColorArray.Add(oneRgb);
                    }
                    ColorRGB = "#" + ColorArray[3] + ColorArray[0] + ColorArray[1] + ColorArray[2];
                }
            }
            return;
        }
        private void ReadPortraits(ICollection<Portrait> alreadyReferenced)
        {
            var PortraitsMaleUrl = FileRessource.portraits.standard_male;

            if (PortraitsMaleUrl != null)
            {
                foreach (var url in PortraitsMaleUrl)
                {
                    string strUrl = (string)url;
                    //strUrl = strUrl.Replace('/','\\');
                    SSFileUrl fileUrl = new SSFileUrl(Url.CommonUrl,Url.LinkFolder, strUrl);
                    var ReferencedUrl = (from portrait in alreadyReferenced where portrait.ImageUrl.Equals(fileUrl) select portrait.ImageUrl).ToList();
                    if (ReferencedUrl.Count > 0)
                        Portraits.Add(new Portrait(ReferencedUrl.ElementAt(0), Gender.Male));
                    else
                    {
                        Portrait newPort = new Portrait(fileUrl, Gender.Male);
                        Portraits.Add(newPort);
                        alreadyReferenced.Add(newPort);
                    }
                }
            }
            var PortraitsFemaleUrl = FileRessource.portraits.standard_female;

            if (PortraitsFemaleUrl != null)
            {
                foreach (var url in PortraitsFemaleUrl)
                {
                    string strUrl = (string)url;
                    //strUrl = strUrl.Replace('/', '\\');
                    SSFileUrl fileUrl = new SSFileUrl(Url.CommonUrl, Url.LinkFolder, strUrl);
                    var ReferencedUrl = (from portrait in alreadyReferenced where portrait.ImageUrl.Equals(fileUrl) select portrait.ImageUrl).ToList();
                    if (ReferencedUrl.Count > 0)
                        Portraits.Add(new Portrait(ReferencedUrl.ElementAt(0), Gender.Female));
                    else
                    {
                        Portrait newPort = new Portrait(fileUrl, Gender.Female);
                        Portraits.Add(newPort);
                        alreadyReferenced.Add(newPort);
                    }
                }
            }
            return;
        }
        //public method
        public void SetOriginal()
        {
            foreach (Portrait p in Portraits)
            {
                Portrait Originaling = new Portrait(p);
                OriginalPortraits.Add(Originaling);
            }
        }
        public void AddPortrait(Portrait adding)
        {
            Portraits.Add(adding);
            OrderPortrait();
            return;
        }
        public void RemovePortrait(int index)
        {
            if (Portraits.Count > index)
                Portraits.RemoveAt(index);
            return;
        }
        public void OrderPortrait()
        {
            ObservableCollection<Portrait> temp;
            temp = new ObservableCollection<Portrait>(Portraits.OrderBy(Portrait => Portrait));
            Portraits.Clear();
            foreach (Portrait j in temp) Portraits.Add(j);
            return;
        }
        public ObservableCollection<Portrait> GetAppended()
        {
            ObservableCollection<Portrait> Appended = new ObservableCollection<Portrait>();
            bool[] originalUsed = new bool[OriginalPortraits.Count];
            for (int i = 0; i < originalUsed.Length; i++) originalUsed[i] = false;

            foreach (Portrait p in Portraits)
            {
                List<int> PosOriginal = OriginalPortraits.FindAll(p, Portrait.EqualsWithGender);
                int PosFound = -1;
                for (int i = 0; i < PosOriginal.Count && PosFound == -1; i++)
                {
                    if (!originalUsed[PosOriginal[i]])
                    {
                        PosFound = PosOriginal[i];
                        originalUsed[PosOriginal[i]] = true;
                    }
                }
                if (PosFound == -1)
                    Appended.Add(p);
            }

            return Appended;
        }
    }

}
