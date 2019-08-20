using PortraitEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model.SSParameters
{
    public class SSParameterArrayChanges<T> where T : IEquatable<T>
    {
        public IEqualityComparer<T> EqualityComparer { get; set; }
        public ObservableCollection<T> ChangedList { get; }
        public ObservableCollection<T> RemovedList { get; } = new ObservableCollection<T>();
        public ObservableCollection<T> AddedList { get; } = new ObservableCollection<T>();
        public ObservableCollection<T> ResultingList { get; } = new ObservableCollection<T>();
        public bool IsChanged
        {
            get
            {
                if (RemovedList.Count > 0 || AddedList.Count > 0)
                    return true;
                return false;
            }
        }
        
        public SSParameterArrayChanges()
        {
            ChangedList = new ObservableCollection<T>();
            ChangedList.CollectionChanged += ChangedList_CollectionChanged;
        }
        public SSParameterArrayChanges(ObservableCollection<T> listToEdit)
        {
            ChangedList = listToEdit;
            CalculateResultingList();
            ChangedList.CollectionChanged += ChangedList_CollectionChanged;
        }
        public SSParameterArrayChanges(ObservableCollection<T> listToEdit, IEqualityComparer<T> equalityComparer)
            : this(listToEdit)
        {
            EqualityComparer = equalityComparer;
        }
        private void ChangedList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (T obj in e.NewItems)
                {
                    T OldAddedObj;
                    OldAddedObj = (from added in AddedList
                                   where EqualityComparer.Equals(added, obj)
                                   select added).FirstOrDefault();
                    if (OldAddedObj != null)
                        AddedList.Remove(OldAddedObj);
                    else
                        ResultingList.Add(obj);
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (T obj in e.OldItems)
                {
                    T OldRemovedObj;
                    OldRemovedObj = (from removed in RemovedList
                                   where EqualityComparer.Equals(removed, obj)
                                   select removed).FirstOrDefault();
                    if (OldRemovedObj != null)
                        RemovedList.Remove(OldRemovedObj);
                    else
                        ResultingList.Remove(obj);
                }
            if (e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
               
                ResultingList.Clear();
                RemovedList.Clear();
                foreach (T obj in AddedList)
                    ResultingList.Add(obj);
            }
        }

        public void Remove(T obj)
        {
            T OldResultingObj;
            T OldAddedObj;
            if (EqualityComparer != null)
            {
                OldResultingObj = (from resulting in ResultingList
                                 where EqualityComparer.Equals(resulting, obj)
                                 select resulting).FirstOrDefault();
                OldAddedObj = (from added in AddedList
                                 where EqualityComparer.Equals(added, obj)
                                 select added).FirstOrDefault();
            }
            else
            {
                OldResultingObj = (from resulting in ResultingList
                                 where resulting.Equals(obj)
                                 select resulting).FirstOrDefault();
                OldAddedObj = (from added in AddedList
                               where added.Equals(obj)
                               select added).FirstOrDefault();
            }
            
            if (OldAddedObj!=null)
            {
                AddedList.Remove(OldAddedObj);
                ResultingList.Remove(OldAddedObj);
                
            }
            else if (OldResultingObj!=null)
            {
                RemovedList.Add(OldResultingObj);
                ResultingList.Remove(OldResultingObj);
            }
        }
        public void Add(T obj)
        {
            T OldRemovedObj;
            if (EqualityComparer != null)
            {
                OldRemovedObj = (from removed in RemovedList
                                 where EqualityComparer.Equals(removed, obj)
                                 select removed).FirstOrDefault();
            }
            else
            {
                OldRemovedObj = (from removed in RemovedList
                                 where removed.Equals(obj)
                                 select removed).FirstOrDefault();
            }
            if (OldRemovedObj!=null)
            {
                RemovedList.Remove(OldRemovedObj);
                ResultingList.Add(OldRemovedObj);
            }
            else
            {
                AddedList.Add(obj);
                ResultingList.Add(obj);
            }
        }

        public void ResetChange()
        {
            RemovedList.Clear();
            AddedList.Clear();
            CalculateResultingList();
        }
        void CalculateResultingList()
        {
            ResultingList.Clear();
            foreach (T obj in ChangedList)
            {
                ResultingList.Add(obj);
            }
            foreach (T obj in RemovedList)
            {
                ResultingList.Remove(obj);
            }
            foreach (T obj in AddedList)
            {
                ResultingList.Add(obj);
            }
        }
    }
}
