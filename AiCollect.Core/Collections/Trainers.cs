using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Trainers : AiCollectObject, IEnumerable<Trainer>
    {

        private List<Trainer> _trainers;

        public Trainers():base()
        {
            Init();
        }

        public Trainers(AiCollectObject parent):base(parent)
        {
            Init();
        }

        private void Init()
        {
            _trainers = new List<Trainer>();
        }

        public Trainer Add()
        {
            Trainer trainer = new Trainer(this);
            trainer.ObjectState = ObjectStates.Added;
            _trainers.Add(trainer);
            return trainer;
        }

        public void Remove(Trainer trainer)
        {
            _trainers.Remove(trainer);
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Trainer> GetEnumerator()
        {
            foreach (var tr in _trainers)
                yield return tr;
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
            JArray trainersObjs = JArray.FromObject(obj["Trainers"]);
            if (trainersObjs != null)
            {
                foreach (var cobj in trainersObjs)
                {
                    var trainer = Add();
                    trainer.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
