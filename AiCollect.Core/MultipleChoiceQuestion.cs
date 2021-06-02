using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class MultipleChoiceQuestion : ChoiceQuestion
    {
     
        public MultipleChoiceQuestion(AiCollectObject parent) : base(parent)
        {
           
        }

        public override void Validate()
        {
           
        }

        public override void Cancel()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    break;             
            }
        }

        public override void Update()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    Validate();
                    break;
                case ObjectStates.Removed:
                    break;
            }
        }

        private MultipleChoiceQuestion _original;

        internal override void SetOriginal()
        {
            _original = Copy() as MultipleChoiceQuestion;
        }

    }
}
