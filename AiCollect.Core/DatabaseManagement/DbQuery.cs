using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    public class DbQuery:AiCollectObject
    {

        int _tableCount;	// number of tables used

        public new DbQueries Parent
        {
            get
            {
                return base.Parent as DbQueries;
            }
        }
        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null)
                {
                    if (_name != value)
                    {
                        _name = value;
                        ObjectState = ObjectStates.Modified;
                        //OnPropertyChanged("Name");
                    }
                }
            }
        }

        private string _sql;
        /// <summary>
        /// Gets or sets the SQL string that represents the current <see cref="QueryFields"/>
        /// collection.
        /// </summary>
        public string Sql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sql))
                {
                  
                   // _sql = BuildSqlStatement();
                }
                return _sql;
            }
            set
            {

                if (!string.IsNullOrWhiteSpace(value))
                    _sql = value;
            }
        }

        private bool _isSystem;
        [DataMember]
        public bool IsSystem
        {
            get { return _isSystem; }
            set
            {
                if (value != _isSystem)
                {
                    _isSystem = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }



        private bool _isDefault;
        [DataMember]
        public bool IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                if (_isDefault != value)
                {
                    _isDefault = value;
                    ObjectState = ObjectStates.Modified;
                    if (_isDefault)
                    {
                        foreach (var q in Parent)
                        {
                            if (q.Key != this.Key)
                                q.IsDefault = false;
                        }
                    }

                }
            }
        }

        private bool _groupBy;
        /// <summary>
        /// Gets or sets a value that specifies whether the query groups the data.
        /// </summary>
        [DataMember]
        public bool GroupBy
        {
            get { return _groupBy; }
            set
            {
                if (_groupBy != value)
                {
                    _groupBy = value;
                    _sql = null;
                }
            }
        }

  

        private int _top;
        /// <summary>
        /// Gets or sets the number of records the query returns using a TOP clause.
        /// </summary>
        [DataMember]
        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                _sql = null;
            }
        }

        private bool _distinct;
        /// <summary>
        /// Gets or sets whether the query should return DISTINCT values.
        /// </summary>
        [DataMember]
        public bool Distinct
        {
            get { return _distinct; }
            set
            {
                _distinct = value;
                _sql = null;
            }
        }

        private bool _missingJoins;

        public DbQuery(AiCollectObject parent) : base(parent)
        {
        }

        /// <summary>
        /// Gets a value that indicates not all tables in the query are related.
        /// </summary>
        public bool MissingJoins
        {
            get { return _missingJoins; }
        }

        public int ExecuteQuery(string query)           
        {
            return 0;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
