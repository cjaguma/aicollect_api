namespace AiCollect.Core
{
    public static class ObjectFactory
    {
        public static DataCollectionObject Create(DataCollectionObectTypes type, AiCollectObject parent, QuestionTypes questionType = QuestionTypes.None)
        {
            DataCollectionObject dataCollectionObject = null;
            switch (type)
            {
                case DataCollectionObectTypes.Questionaire:
                    dataCollectionObject = new Questionaire(parent);
                    dataCollectionObject.CollectionObjectType = DataCollectionObectTypes.Questionaire;
                    break;
                case DataCollectionObectTypes.Section:
                    dataCollectionObject = new Section(parent);
                    dataCollectionObject.CollectionObjectType = DataCollectionObectTypes.Section;
                    break;
                case DataCollectionObectTypes.Question:
                    dataCollectionObject = CreateQuestion(parent, questionType);
                    dataCollectionObject.CollectionObjectType = DataCollectionObectTypes.Question;
                    break;
            }
            return dataCollectionObject;
        }

        public static Question CreateQuestion(AiCollectObject parent, QuestionTypes qtype)
        {
            Question question = null;
            switch (qtype)
            {
                case QuestionTypes.Closed:
                    question = new ClosedQuestion(parent);
                    break;
                case QuestionTypes.MultipleChoice:
                    question = new MultipleChoiceQuestion(parent);
                    break;
                case QuestionTypes.Open:
                    question = new OpenQuestion(parent);
                    break;
                case QuestionTypes.Map:
                    question = new MapQuestion(parent);
                    break;
                case QuestionTypes.Location:
                    question = new LocationQuestion(parent);
                    break;
                case QuestionTypes.Other:
                    question = new OtherQuestion(parent);
                    break;
            }
            if (question != null)
                question.QuestionType = qtype;
            return question;
        }

        public static Certification CreateCertification(CertificationTypes certificationType, AiCollectObject parent)
        {
            Certification certification = null;
            switch(certificationType)
            {
                case CertificationTypes.FairTrade:
                    certification = new FairTrade(parent);
                    break;
                case CertificationTypes.Organic:
                    certification = new Organic(parent);
                    break;
                case CertificationTypes.UTZ:
                    certification = new UTZ(parent);
                    break;
            }
            return certification;
        }

    }
}
