using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Answer:AiCollectObject
    {
        private string _answerText;
        [DataMember]
        public string AnswerText
        {
            get
            {
                return _answerText;
            }
            set
            {
                if (_answerText != value)
                {
                    _answerText = value;
                }
            }
        }
        [DataMember]
        public new string Key
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }
        [DataMember]
        public new int OID
        {
            get
            {
                return base.OID;
            }
            set
            {
                base.OID = value;
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string questionKey { get; set; }

        [DataMember]
        public string QuestionaireKey { get; set; }

        [DataMember]
        public string CertificationKey { get; set; }
        [DataMember]
        public string FieldKey { get; set; }

        /// <summary>
        /// Represents how many times a question occurs in a single dependency, 
        /// this means it will be 0 for a regular question in a section or subsection 
        /// but can be 1 or more depending on the numerical value entered in the question 
        /// that the subsequent section or subsection is dependant on.
        /// </summary>
        [DataMember]
        public int Occurance { get; set; }

        public Answer(AiCollectObject parent):base(parent)
        {

        }

        public override void Cancel()
        {
           
        }

        public override void Update()
        {
           
        }

        public override void Validate()
        {
           
        }
    }
}
