using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class DataFields : AiCollectObject, IEnumerable<DataField>
    {
        #region Members
        private List<DataField> _fields;
        #endregion

        #region Constructor
        public DataFields()
        {
            _fields = new List<DataField>();
        }
        #endregion

        public DataField Add()
        {
            DataField dataField = new DataField();
            _fields.Add(dataField);
            return dataField;
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<DataField> GetEnumerator()
        {
            foreach (var df in _fields)
                yield return df;
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
