using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Trainees : AiCollectObject, IEnumerable<Trainee>
    {

        private List<Trainee> _trainees;

        public Trainees()
        {
            Init();
        }

        public Trainees(AiCollectObject parent):base(parent)
        {
            Init();
        }

        private void Init()
        {
            _trainees = new List<Trainee>();
        }

        public Trainee Add()
        {
            Trainee trainee = new Trainee(this);
            trainee.ObjectState = ObjectStates.Added;
            _trainees.Add(trainee);
            return trainee;
        }

        public void Remove(Trainee trainee)
        {
            _trainees.Remove(trainee);
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Trainee> GetEnumerator()
        {
            foreach (var tc in _trainees)
                yield return tc;
        }

        public override void Update()
        {
            
        }

        public override void Validate()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            JArray traineesObjs = JArray.FromObject(obj["Trainees"]);
            if (traineesObjs != null)
            {
                foreach (var cobj in traineesObjs)
                {
                    var trainee = Add();
                    trainee.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
