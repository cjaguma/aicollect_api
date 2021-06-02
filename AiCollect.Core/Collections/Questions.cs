using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class Questions : AiCollectObject, IEnumerable<Question>,ICollection<Question>
    {

        public IReadOnlyCollection<Question> List
        {
            get
            {
                return _questions.AsReadOnly();
            }
        }

        public List<Question> _questions;

        public Questions(AiCollectObject parent) : base(parent)
        {
            _questions = new List<Question>();
        }
        
        public Questions():base(null)
        {
            _questions = new List<Question>();
        }

        public Question Add(QuestionTypes questionType)
        {
            Question question=null;
            switch(questionType)
            {
                case QuestionTypes.Closed:
                    question = AddClosedQuestion();
                    break;
                case QuestionTypes.MultipleChoice:
                    question = AddMultipleQuestion();
                    break;
                case QuestionTypes.Open:
                    question = AddOpenQuestion();
                    break;
                case QuestionTypes.Map:
                    question = AddMapQuestion();
                    break;
                case QuestionTypes.Location:
                    question = AddLocationQuestion();
                    break;
                case QuestionTypes.Other:

                break;
            }
            return question;
        }



        public int Count
        {
            get
            {
                return _questions.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Question this[int index]
        {
            get
            {
                return _questions[index];
            }
        }

        public ClosedQuestion AddClosedQuestion()
        {
            ClosedQuestion question = (ClosedQuestion)ObjectFactory.Create(DataCollectionObectTypes.Question, this, QuestionTypes.Closed);
            _questions.Add(question);
            return question;
        }

        public OpenQuestion AddOpenQuestion()
        {
            OpenQuestion question = (OpenQuestion)ObjectFactory.Create(DataCollectionObectTypes.Question, this, QuestionTypes.Open);
            _questions.Add(question);
            return question;
        }

        public LocationQuestion AddLocationQuestion()
        {
            LocationQuestion question = (LocationQuestion)ObjectFactory.Create(DataCollectionObectTypes.Question, this, QuestionTypes.Location);
            _questions.Add(question);
            return question;
        }

        public MapQuestion AddMapQuestion()
        {
            MapQuestion question = (MapQuestion)ObjectFactory.Create(DataCollectionObectTypes.Question, this, QuestionTypes.Map);
            _questions.Add(question);
            return question;
        }

        public MultipleChoiceQuestion AddMultipleQuestion()
        {
            MultipleChoiceQuestion question = (MultipleChoiceQuestion)ObjectFactory.Create(DataCollectionObectTypes.Question, this, QuestionTypes.MultipleChoice);
            _questions.Add(question);
            return question;
        }

        public void Add(Question item)
        {
            _questions.Add(item);
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _questions.Clear();
        }

        public bool Contains(Question item)
        {
            return _questions.Contains(item);
        }

        public void CopyTo(Question[] array, int arrayIndex)
        {
            
        }

        public IEnumerator<Question> GetEnumerator()
        {
            foreach (var q in _questions)
                yield return q;
        }

        public bool Remove(Question item)
        {
            return _questions.Remove(item);
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
