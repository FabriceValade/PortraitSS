using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFileDirectory<G>  where G:SSFileGroup<SSFile>
    {
        public List<G> List { get; set; } = new List<G>();

        public SSFile AddFile(URL url, string modSource)
        {
            FileInfo info = new FileInfo(url.FullUrl);
            string newFileName = info.Name;
            G Matching = (from fileGroup in List
                    where fileGroup.FileName == newFileName
                    select fileGroup).FirstOrDefault();
            SSFile newFile;
            if (Matching != null)
                newFile = Matching.Add(url, modSource);
            else
            {
                G newGroup = Activator.CreateInstance(typeof(G), new Object[] { url, modSource }) as G;
                List.Add(newGroup);
                newFile = newGroup.GroupFileList.Single();
            }
            return newFile;
        }


    }
}
