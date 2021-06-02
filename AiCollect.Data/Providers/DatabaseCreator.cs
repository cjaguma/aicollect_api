using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AiCollect.Core;
using System.Data.SqlClient;
using System.Data;

namespace AiCollect.Data.Providers
{
    public class DatabaseCreator : Provider
    {
        public Client Client { get; set; }
        public DatabaseCreator(dloDbInfo dbInfo, Client client) : base(dbInfo)
        {
            Client = client;
        }


        internal bool CreateDatabase()
        {

            List<string> queries = new List<string>();
            string query = string.Empty;
            //create db query

            query = "";
            query = $@"
				
				USE[{Client.Name}]
				
				CREATE TABLE[dbo].[dsto_answer](
					[OID][int] IDENTITY(1, 1) NOT NULL,
					[guid] [uniqueidentifier]
						NOT NULL,
					[created_by]
							[varchar]
						(max) NULL,

				[answertext] [varchar]
						(max) NULL,

				[yref_question] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[yref_questionaire] [uniqueidentifier] NULL,
					[yref_fieldInspection] [uniqueidentifier] NULL,
					[yref_certification] [uniqueidentifier] NULL,
					[occurance] [int] NULL,
				 CONSTRAINT[PK_dsto_answer] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				
				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_billing]
						(

				   [oid][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NOT NULL,

				   [bill] [decimal](18, 0) NULL,
					[billingdate] [datetime] NULL,
					[invoiceno] [varchar] (100) NULL,
					[yref_package] [uniqueidentifier] NULL,
					[clientid] [int] NULL,
					[paymentstatus] [int] NOT NULL,
					[deleted] [bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_billing] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_category]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [name] [varchar]
						(max) NULL,

				  [deleted] [bit]
						NOT NULL,
				CONSTRAINT[PK_dsto_category] PRIMARY KEY CLUSTERED
				(
				 [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_certification]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [varchar] (200) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [nvarchar] (25) NULL,
					[configuration_id] [int] NULL,
					[CertificationType] [int] NULL,
					[farmerid] [uniqueidentifier] NULL,
					[status] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[yref_template] [uniqueidentifier] NULL,
				 CONSTRAINT[PK__dsto_Cer__497F6CB41164961F] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_client]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [varchar] (50) NOT NULL,


					 [contact] [varchar] (50) NOT NULL,


					  [location] [varchar] (50) NOT NULL,


					   [email] [varchar] (50) NULL,
					[yref_package] [uniqueidentifier] NULL,
					[expiry_date] [datetime] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[logo] [varchar]
						(max) NULL,
				 CONSTRAINT[PK__dsto_cli__CB394B392A98BA9E] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_configuration]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [name] [varchar]
						(max) NULL,

				  [filename] [varchar]
						(max) NULL,

				  [version] [nvarchar] (20) NOT NULL,

				   [config] [varchar] (max) NOT NULL,
					[status] [int] NULL,
					[created_on] [datetime] NULL,
					[client_id] [int] NULL,
					[type] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[category] [int] NULL,
				 CONSTRAINT[pk_oid] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_ConfigurationUser]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [yref_user] [int] NULL,
					[yref_configuration] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_ConfigurationUser] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_conflicts]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[imei] [nvarchar] (20) NOT NULL,


					 [data] [varchar] (max) NOT NULL,
					[conflict_type] [int] NULL,
					[master_table_key] [varchar] (50) NOT NULL,


					 [master_row_oid] [int] NOT NULL,


					 [master_row_key] [varchar] (50) NOT NULL,


					  [status] [int] NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_dependency]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NULL,
					[TargetObjectType] [int] NULL,
					[yref_question] [varchar] (50) NULL,
					[targetobjectkey]
						[uniqueidentifier]
						NOT NULL,
					[deleted] [bit]
						NOT NULL,
					[template] [uniqueidentifier] NULL,
				 CONSTRAINT[PK_dsto_dependency] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_EnumLists]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [varchar] (25) NULL,
					[type] [int] NULL,
					[configurationId] [int] NULL,
					[questionId] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK__dsto_Enu__497F6CB4F92B45A3] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_EnumListValues]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[Code] [int] NULL,
					[Description] [varchar] (25) NULL,
					[enumListId] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_fieldcoordinates]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[latitude] [varchar] (50) NULL,
					[longitude] [varchar] (50) NULL,
					[field_inspection_id] [int] NULL,
				 CONSTRAINT[pk_coordinates] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_fieldInspection]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [varchar] (200) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [nvarchar] (25) NULL,
					[configuration_id] [int] NULL,
					[Status] [int] NULL,
					[datetaken] [datetime] NULL,
					[farmerid] [uniqueidentifier] NULL,
					[fieldname] [varchar] (50) NULL,
					[deleted]
						[bit]
						NOT NULL,
					[yref_template] [uniqueidentifier] NULL,
				 CONSTRAINT[PK_dsto_fieldInspection] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_package]
						(

				   [oid][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NOT NULL,

				   [name] [varchar] (100) NOT NULL,
					[plan] [int] NOT NULL,
					[price] [decimal](18, 0) NOT NULL,
					[deleted] [bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_package] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_permissions]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [object_id] [nvarchar] (38) NOT NULL,

				   [objectname] [nvarchar] (50) NOT NULL,
					[permission] [ntext]
						NOT NULL,
					[permission_type] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_purchase]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [varchar] (200) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [nvarchar] (25) NULL,
					[lotid] [varchar] (50) NULL,
					[configuration_id] [int] NULL,
					[quantity] [decimal](18, 0) NULL,
					[dateofpurchase] [datetime] NULL,
					[price] [decimal](18, 0) NULL,
					[farmerid] [uniqueidentifier] NULL,
					[station] [int] NULL,
					[product] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[pk_purchase] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_questionaire]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [varchar] (200) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [varchar] (200) NULL,
					[name] [nvarchar] (25) NULL,
					[configuration_id] [int] NULL,
					[status] [int] NULL,
					[latitude] [decimal](18, 10) NULL,
					[longitude] [decimal](18, 10) NULL,
					[yref_template] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[yref_category] [varchar]
						(max) NULL,
					[yref_region] [varchar]
						(max) NULL,
					[code] [bigint] NULL,
				 CONSTRAINT[PK__dsto_que__497F6CB4270649B4] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY],
				 CONSTRAINT[uni_oid_questionaire] UNIQUE NONCLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_questionaireXcategory]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[yref_questionaire] [uniqueidentifier] NULL,
					[yref_category] [uniqueidentifier] NULL,
				PRIMARY KEY CLUSTERED
				(
					[OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_questions]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[questiontext] [varchar] (max) NOT NULL,
					[question_type] [int] NULL,
					[data_type] [int] NULL,
					[answer]
						[varchar]
						(max) NULL,
					[yref_questionaire] [uniqueidentifier] NULL,
					[yref_section] [uniqueidentifier] NULL,
					[yref_subsection] [uniqueidentifier] NULL,
					[required] [bit] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_questions_1] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_Region]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar]
						(max) NULL,

				  [yref_questionaire] [uniqueidentifier] NULL,
					[name]
						[varchar]
						(max) NULL,
					[prefix] [varchar]
						(max) NULL,
					[deleted] [bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_Region] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_sections]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[yref_questionaire] [uniqueidentifier] NULL,
					[yref_field_inspection] [uniqueidentifier] NULL,
					[Name] [varchar] (50) NULL,
					[IsCompleted] [bit] NULL,
					[yref_certification] [uniqueidentifier] NULL,
					[yref_template] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
					[description] [varchar] (1000) NULL,
				 CONSTRAINT[PK__dsto_sec__497F6CB49FC124B2] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_sectionXquestion]
						(

				   [oid][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NULL,
					[created_on] [datetime] NULL,
					[deleted] [int] NULL,
					[yref_question] [uniqueidentifier] NULL,
					[yref_section] [uniqueidentifier] NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_skipcondition]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NULL,
					[yref_attribute] [varchar] (100) NULL,
					[yref_target] [varchar] (100) NULL,
					[answer] [varchar] (100) NULL,
					[yref_question] [varchar] (100) NULL,
					[deleted]
						[bit]
						NOT NULL,
					[dataCollectionObectType] [int] NULL,
				 CONSTRAINT[PK_dsto_skipcondition] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_subsections]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [name] [varchar]
						(max) NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[yref_section] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK__dsto_sub__497F6CB49242BCD7] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_subsectionXquestion]
						(

				   [oid][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_by] [varchar] (50) NULL,
					[created_on] [datetime] NULL,
					[deleted] [int] NULL,
					[yref_question] [uniqueidentifier] NULL,
					[tref_section] [uniqueidentifier] NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_template]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[name] [nvarchar] (25) NULL,
					[description]
						[varchar]
						(max) NULL,
					[templateType] [int] NULL,
					[yref_category] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				

				
				CREATE TABLE[dbo].[dsto_topic]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[Name] [varchar] (25) NULL,
					[yref_training] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_trainee]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[yref_training] [uniqueidentifier] NULL,
					[yref_questionaire] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_trainer]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[Name] [varchar] (25) NULL,
					[yref_training] [uniqueidentifier] NULL,
					[deleted]
						[bit]
						NOT NULL,
				PRIMARY KEY CLUSTERED
				(
					[guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_training]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [varchar] (200) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [varchar] (200) NULL,
					[name] [nvarchar] (25) NULL,
					[configuration_id] [int] NULL,
					[startdate] [datetime] NULL,
					[enddate] [datetime] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK__dsto_Tra__497F6CB4BB9EE75B] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_user]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [created_on] [datetime] NULL,
					[created_by] [nvarchar] (25) NULL,
					[last_updated_on] [datetime] NULL,
					[last_modified_by] [nvarchar] (25) NULL,
					[firstname] [varchar] (50) NULL,
					[lastname] [varchar] (50) NULL,
					[username] [varchar] (50) NULL,
					[email] [varchar] (50) NOT NULL,


					 [userType] [int] NULL,
					[password] [varchar] (50) NULL,
					[client_id] [int] NULL,
					[isadmin] [bit] NULL,
					[usercode] [varchar] (25) NULL,
					[deleted]
						[bit]
						NOT NULL,
					[enabled] [bit]
						NOT NULL,
				 CONSTRAINT[PK__dsto_use__CB394B39810584C8] PRIMARY KEY CLUSTERED
				(
				   [OID] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_userpermission]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [permission] [int] NULL,
					[permissionobject] [int] NULL,
					[yref_userright] [int] NULL,
					[deleted]
						[bit]
						NOT NULL,
				 CONSTRAINT[PK_dsto_userpermission] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]
				

				SET ANSI_NULLS ON
				
				SET QUOTED_IDENTIFIER ON
				
				CREATE TABLE[dbo].[dsto_userright]
						(

				   [OID][int] IDENTITY(1,1) NOT NULL,

				  [guid] [uniqueidentifier]
						NOT NULL,

				  [objectname] [varchar]
						(max) NULL,

				  [objecttype] [int] NULL,
					[primarykey] [varchar] (100) NULL,
					[secondarykey] [varchar] (100) NULL,
					[deleted]
						[bit]
						NOT NULL,
					[yref_user] [int] NULL,
					[yref_configuration] [int] NULL,
				 CONSTRAINT[PK_dsto_userright] PRIMARY KEY CLUSTERED
				(
				   [guid] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				
				ALTER TABLE[dbo].[dsto_answer] ADD CONSTRAINT[DF_dsto_answer_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_answer] ADD CONSTRAINT[DF_dsto_answer_occurance]  DEFAULT((0)) FOR[occurance]
				
				ALTER TABLE[dbo].[dsto_billing] ADD CONSTRAINT[DF_dsto_billing_paymentstatus]  DEFAULT((0)) FOR[paymentstatus]
				
				ALTER TABLE[dbo].[dsto_billing] ADD CONSTRAINT[DF_dsto_billing_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_category] ADD CONSTRAINT[DF_dsto_category_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_certification] ADD CONSTRAINT[DF__dsto_Certi__guid__09A971A2]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_certification] ADD CONSTRAINT[DF__dsto_Cert__creat__0A9D95DB]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_certification] ADD CONSTRAINT[DF__dsto_Cert__last___0B91BA14]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_certification] ADD CONSTRAINT[DF_dsto_certification_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_client] ADD CONSTRAINT[DF__dsto_clien__guid__5CD6CB2B]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_client] ADD CONSTRAINT[DF__dsto_clie__creat__5DCAEF64]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_client] ADD CONSTRAINT[DF__dsto_clie__last___5EBF139D]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_client] ADD CONSTRAINT[DF_dsto_client_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_configuration] ADD CONSTRAINT[DF__dsto_confi__guid__24927208]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_configuration] ADD CONSTRAINT[DF__dsto_conf__statu__25869641]  DEFAULT('0') FOR[status]
				
				ALTER TABLE[dbo].[dsto_configuration] ADD CONSTRAINT[DF__dsto_conf__creat__267ABA7A]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_configuration] ADD CONSTRAINT[DF_dsto_configuration_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_ConfigurationUser] ADD CONSTRAINT[DF_dsto_ConfigurationUser_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_conflicts] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_conflicts] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_conflicts] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_conflicts] ADD DEFAULT('0') FOR[conflict_type]
				
				ALTER TABLE[dbo].[dsto_conflicts] ADD DEFAULT('0') FOR[status]
				
				ALTER TABLE[dbo].[dsto_dependency] ADD CONSTRAINT[DF_dsto_dependency_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_EnumLists] ADD CONSTRAINT[DF__dsto_EnumL__guid__681373AD]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_EnumLists] ADD CONSTRAINT[DF__dsto_Enum__creat__690797E6]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_EnumLists] ADD CONSTRAINT[DF__dsto_Enum__last___69FBBC1F]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_EnumLists] ADD CONSTRAINT[DF_dsto_EnumLists_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_EnumListValues] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_EnumListValues] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_EnumListValues] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_EnumListValues] ADD CONSTRAINT[DF_dsto_EnumListValues_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_fieldcoordinates] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_fieldcoordinates] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_fieldcoordinates] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] ADD CONSTRAINT[DF__dsto_Field__guid__02FC7413]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] ADD CONSTRAINT[DF__dsto_Fiel__creat__03F0984C]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] ADD CONSTRAINT[DF__dsto_Fiel__last___04E4BC85]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] ADD CONSTRAINT[DF_dsto_fieldInspection_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_package] ADD CONSTRAINT[DF_dsto_package_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_permissions] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_permissions] ADD DEFAULT('1') FOR[permission_type]
				
				ALTER TABLE[dbo].[dsto_permissions] ADD CONSTRAINT[DF_dsto_permissions_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_purchase] ADD CONSTRAINT[DF__dsto_Purch__guid__151B244E]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_purchase] ADD CONSTRAINT[DF__dsto_Purc__creat__160F4887]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_purchase] ADD CONSTRAINT[DF__dsto_Purc__last___17036CC0]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_purchase] ADD CONSTRAINT[DF_dsto_purchase_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_questionaire] ADD CONSTRAINT[DF__dsto_quest__guid__33D4B598]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_questionaire] ADD CONSTRAINT[DF__dsto_ques__creat__34C8D9D1]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_questionaire] ADD CONSTRAINT[DF__dsto_ques__last___35BCFE0A]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_questionaire] ADD CONSTRAINT[DF_dsto_questionaire_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF__dsto_quest__guid__46E78A0C]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF__dsto_ques__creat__47DBAE45]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF__dsto_ques__last___48CFD27E]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF__dsto_ques__quest__49C3F6B7]  DEFAULT('0') FOR[question_type]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF_dsto_questions_required]  DEFAULT((0)) FOR[required]
				
				ALTER TABLE[dbo].[dsto_questions] ADD CONSTRAINT[DF_dsto_questions_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_Region] ADD CONSTRAINT[DF_dsto_Region_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_sections] ADD CONSTRAINT[DF__dsto_secti__guid__38996AB5]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_sections] ADD CONSTRAINT[DF__dsto_sect__creat__398D8EEE]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_sections] ADD CONSTRAINT[DF__dsto_sect__last___3A81B327]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_sections] ADD CONSTRAINT[DF_dsto_sections_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_sectionXquestion] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_sectionXquestion] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_sectionXquestion] ADD DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_skipcondition] ADD CONSTRAINT[DF_dsto_skipcondition_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_skipcondition] ADD CONSTRAINT[DF_dsto_skipcondition_dataCollectionObectType]  DEFAULT((0)) FOR[dataCollectionObectType]
				
				ALTER TABLE[dbo].[dsto_subsections] ADD CONSTRAINT[DF__dsto_subse__guid__3F466844]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_subsections] ADD CONSTRAINT[DF__dsto_subs__creat__403A8C7D]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_subsections] ADD CONSTRAINT[DF__dsto_subs__last___412EB0B6]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_subsections] ADD CONSTRAINT[DF_dsto_subsections_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_subsectionXquestion] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_subsectionXquestion] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_subsectionXquestion] ADD DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_template] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_template] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_template] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_template] ADD CONSTRAINT[DF_dsto_template_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_topic] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_topic] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_topic] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_topic] ADD CONSTRAINT[DF_dsto_topic_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_trainee] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_trainee] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_trainee] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_trainee] ADD CONSTRAINT[DF_dsto_trainee_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_trainer] ADD DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_trainer] ADD DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_trainer] ADD DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_trainer] ADD CONSTRAINT[DF_dsto_trainer_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_training] ADD CONSTRAINT[DF__dsto_Train__guid__0F624AF8]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_training] ADD CONSTRAINT[DF__dsto_Trai__creat__10566F31]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_training] ADD CONSTRAINT[DF__dsto_Trai__last___114A936A]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_training] ADD CONSTRAINT[DF_dsto_training_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF__dsto_user__guid__70DDC3D8]  DEFAULT(newid()) FOR[guid]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF__dsto_user__creat__71D1E811]  DEFAULT(getdate()) FOR[created_on]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF__dsto_user__last___72C60C4A]  DEFAULT(getdate()) FOR[last_updated_on]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF_dsto_user_isadmin]  DEFAULT((0)) FOR[isadmin]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF_dsto_user_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_user] ADD CONSTRAINT[DF_dsto_user_enabled]  DEFAULT((1)) FOR[enabled]
				
				ALTER TABLE[dbo].[dsto_userpermission] ADD CONSTRAINT[DF_dsto_userpermission_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_userright] ADD CONSTRAINT[DF_dsto_userright_deleted]  DEFAULT((0)) FOR[deleted]
				
				ALTER TABLE[dbo].[dsto_answer] WITH CHECK ADD CONSTRAINT[FK_dsto_answer_dsto_questions] FOREIGN KEY([yref_question])
				REFERENCES[dbo].[dsto_questions]
						([guid])
				
				ALTER TABLE[dbo].[dsto_answer]
						CHECK CONSTRAINT[FK_dsto_answer_dsto_questions]
				
				ALTER TABLE[dbo].[dsto_billing] WITH NOCHECK ADD CONSTRAINT[FK_dsto_billing_dsto_client] FOREIGN KEY([clientid])
				REFERENCES[dbo].[dsto_client]
						([OID])
				
				ALTER TABLE[dbo].[dsto_billing]
						NOCHECK CONSTRAINT[FK_dsto_billing_dsto_client]
				
				ALTER TABLE[dbo].[dsto_billing] WITH CHECK ADD CONSTRAINT[FK_dsto_billing_dsto_package] FOREIGN KEY([yref_package])
				REFERENCES[dbo].[dsto_package]
						([guid])
				
				ALTER TABLE[dbo].[dsto_billing]
						CHECK CONSTRAINT[FK_dsto_billing_dsto_package]
				
				ALTER TABLE[dbo].[dsto_certification] WITH NOCHECK ADD CONSTRAINT[fk_configuration_Certification] FOREIGN KEY([configuration_id])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_certification]
						NOCHECK CONSTRAINT[fk_configuration_Certification]
				
				ALTER TABLE[dbo].[dsto_certification] WITH CHECK ADD CONSTRAINT[fk_questionaire_dsto_Certification] FOREIGN KEY([farmerid])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_certification]
						CHECK CONSTRAINT[fk_questionaire_dsto_Certification]
				
				ALTER TABLE[dbo].[dsto_client] WITH CHECK ADD CONSTRAINT[FK_dsto_client_dsto_package] FOREIGN KEY([yref_package])
				REFERENCES[dbo].[dsto_package]
						([guid])
				
				ALTER TABLE[dbo].[dsto_client]
						CHECK CONSTRAINT[FK_dsto_client_dsto_package]
				
				ALTER TABLE[dbo].[dsto_ConfigurationUser] WITH CHECK ADD CONSTRAINT[FK_dsto_ConfigurationUser_dsto_configuration] FOREIGN KEY([yref_configuration])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_ConfigurationUser]
						CHECK CONSTRAINT[FK_dsto_ConfigurationUser_dsto_configuration]
				
				ALTER TABLE[dbo].[dsto_ConfigurationUser] WITH CHECK ADD CONSTRAINT[FK_dsto_ConfigurationUser_dsto_user] FOREIGN KEY([yref_user])
				REFERENCES[dbo].[dsto_user]
						([OID])
				
				ALTER TABLE[dbo].[dsto_ConfigurationUser]
						CHECK CONSTRAINT[FK_dsto_ConfigurationUser_dsto_user]
				
				ALTER TABLE[dbo].[dsto_EnumLists] WITH NOCHECK ADD CONSTRAINT[FK__dsto_Enum__confi__6AEFE058] FOREIGN KEY([configurationId])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_EnumLists]
						NOCHECK CONSTRAINT[FK__dsto_Enum__confi__6AEFE058]
				
				ALTER TABLE[dbo].[dsto_EnumListValues] WITH CHECK ADD CONSTRAINT[FK__dsto_Enum__enumL__70A8B9AE] FOREIGN KEY([enumListId])
				REFERENCES[dbo].[dsto_EnumLists]
						([guid])
				
				ALTER TABLE[dbo].[dsto_EnumListValues]
						CHECK CONSTRAINT[FK__dsto_Enum__enumL__70A8B9AE]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] WITH CHECK ADD CONSTRAINT[fk_configuration_Inspection] FOREIGN KEY([configuration_id])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_fieldInspection]
						CHECK CONSTRAINT[fk_configuration_Inspection]
				
				ALTER TABLE[dbo].[dsto_fieldInspection] WITH CHECK ADD CONSTRAINT[fk_questionaire_dsto_FieldInspection] FOREIGN KEY([farmerid])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_fieldInspection]
						CHECK CONSTRAINT[fk_questionaire_dsto_FieldInspection]
				
				ALTER TABLE[dbo].[dsto_purchase] WITH CHECK ADD CONSTRAINT[fk_configuration_Purchase] FOREIGN KEY([configuration_id])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_purchase]
						CHECK CONSTRAINT[fk_configuration_Purchase]
				
				ALTER TABLE[dbo].[dsto_purchase] WITH CHECK ADD CONSTRAINT[fk_questionaire_purchase] FOREIGN KEY([farmerid])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_purchase]
						CHECK CONSTRAINT[fk_questionaire_purchase]
				
				ALTER TABLE[dbo].[dsto_questionaire] WITH CHECK ADD CONSTRAINT[fk_configuration] FOREIGN KEY([configuration_id])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_questionaire]
						CHECK CONSTRAINT[fk_configuration]
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory] WITH CHECK ADD CONSTRAINT[FK__dsto_ques__yref___4707859D] FOREIGN KEY([yref_questionaire])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory]
						CHECK CONSTRAINT[FK__dsto_ques__yref___4707859D]
				
				ALTER TABLE[dbo].[dsto_questionaireXcategory] WITH CHECK ADD FOREIGN KEY([yref_category])
				REFERENCES[dbo].[dsto_category]
						([guid])
				
				ALTER TABLE[dbo].[dsto_questions] WITH CHECK ADD CONSTRAINT[FK__dsto_ques__yref___4AB81AF0] FOREIGN KEY([yref_questionaire])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_questions]
						CHECK CONSTRAINT[FK__dsto_ques__yref___4AB81AF0]
				
				ALTER TABLE[dbo].[dsto_questions] WITH NOCHECK ADD CONSTRAINT[FK__dsto_ques__yref___4BAC3F29] FOREIGN KEY([yref_section])
				REFERENCES[dbo].[dsto_sections]
						([guid])
				
				ALTER TABLE[dbo].[dsto_questions]
						NOCHECK CONSTRAINT[FK__dsto_ques__yref___4BAC3F29]
				
				ALTER TABLE[dbo].[dsto_questions] WITH CHECK ADD CONSTRAINT[FK__dsto_ques__yref___4CA06362] FOREIGN KEY([yref_subsection])
				REFERENCES[dbo].[dsto_subsections]
						([guid])
				
				ALTER TABLE[dbo].[dsto_questions]
						CHECK CONSTRAINT[FK__dsto_ques__yref___4CA06362]
				
				ALTER TABLE[dbo].[dsto_sections] WITH CHECK ADD CONSTRAINT[FK__dsto_sect__yref___3C69FB99] FOREIGN KEY([yref_questionaire])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_sections]
						CHECK CONSTRAINT[FK__dsto_sect__yref___3C69FB99]
				
				ALTER TABLE[dbo].[dsto_sections] WITH CHECK ADD FOREIGN KEY([yref_template])
				REFERENCES[dbo].[dsto_template]
						([guid])
				
				ALTER TABLE[dbo].[dsto_sections] WITH CHECK ADD CONSTRAINT[FK_dsto_sections_dsto_certification] FOREIGN KEY([yref_certification])
				REFERENCES[dbo].[dsto_certification]
						([guid])
				
				ALTER TABLE[dbo].[dsto_sections]
						CHECK CONSTRAINT[FK_dsto_sections_dsto_certification]
				
				ALTER TABLE[dbo].[dsto_sections] WITH CHECK ADD CONSTRAINT[FK_dsto_sections_dsto_fieldInspection] FOREIGN KEY([yref_field_inspection])
				REFERENCES[dbo].[dsto_fieldInspection]
						([guid])
				
				ALTER TABLE[dbo].[dsto_sections]
						CHECK CONSTRAINT[FK_dsto_sections_dsto_fieldInspection]
				
				ALTER TABLE[dbo].[dsto_sectionXquestion] WITH CHECK ADD CONSTRAINT[FK__dsto_sect__yref___534D60F1] FOREIGN KEY([yref_section])
				REFERENCES[dbo].[dsto_sections]
						([guid])
				
				ALTER TABLE[dbo].[dsto_sectionXquestion]
						CHECK CONSTRAINT[FK__dsto_sect__yref___534D60F1]
				
				ALTER TABLE[dbo].[dsto_subsections] WITH NOCHECK ADD CONSTRAINT[FK__dsto_subs__yref___440B1D61] FOREIGN KEY([yref_section])
				REFERENCES[dbo].[dsto_sections]
						([guid])
				
				ALTER TABLE[dbo].[dsto_subsections]
						NOCHECK CONSTRAINT[FK__dsto_subs__yref___440B1D61]
				
				ALTER TABLE[dbo].[dsto_subsectionXquestion] WITH CHECK ADD CONSTRAINT[FK__dsto_subs__tref___59FA5E80] FOREIGN KEY([tref_section])
				REFERENCES[dbo].[dsto_subsections]
						([guid])
				
				ALTER TABLE[dbo].[dsto_subsectionXquestion]
						CHECK CONSTRAINT[FK__dsto_subs__tref___59FA5E80]
				
				ALTER TABLE[dbo].[dsto_template] WITH CHECK ADD CONSTRAINT[FK_dsto_template_dsto_category] FOREIGN KEY([yref_category])
				REFERENCES[dbo].[dsto_category]
						([guid])
				
				ALTER TABLE[dbo].[dsto_template]
						CHECK CONSTRAINT[FK_dsto_template_dsto_category]
				
				ALTER TABLE[dbo].[dsto_topic] WITH CHECK ADD CONSTRAINT[FK__dsto_topi__yref___44CA3770] FOREIGN KEY([yref_training])
				REFERENCES[dbo].[dsto_training]
						([guid])
				
				ALTER TABLE[dbo].[dsto_topic]
						CHECK CONSTRAINT[FK__dsto_topi__yref___44CA3770]
				
				ALTER TABLE[dbo].[dsto_topic] WITH CHECK ADD CONSTRAINT[FK__dsto_topi__yref___46B27FE2] FOREIGN KEY([yref_training])
				REFERENCES[dbo].[dsto_training]
						([guid])
				
				ALTER TABLE[dbo].[dsto_topic]
						CHECK CONSTRAINT[FK__dsto_topi__yref___46B27FE2]
				
				ALTER TABLE[dbo].[dsto_trainee] WITH CHECK ADD CONSTRAINT[FK__dsto_trai__yref___5224328E] FOREIGN KEY([yref_training])
				REFERENCES[dbo].[dsto_training]
						([guid])
				
				ALTER TABLE[dbo].[dsto_trainee]
						CHECK CONSTRAINT[FK__dsto_trai__yref___5224328E]
				
				ALTER TABLE[dbo].[dsto_trainee] WITH CHECK ADD CONSTRAINT[FK_dsto_trainee_dsto_questionaire] FOREIGN KEY([yref_questionaire])
				REFERENCES[dbo].[dsto_questionaire]
						([guid])
				
				ALTER TABLE[dbo].[dsto_trainee]
						CHECK CONSTRAINT[FK_dsto_trainee_dsto_questionaire]
				
				ALTER TABLE[dbo].[dsto_trainer] WITH CHECK ADD CONSTRAINT[FK__dsto_trai__yref___4C6B5938] FOREIGN KEY([yref_training])
				REFERENCES[dbo].[dsto_training]
						([guid])
				
				ALTER TABLE[dbo].[dsto_trainer]
						CHECK CONSTRAINT[FK__dsto_trai__yref___4C6B5938]
				
				ALTER TABLE[dbo].[dsto_training] WITH CHECK ADD CONSTRAINT[fk_configuration_Training] FOREIGN KEY([configuration_id])
				REFERENCES[dbo].[dsto_configuration]
						([OID])
				
				ALTER TABLE[dbo].[dsto_training]
						CHECK CONSTRAINT[fk_configuration_Training]
				
				ALTER TABLE[dbo].[dsto_user] WITH CHECK ADD CONSTRAINT[FK__dsto_user__clien__6FE99F9F] FOREIGN KEY([client_id])
				REFERENCES[dbo].[dsto_client]
						([OID])
				
				ALTER TABLE[dbo].[dsto_user]
						CHECK CONSTRAINT[FK__dsto_user__clien__6FE99F9F]	

				create table dsto_module
				(
					oid int identity(1,1) not null primary key,
					[guid] uniqueidentifier not null,
					created_on datetime,
					created_by varchar(50),
					last_modified_on datetime,
					last_modified_by varchar(50),
					[name] varchar(50),
					[yref_configuration] int references dsto_configuration(oid)
				)


				create table dsto_module_link
				(
					oid int identity(1,1) not null primary key,
					[guid] uniqueidentifier not null,
					created_on datetime,
					created_by varchar(50),
					last_modified_on datetime,
					last_modified_by varchar(50),
					[name] varchar(50),
					[parentobject] varchar(max),
					[referredobject] varchar(max),
					[multiplicity] int
				)

				alter table dsto_questionaire add yref_module int foreign key references dsto_module(OID)

				";

            string createDatabase =
                $@"USE[master]			
				CREATE DATABASE[{Client.Name}]";
            if (!DbInfo.DatabaseExists(Client.Name))
                if (DbInfo.ExecuteNonQuery(createDatabase, true) >= -1)
                    return DbInfo.ExecuteNonQuery(query, true) >= -1;
            return false;
        }
    }
}