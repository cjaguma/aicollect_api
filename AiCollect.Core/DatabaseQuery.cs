namespace AiCollect.Core
{
    public enum Order
    {
        First = 0,
        Last = 1
    }

    public enum Messages
    {
        UseDatabase = 1,
        CreateDatabase = 2,
        DropDatabase = 3,
        CreateTable = 4,
        AlterTable = 5,
        RenameTable = 6,
        DropTable = 7,
        InsertRecord = 8,
        UpdateRecord = 9,
        RenameColumn = 10,
        CreateTrigger = 11,
        DeleteRecord = 12,
        Warning = 13,
        EnableTableCT = 14,
        DisableTableCT = 15,
        EnableDBCT = 16,
        DisableDBCT = 17,
        CheckDBCT = 18,
        DropDefaultConstraint = 19,
        CreateFunction = 20,
        AlterDatabase,
        CreateProcedure,
        AlterProcedure,
        DropProcedure,
        DropForeignKeyConstraint,
    }

    public enum QueryTypes
    {
        User,
        Datalabs,
        System
    }

    public class DatabaseQuery
    {
        protected Order _order;

        public Order Order
        {
            get { return _order; }
            private set { _order = value; }
        }

        protected Messages _message;

        public Messages Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                if (_message == Messages.AlterTable || _message == Messages.DropDefaultConstraint)
                    _order = Order.Last;
                else _order = Order.First;
            }
        }

        protected string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        protected string _sqlStatement;

        public string SqlStatement
        {
            get { return _sqlStatement; }
            set { _sqlStatement = value; }
        }

        private int _result;

        public int Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public string FriendlyMessage { get; set; }
        public string ErrorMessage { get; set; }
        public QueryTypes QueryType { get; set; }
    }
}