using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public class EnumLists : AiCollectObject, IEnumerable<EnumList>, ICollection<EnumList>
    {

        private List<EnumList> _enumLists;

        public EnumLists(AiCollectObject parent) : base(parent)
        {
            _enumLists = new List<EnumList>();
        }
        public EnumLists() : base(null)
        {
            _enumLists = new List<EnumList>();
        }
        public int Count
        {
            get
            {
                return _enumLists.Count;
            }
        }
        public IReadOnlyCollection<EnumList> List
        {
            get
            {
                return _enumLists.AsReadOnly();
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public EnumList this[int index]
        {
            get
            {
                return _enumLists[index];
            }
        }



        public EnumList Add()
        {
            EnumList list = new EnumList(this);
            _enumLists.Add(list);
            return list;
        }

        public override void ReadJson(Newtonsoft.Json.Linq.JObject obj)
        {
            base.ReadJson(obj);

            _enumLists.Clear();

            JArray lists = JArray.FromObject(obj["EnumerationLists"]);
            if (lists != null)
            {
                foreach (var liObj in lists)
                {
                    EnumList item = Add();
                    item.ReadJson((JObject)liObj);
                }
            }
        }

        public IEnumerator<EnumList> GetEnumerator()
        {
            foreach (var en in _enumLists)
                yield return en;
        }

        public EnumList ByKey(string key)
        {
            return _enumLists.FirstOrDefault(a => a.Key == key);
        }

        public EnumList ByName(string name)
        {
            return _enumLists.FirstOrDefault(a => a.Name == name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public void Add(EnumList item)
        {
            _enumLists.Add(item);
        }

        public void Clear()
        {
            _enumLists.Clear();
        }

        public bool Contains(EnumList item)
        {
            return _enumLists.Contains(item);
        }

        public void CopyTo(EnumList[] array, int arrayIndex)
        {

        }

        public bool Remove(EnumList item)
        {
            _enumLists.Remove(item);
            return true;
        }

        public DatabaseQueries GenerateDatabase(DataProviders provider, EnumLists importedLists = null)
        {
            DatabaseQueries queries = new DatabaseQueries();
            if (importedLists == null)
            {
                foreach (var list in _enumLists)
                {
                    queries.Append(list.GenerateDatabase(provider));
                }
            }
            else
            {
                EnumList importedList = null;
                //Get lists to create and update
                foreach (var list in _enumLists)
                {
                    importedList = importedLists.ByKey(list.Key);
                    if (importedList == null)
                    {
                        //Create the list
                        queries.Append(list.GenerateDatabase(provider));
                    }
                    else
                    {
                        //update the selected list table
                        //if the lists have changed then update
                        if (list.CompareTo(importedList) == 0)
                            queries.Append(list.GenerateDatabase(provider, importedList));
                    }
                }

                //Get lists to delete
                EnumList toDelete = null;
                foreach (var list in importedLists)
                {
                    toDelete = ByKey(list.Key);
                    if (toDelete == null)
                    {
                        //Call delete query generator
                        queries.Add(list.GenerateDatabaseDrop(provider));
                    }
                }
            }

            return queries;
        }

        public int Compare(EnumLists other)
        {
            foreach (var enumList in _enumLists)
            {
                var enumIn = ByKey(enumList.Key);
                if (enumIn != null)
                {
                    //compare
                    var different = enumList.Compare(enumIn) == 0;
                    if(different)
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 1;
        }

    }
}
