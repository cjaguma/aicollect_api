using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Devices : AiCollectObject, IEnumerable<Device>,ICollection<Device>
    {
        private List<Device> _devices;
        public Devices(AiCollectObject parent) : base(parent)
        {
            _devices = new List<Device>();
        }
        public int Count
        {
            get
            {
                return _devices.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(Device item)
        {
            _devices.Add(item);
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _devices.Clear();
        }

        public bool Contains(Device item)
        {
            return _devices.Contains(item);
        }

        public void CopyTo(Device[] array, int arrayIndex)
        {
            
        }

        public IEnumerator<Device> GetEnumerator()
        {
            foreach (var dv in _devices)
                yield return dv;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
        }

        public bool Remove(Device item)
        {
            return _devices.Remove(item);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
