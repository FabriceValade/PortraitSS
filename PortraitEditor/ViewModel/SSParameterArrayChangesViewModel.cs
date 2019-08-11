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
        ObservableCollection<T> ChangedList;
        ObservableCollection<T> RemovedList = new ObservableCollection<T>();
        ObservableCollection<T> AddedList = new ObservableCollection<T>();
        public ObservableCollection<T> ResultingList { get; } = new ObservableCollection<T>();
       
        public SSParameterArrayChangesViewModel(ObservableCollection<T> listToEdit)
        {
            ChangedList = listToEdit;
            //ResultingList = listToEdit;
            CalculateResultingList();
            ChangedList.CollectionChanged += ChangedList_CollectionChanged;
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
        }

        public void Remove(T obj)
        {
            if (ChangedList.Contains(obj))
            {
                RemovedList.Add(obj);
                ResultingList.Remove(obj);
            }
        }
        public void Add(T obj)
        {
            AddedList.Add(obj);
            ResultingList.Add(obj);
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
