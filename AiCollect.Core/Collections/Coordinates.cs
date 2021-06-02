using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Coordinates : AiCollectObject, IEnumerable<Coordinate>
    {
        private List<Coordinate> _coordinates;

        public new Questionaire Parent
        {
            get
            {
                return base.Parent as Questionaire;
            }
        }

        public Coordinates(Questionaire parent):base(parent)
        {
            _coordinates = new List<Coordinate>();
        }

        public int Count
        {
            get
            {
                return _coordinates.Count;
            }
        }

        public Coordinate Add()
        {
            Coordinate coordinate = new Coordinate(this);
            return coordinate;
        }

        public override void Cancel()
        {
           

        }

        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (var f in _coordinates)
                yield return f;
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
