using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Clients : AiCollectObject, IEnumerable<Client>
    {

        private List<Client> _clients;

        public Clients()
        {
            _clients = new List<Client>();
        }

        public override void Cancel()
        {
            
        }

        public void Add(Client client)
        {
            _clients.Add(client);
        }
        public IEnumerator<Client> GetEnumerator()
        {
            foreach (var c in _clients)
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
