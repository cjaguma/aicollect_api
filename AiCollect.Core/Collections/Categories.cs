using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Categories : AiCollectObject, IEnumerable<Category>
    {

        private List<Category> _categories;

        public Categories()
        {
            _categories = new List<Category>();
        }

        public Category Add()
        {
            Category category = new Category();
            _categories.Add(category);
            return category;
        }

        public void Add(Category category)
        {
            _categories.Add(category);
        }
        public void Clear()
        {
            if (_categories.Count > 0)
                _categories.Clear();
        }

        public override void Cancel()
        {

        }

        public IEnumerator<Category> GetEnumerator()
        {
            foreach (var b in _categories)
                yield return b;
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

        public void ReadJson(JArray jArrray)
        {
            if (jArrray == null) return;
            foreach (JObject categoryObj in jArrray)
            {
                var category = Add();
                category.ReadJson(categoryObj);
            }
        }
    }
}
