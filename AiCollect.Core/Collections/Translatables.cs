using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class Translatables : AiCollectObject, IEnumerable<Translatable>
    {
        private List<Translatable> _translatables;
        #region Constructor
        public Translatables()
        {
            Init();
        }
        #endregion
        #region Add
        public Translatable Add()
        {
            Translatable translatable = new Translatable();
            _translatables.Add(translatable);
            return translatable;
        }

        public void Add(Translatable translatable)
        {
            _translatables.Add(translatable);
        }
        #endregion
        #region Init
        private void Init()
        {
            _translatables = new List<Translatable>();
        }
        #endregion

        public override void Cancel()
        {

        }

        public IEnumerator<Translatable> GetEnumerator()
        {
            foreach (var t in _translatables)
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


        internal string Translate(string originalText, Configuration configuration)
        {
            foreach (var tb in _translatables)
            {
                if (!tb.Name.Equals(originalText)) continue;
                foreach (var translation in tb.Translations)
                {
                    if (translation.Language.Code != configuration.Languages.Default.Code) continue;
                    return translation.TranslatedText;
                }
            }
            return "";
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _translatables.Clear();
            JArray translatablesObjs = JArray.FromObject(obj["Translatables"]);
            if (translatablesObjs != null)
            {
                foreach (var cobj in translatablesObjs)
                {
                    var translatable = Add();
                    translatable.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
