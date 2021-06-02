using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Languages : AiCollectObject, IEnumerable<Language>, ICollection<Language>
    {
        #region Members
        private List<Language> _languages;
        #endregion

        #region Properties

        #region Default 
        public Language Default
        {
            get
            {
                return _languages.Find(l => l.IsDefault);
            }
        }
        #endregion

        public int Count
        {
            get
            {
                return _languages.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Constructor
        public Languages(AiCollectObject parent) : base(parent)
        {
            _languages = new List<Language>();
        }
        #endregion

        public override void Cancel()
        {

        }

        #region Add Method
        public Language Add()
        {
            Language language = new Language(this);
            _languages.Add(language);
            return language;
        }
        #endregion

        public IEnumerator<Language> GetEnumerator()
        {
            foreach (var language in _languages)
                yield return language;
        }

        public override void Update()
        {
        }

        public override void Validate()
        { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _languages.Clear();
            JArray permissionsObj = JArray.FromObject(obj["Languages"]);
            foreach (JObject usrRightObj in permissionsObj)
            {
                var language = Add();
                language.ReadJson(usrRightObj);
            }
        }

        public override JObject ToJson()
        {
            JObject obj = base.ToJson();
            JArray jArray = new JArray();
            foreach (var language in _languages)
            {
                jArray.Add(language.ToJson());
            }          
            obj.Add("Languages", jArray);
            return obj;
        }

        public void Add(Language item)
        {
            _languages.Add(item);
        }

        public void Clear()
        {
            _languages.Clear();
        }

        public bool Contains(Language item)
        {
            return _languages.Contains(item);
        }

        public void CopyTo(Language[] array, int arrayIndex)
        {
            
        }

        public bool Remove(Language item)
        {
            return _languages.Remove(item);
        }
    }
}
