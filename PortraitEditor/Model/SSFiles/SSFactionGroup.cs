using PortraitEditor.Model.SSParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSFiles
{
    public class SSFactionGroup : SSFileGroup<SSFaction>
    {


        #region Properties of this kind of group
        string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName == null)
                    return base.FileName;
                return _DisplayName;
            }
            private set => _DisplayName = value;
        }

        string _LogoPath;
        public string LogoPath
        {
            get
            { if (_LogoPath == null)
                    _LogoPath =  Properties.Settings.Default.FallbackLogo;
                return _LogoPath;
            }
            private set => _LogoPath = value;
        }

        string _ColorRGB;
        public string ColorRGB
        {
            get
            {
                if (_ColorRGB == null)
                    _ColorRGB = "#FFFFFFFF";
                return _ColorRGB;
            }
            private set => _ColorRGB = value;
        }

        public bool IsIncomplete
        {
            get

            {
                if (DisplayName == FileName)
                    return true;
                return false;
            }
        }

        #region Portrait properties
        public SSParameterArrayChanges<SSPortrait> _PortraitHandler;
        public ObservableCollection<SSPortrait> FilePortraits
        {
            get => _PortraitHandler.ChangedList;
        }
        public ObservableCollection<SSPortrait> Portraits
        {
            get => _PortraitHandler.ResultingList;
        }
        public ObservableCollection<SSPortrait> BufferAddedPortraits { get => _PortraitHandler.AddedList; }
        public ObservableCollection<SSPortrait> BufferRemovedPortraits { get => _PortraitHandler.RemovedList; }
        #endregion

        #endregion

        #region Constructors
        public SSFactionGroup() : base()
        {
            FileList.CollectionChanged += Files_CollectionChanged;
            _PortraitHandler = new SSParameterArrayChanges<SSPortrait>() { EqualityComparer = new PortraitGenderEqualityComparer() };
        }
        public SSFactionGroup(SSFaction newFile, List<string> availableLink) : base(newFile, availableLink)
        {
            FileList.CollectionChanged += Files_CollectionChanged;
            _PortraitHandler = new SSParameterArrayChanges<SSPortrait>() { EqualityComparer = new PortraitGenderEqualityComparer() };
            SynchroniseGroup();
        }

        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SynchroniseGroup();
        }
        #endregion

        #region Overiden method
        public override void Add(SSFaction newFile)
        {
            base.Add(newFile);
            //synchronisation
            SynchroniseGroup();
            OnGroupChanged();
            return;
        }
        #endregion
        #region SSFileLister _PortraitHandler exposed method and Properties notified by it
        public bool BufferedPortraits { get => _PortraitHandler.IsChanged; }
        public void BufferPortraitRemove(SSPortrait portrait)
        {
            _PortraitHandler.Remove(portrait);
        }
        public void BufferPortraitAdd(SSPortrait portrait)
        {
            _PortraitHandler.Add(portrait);
        }
        public void BufferReset()
        {
            _PortraitHandler.ResetChange();
        }
        #endregion
        #region method
        public void SynchroniseGroup()
        {
            SynchroniseParameter("DisplayName", "DisplayName");
            SynchroniseParameter("LogoPath", "LogoPath");
            SynchroniseParameter("ColorRGB", "ColorRGB");
            AgregateParameterArray<SSPortrait>("Portraits", "FilePortraits");
        }
        public void SynchroniseParameter(string factionParameterName, string thisParameterName)
        {
            SSFaction CoreModFile = (from file in base.FileList
                                     where file.ModSource.Name == PortraitEditorConfiguration.CoreModName
                                     select file).SingleOrDefault();
            string CoreProperty = null;
            if (CoreModFile != null)
                CoreProperty = CoreModFile.GetType().GetProperty(factionParameterName).GetValue(CoreModFile) as string;

            List<SSFaction> ContributingModsFaction = (from file in base.FileList
                                                       where file.ModSource.Name != PortraitEditorConfiguration.CoreModName && file.GetType().GetProperty(factionParameterName).GetValue(file) != null
                                                       select file).ToList<SSFaction>();
            string ModProperty = null;
            if (ContributingModsFaction.Count() > 0)
            {
                List<string> modsName = (from faction in ContributingModsFaction
                                         select faction.ModSource.Name).ToList<string>();
                modsName.Sort();
                string winningmodname = modsName.Last();
                ModProperty = (from faction in ContributingModsFaction
                               where faction.ModSource.Name == winningmodname
                               select faction.GetType().GetProperty(factionParameterName).GetValue(faction) as string).SingleOrDefault();
            }
            PropertyInfo PropertyInfoThis = this.GetType().GetProperty(thisParameterName);
            if (CoreProperty == null && ModProperty == null)
            { return; }
            else
            {
                if (ModProperty != null)
                {
                    PropertyInfoThis.SetValue(this, ModProperty);
                    return;
                }
                if (CoreProperty != null)
                {
                    PropertyInfoThis.SetValue(this, CoreProperty);
                    return;
                }
            }
        }
        public void AgregateParameterArray<T>(string factionParameterName, string thisParameterName)
        {
            List<T> AgregatedList = (List<T>)base.FileList.SelectMany(x => x.GetType().GetProperty(factionParameterName).GetValue(x) as List<T>).ToList();
            ObservableCollection<T> goalList = (ObservableCollection<T>)this.GetType().GetProperty(thisParameterName).GetValue(this);
            IEnumerable<T> NewObjs = from obj in AgregatedList
                                     where goalList.Contains(obj) != true
                                     select obj;
            //goalList.Clear();
            foreach (T obj in NewObjs)
            {
                goalList.Add(obj);
            }
            List<T> DisapearedObjs = (from obj in goalList
                                      where AgregatedList.Contains(obj) != true
                                      select obj).ToList();
            foreach (T obj in DisapearedObjs.ToList())
            {
                goalList.Remove(obj);
            }
        }

        public void TakeFileToModif(SSFaction file)
        {
            if (!base.FileList.Contains(file))
                return;
            foreach (SSPortrait portrait in file.Portraits)
            {
                this._PortraitHandler.Add(portrait);
            }
            file.Delete();
        }
        #endregion
    }
}
