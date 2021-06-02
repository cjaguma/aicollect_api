using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Translations : AiCollectObject, IEnumerable<Translation>
    {
        private List<Translation> _translations;
        #region Constructor
        public Translations()
        {
            Init();
        }
        #endregion

        #region Init
        private void Init()
        {
            _translations = new List<Translation>();
        }
        #endregion

        #region Add
        public Translation Add()
        {
            Translation translation = new Translation();
            _translations.Add(translation);
            return translation;
        }
        #endregion

        public override void Cancel()
        {         
        }

        public IEnumerator<Translation> GetEnumerator()
        {
            foreach (var t in _translations)
                yield return t;
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
            _translations.Clear();
            JArray translationsObjs = JArray.FromObject(obj["Translations"]);
            if (translationsObjs != null)
            {
                foreach (var cobj in translationsObjs)
                {
                    var translation = Add();
                    translation.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
