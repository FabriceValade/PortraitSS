using PortraitEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class SSParameterArrayChangesViewModel<T> : ViewModelBase where T : IEquatable<T>
    {
        IEqualityComparer<T> EqualityComparer;
        ObservableCollection<T> ChangedList;
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
        
        public SSParameterArrayChangesViewModel(ObservableCollection<T> listToEdit)
        {
            ChangedList = listToEdit;
            //ResultingList = listToEdit;
            CalculateResultingList();
            ChangedList.CollectionChanged += ChangedList_CollectionChanged;
        }
        public SSParameterArrayChangesViewModel(ObservableCollection<T> listToEdit, IEqualityComparer<T> equalityComparer)
            : this(listToEdit)
        {
            EqualityComparer = equalityComparer;
        }
        private void ChangedList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (T obj in e.NewItems)
                {
                    if (AddedList.Contains<T>(obj))
                    {
                        List<int> positions=AddedList.FindAll(obj);
                        T FoundObj = AddedList.ElementAt(positions[0]);
                        AddedList.Remove(FoundObj);
                    }else
                    {

                        ResultingList.Add(obj);
                    }
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (T obj in e.OldItems)
                {
                    if (RemovedList.Contains<T>(obj))
                    {
                        List<int> positions = RemovedList.FindAll(obj);
                        T FoundObj = RemovedList.ElementAt(positions[0]);
                        RemovedList.Remove(FoundObj);
                    }
                    else
                    {
                        List<int> positions = ResultingList.FindAll(obj);
                        T FoundObj = ResultingList.ElementAt(positions[0]);
                        ResultingList.Remove(FoundObj);
                    }
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
            T OldChangedObj;
            T OldAddedObj;
            if (EqualityComparer != null)
            {
                OldChangedObj = (from changed in ChangedList
                                 where EqualityComparer.Equals(changed, obj)
                                 select changed).FirstOrDefault();
                OldAddedObj = (from added in AddedList
                                 where EqualityComparer.Equals(added, obj)
                                 select added).FirstOrDefault();
            }
            else
            {
                OldChangedObj = (from changed in ChangedList
                                 where changed.Equals(obj)
                                 select changed).FirstOrDefault();
                OldAddedObj = (from added in AddedList
                               where added.Equals(obj)
                               select added).FirstOrDefault();
            }
            
            if (OldAddedObj!=null)
            {
                AddedList.Remove(OldAddedObj);
                ResultingList.Remove(OldAddedObj);
                
            }
            else if (OldChangedObj!=null)
            {
                RemovedList.Add(OldChangedObj);
                ResultingList.Remove(OldChangedObj);
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
