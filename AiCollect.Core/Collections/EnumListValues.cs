using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class EnumListValues : AiCollectObject, IEnumerable<EnumListValue>,ICollection<EnumListValue>
    {

        public IReadOnlyCollection<EnumListValue> List
        {
            get
            {
                return _values.AsReadOnly();
            }
        }

        private List<EnumListValue> _values;

        [Browsable(false)]
        public new EnumList Parent
        {
            get
            {
                return (EnumList)base.Parent as EnumList;
            }
        }

        public EnumListValues(AiCollectObject parent) : base(parent)
        {

            _values = new List<EnumListValue>();
            
        }


        public EnumListValues():base(null)
        {
            _values = new List<EnumListValue>();
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<EnumListValue> GetEnumerator()
        {
            foreach (var enumValue in _values)
                yield return enumValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public EnumListValue Add()
        {
            EnumListValue item = new EnumListValue(this);
            item.ObjectState = ObjectStates.Added;           
            _values.Add(item);
            return item;
        }

        public override void ReadJson(Newtonsoft.Json.Linq.JObject obj)
        {
            base.ReadJson(obj);

            _values.Clear();

            JArray listItems = JArray.FromObject(obj["EnumValues"]);
            if (listItems != null)
            {
                foreach (var liObj in listItems)
                {
                    EnumListValue item = Add();
                    item.ReadJson((JObject)liObj);
                }
            }
        }

        public EnumListValue ByDescription(string description)
        {
            return _values.FirstOrDefault(c => c.Description == description);
        }

        public override void Validate()
        {
            
        }

        public override void Cancel()
        {
            
        }

        public override void Update()
        {
            
        }

        public void Add(EnumListValue item)
        {
            item.ObjectState = ObjectStates.Added;
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(EnumListValue item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(EnumListValue[] array, int arrayIndex)
        {
            
        }

        public bool Remove(EnumListValue item)
        {
            return _values.Remove(item);
        }

        internal EnumListValue ByKey(string key)
        {
            return _values.FirstOrDefault(c => c.Key.Equals(key));
        }

        public int Compare(EnumListValues other)
        {
            foreach (var enumListValue in _values)
            {
                var enumValueIn = ByKey(enumListValue.Key);
                if (enumValueIn != null)
                {
                    //compare
                    var different = enumListValue.Compare(enumValueIn) == 0;
                    if (different)
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
