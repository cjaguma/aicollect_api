using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public enum AttributeTypes
    {
        SimpleAttribute = 0,
        Objectlink = 1,
        Transitionlink = 2
    }
    public enum TriggerEvents
    {
        None = 0x0,
        Insert = 0x1,
        Update = 0x2,
        Delete = 0x4
    }

    public enum EnumListTypes
    {
        Region,
        Question,
        Product,
        Price
    }

    public enum Cardinalities
    {
        Optional = 0,
        Required = 1
    }

    public enum DataTypes
    {
        None = 0,
        Alphanumeric = 1,
        AlphanumericMasked = 2,
        Date = 3,
        Time = 4,
        Numeric = 5,
        YesNo = 6,
        Memo = 7,
        DateTime = 8,
        List = 9,
        Binary = 10,
        Autonumber = 11,
        Calculated = 12,
        Image = 13,
        Audio = 14,
        Video = 15,
        BarCode = 16,
        MultipleSelect = 17,
        TextBlock = 18,
        UniqueIdentifier = 19,
        Illustration = 20,
        Enumeration = 21,
        Telephone = 22,
		Map = 23,
        Location = 24
    }

    public enum SyncDirection
    {
        None = 0,
        Up,
        Down,
        TwoWay
    }
    public enum LinkedAttributeTypes
    {
        Object,
        Transition
    }
    public enum Platforms
    {
        Mobile,
        Web
    }

    public enum DataCollectionObectTypes
    {
        Section,
        Question,
        SubSection,
        None,
        Questionaire
    }

    public enum QuestionTypes
    {
        None,
        Closed,
        Open,
        MultipleChoice,
        Map,
        Location,
        Other
    }

    public enum QuestionSubType
    {
        Numeric,
        Text,
        MultilineText,
        Date,
        Map,
        Photo, 
        PhotoGalarry

    }
    public enum DataAttributeTypes
    {
        Simple,
        Link,
        TransitionLink
    }

    public enum ObjectStates
    {
        None = 0,
        Removed = 1,
        Added = 2,
        Modified = 3
    }

    public enum DataProviders
    {
        /// <summary>
        /// POSTGRES SQL
        /// </summary>
        POSTGRESSQL,
        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        SQL,
        /// <summary>
        /// Microsoft SQL Server Compact
        /// </summary>
        SQLCE,
        MYSQL,
        SQLite,
        Oracle,
        OLEDB
        //WebService,
        //XML
    }

    public enum Authentications
    {
        Windows,
        Database
    }

    public enum ObjectType
    {
        Configuration = 0,
        Questionaire = 1,
        Section = 2,
        Subsection = 3,
        Purchase = 4,
        Certification = 5,
        Training = 6,
        Inspection = 7,
        Admin = 8,
        SuperAdmin = 9,
        Question,
        EnumList,
        None = 10
    }

    public enum DatabaseTypes
    {
        SQLServer,
        MySQL,
        SQLite
    }

    public enum Multiplicities
    {
        ZT1 = 1,
        ZTN = 2
    }


    [Flags]
    public enum PermisionType : byte
    {
        None = 0,
        View = 1,
        Add = 2,
        Delete = 4,
        Edit = 8,
    }

    public enum Status
    {
        On = 1,
        Off = 2
    }

    public enum ConnectionTestResults { ErrorInConnection, ConnectionFailed, ConnectionSucceeded, DatabasedoesnotExist, NotYouIntegreatitDB, DBNeedsToBeUpgraded, DBGeneratedInNewerVersion }
    /// <summary>
    ///  Specifies identifiers to indicate the different types of connection test.
    /// </summary>
    public enum ConnectionTestTypes { ConnectivityTest, IsYouIntegreatitDB, DatabaseExists }

    public enum PermissionObjects
    {
        Questionaire,
        Section,
        SubSection,
        Question
    }

    public enum CertificationTypes
    {
        Organic,
        FairTrade,
        UTZ
    }

  
    public enum Statuses
    {
        Template,
        New,
        Partial,
        Completed,
        Suspended
    }

    public enum UserStatuses
    {
        Active,
        Inactive
    }

    public enum UserTypes
    {
        ClientUser,
        RokeConsult
    }

    public enum Plan
    {
        Monthly,
        Quartely,
        Yearly
    }

    public enum PaymentStatus
    {
        Pending,
        Paid
    }

    public enum TemplateTypes
    {
        Questionaire,
        FieldInspection,
        Certification
    }

    public enum ConfigurationTypes
    {
        Agriculture,
        Health,
        Education,
        Energy,
        Audits
    }

}