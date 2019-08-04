using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public abstract class SSFileDirectory<F>  where F:SSFileGroup<SSFile>
    {
        public List<F> List { get; set; } = new List<F>();

        public void AddFile(URL url, string Modsource)
        {
            FileInfo info = new FileInfo(url.FullUrl);
            string newFileName = info.Name;
            F Matching = (from fileGroup in List
                    where fileGroup.FileName == newFileName
                    select fileGroup).FirstOrDefault();
            SSFile newFile;
            if (Matching != null)
                newFile = Matching.Add(url, Modsource);
        }


    }
}
