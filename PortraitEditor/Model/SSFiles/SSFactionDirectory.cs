using PortraitEditor.Model.SSParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFactionDirectory : SSFileDirectory<SSFactionGroup,SSFaction>
    {
        public SSFactionDirectory()
        {
            base.FileDirectory.CollectionChanged += FileDirectory_CollectionChanged;
        }

        

        private ObservableCollection<SSPortrait> _GlobalAvailablePortrait = new ObservableCollection<SSPortrait>();
        public ObservableCollection<SSPortrait> GlobalAvailablePortrait { get => _GlobalAvailablePortrait; }
        private IEnumerable<URLRelative> GlobalAvailablePortraitUrl { get => from portrait in _GlobalAvailablePortrait select portrait.Url; }

        private void FileDirectory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count == 1)
            {
                List<SSPortrait> possibleNewPortrait = (e.NewItems[0] as SSFaction).Portraits;
                foreach (SSPortrait portrait in possibleNewPortrait)
                {
                    URLRelative EquivalentGlobal = (from url in GlobalAvailablePortraitUrl
                                                    where url.Equals(portrait.Url)
                                                    select url).FirstOrDefault();
                    if (EquivalentGlobal != null)
                        portrait.Url = EquivalentGlobal;
                    else
                        GlobalAvailablePortrait.Add(portrait);
                }
            }
            if (e.OldItems != null && e.OldItems.Count == 1)
            {
                List<SSPortrait> possibleLostPortrait = (e.OldItems[0] as SSFaction).Portraits;
                IEnumerable<SSPortrait> distinctLostPortrait = from portrait in possibleLostPortrait
                                           group portrait by portrait.Url into g
                                           select g.First();
                IEnumerable < URLRelative > RemainingAvailableUrl = base.FileDirectory.SelectMany(x => x.Portraits).Select(x => x.Url);
                foreach (SSPortrait portrait in distinctLostPortrait)
                {
                    if (!RemainingAvailableUrl.Contains(portrait.Url))
                    {
                        SSPortrait lastPortrait = (from gPortrait in GlobalAvailablePortrait
                                                   where gPortrait.Url.Equals(portrait.Url)
                                                   select gPortrait).Single();
                        GlobalAvailablePortrait.Remove(lastPortrait);
                    }
                }
            }
        }
    }
}
