using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Products : AiCollectObject, IEnumerable<Product>
    {
        private List<Product> _products;

        public Products(AiCollectObject parent) : base(parent)
        {
            _products = new List<Product>();
        }
        
        public Product Add()
        {
            Product product = new Product(this);
            product.ObjectState = ObjectStates.Added;
            _products.Add(product);
            return product;
        }

        public void Remove(Product product)
        {
            _products.Remove(product);
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Product> GetEnumerator()
        {
            foreach (var c in _products)
                yield return c;
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
    }
}
