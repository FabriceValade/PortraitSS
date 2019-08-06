using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFileDirectory<G, F>  where G:SSFileGroup<F> where F:SSFile
    {
        public List<G> Directory { get; private set; } = new List<G>();

        public void AddFile(F newFile)
        {
            G Matching = (from fileGroup in Directory
                    where fileGroup.FileName == newFile.FileName
                    select fileGroup).SingleOrDefault();

            if (Matching != null)
                Matching.Add(newFile);
            else
            {
                G newGroup = Activator.CreateInstance(typeof(G), new Object[] { newFile }) as G;
                Directory.Add(newGroup);
            }
            return;
        }
        public void AddRange(List<F> FileList)
        {
            foreach (F newFile in FileList)
            {
                this.AddFile(newFile);
            }
        }

        public void RemoveFile(F file)
        {
            G Matching = (from fileGroup in Directory
                          where fileGroup.GroupFileList.Contains(file)
                          select fileGroup).SingleOrDefault();
            Matching.GroupFileList.Remove(file);
            if (Matching.GroupFileList.Count() == 0)
                Directory.Remove(Matching);
            return;
        }
        public void RemoveGroup(G Group)
        {
            foreach (F file in Group.GroupFileList)
            {
                file.RemoveFromMod();
            }
            Group.GroupFileList.Clear();
            Directory.Remove(Group);

        }
        public void Clear()
        {
            Directory.Clear();
        }
    }
}
