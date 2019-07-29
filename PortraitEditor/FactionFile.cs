using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortraitEditor
{
    public class FactionFile
    {
        //public string Path { get; set; }
        public SSFileUrl Url { get; set; }
        //public string FileId { get; set; }
        public string DisplayName { get; set; }
        public string LogoPath { get; set; }
        public string ColorRGB { get; set; }
        public ObservableCollection<Portrait> Portraits { get; set; } = new ObservableCollection<Portrait>();
        public ObservableCollection<Portrait> OriginalPortraits { get; set; } = new ObservableCollection<Portrait>();


        public FactionFile() { }
        public FactionFile(SSFileUrl sSFileUrl)
        {
            Url = new SSFileUrl(sSFileUrl);
            //Regex ExtractFactionFileName = new Regex(@"(?:.*\\)(.*)(?:.faction)");
            //FileId = ExtractFactionFileName.Match(Path).Groups[1].ToString();

            dynamic FileRessource = new JavaRessourceExtractor(Url.FullUrl).JavaRessource;
            DisplayName = FileRessource.displayName;

            //string FormatedSource = relativePathSource.Replace("\\", "/");
            LogoPath = Url.CommonUrl + FileRessource.logo;

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
            var PortraitsMaleUrl = FileRessource.portraits.standard_male;

            if (PortraitsMaleUrl != null)
            {
                foreach (var url in PortraitsMaleUrl)
                {
                    Portraits.Add(new Portrait(Url.CommonUrl, (string)url, Gender.Male));
                }
            }
            var PortraitsFemaleUrl = FileRessource.portraits.standard_female;

            if (PortraitsFemaleUrl != null)
            {
                foreach (var url in PortraitsFemaleUrl)
                {
                    Portraits.Add(new Portrait(Url.CommonUrl, (string)url, Gender.Female));
                }
            }
            return;
        }

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
