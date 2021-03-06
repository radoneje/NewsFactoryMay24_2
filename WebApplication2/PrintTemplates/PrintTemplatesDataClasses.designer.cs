#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication2.PrintTemplates
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="NewsFactory")]
	public partial class PrintTemplatesDataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void InsertNew(New instance);
    partial void UpdateNew(New instance);
    partial void DeleteNew(New instance);
    partial void InsertProgram(Program instance);
    partial void UpdateProgram(Program instance);
    partial void DeleteProgram(Program instance);
    partial void InsertPrintTemplate(PrintTemplate instance);
    partial void UpdatePrintTemplate(PrintTemplate instance);
    partial void DeletePrintTemplate(PrintTemplate instance);
    partial void InsertTemplateVariable(TemplateVariable instance);
    partial void UpdateTemplateVariable(TemplateVariable instance);
    partial void DeleteTemplateVariable(TemplateVariable instance);
    #endregion
		
		public PrintTemplatesDataClassesDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["NewsFactoryConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PrintTemplatesDataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PrintTemplatesDataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PrintTemplatesDataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PrintTemplatesDataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<New> News
		{
			get
			{
				return this.GetTable<New>();
			}
		}
		
		public System.Data.Linq.Table<Program> Programs
		{
			get
			{
				return this.GetTable<Program>();
			}
		}
		
		public System.Data.Linq.Table<PrintTemplate> PrintTemplates
		{
			get
			{
				return this.GetTable<PrintTemplate>();
			}
		}
		
		public System.Data.Linq.Table<vWeb_BlockForPrintTemplate> vWeb_BlockForPrintTemplates
		{
			get
			{
				return this.GetTable<vWeb_BlockForPrintTemplate>();
			}
		}
		
		public System.Data.Linq.Table<TemplateVariable> TemplateVariables
		{
			get
			{
				return this.GetTable<TemplateVariable>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.fWeb_GetUserName", IsComposable=true)]
		public string fWeb_GetUserName([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="BigInt")] System.Nullable<long> userId)
		{
			return ((string)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userId).ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.News")]
	public partial class New : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _id;
		
		private string _Name;
		
		private int _EditorId;
		
		private System.DateTime _NewsDate;
		
		private string _Description;
		
		private long _NewsTime;
		
		private long _CalcTime;
		
		private long _TaskTime;
		
		private bool _Deleted;
		
		private int _ProgramId;
		
		private string _Cassete;
		
		private long _Time_Code;
		
		private long _Duration;
		
		private EntityRef<Program> _Program;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(long value);
    partial void OnidChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnEditorIdChanging(int value);
    partial void OnEditorIdChanged();
    partial void OnNewsDateChanging(System.DateTime value);
    partial void OnNewsDateChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnNewsTimeChanging(long value);
    partial void OnNewsTimeChanged();
    partial void OnCalcTimeChanging(long value);
    partial void OnCalcTimeChanged();
    partial void OnTaskTimeChanging(long value);
    partial void OnTaskTimeChanged();
    partial void OnDeletedChanging(bool value);
    partial void OnDeletedChanged();
    partial void OnProgramIdChanging(int value);
    partial void OnProgramIdChanged();
    partial void OnCasseteChanging(string value);
    partial void OnCasseteChanged();
    partial void OnTime_CodeChanging(long value);
    partial void OnTime_CodeChanged();
    partial void OnDurationChanging(long value);
    partial void OnDurationChanged();
    #endregion
		
		public New()
		{
			this._Program = default(EntityRef<Program>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EditorId", DbType="Int NOT NULL")]
		public int EditorId
		{
			get
			{
				return this._EditorId;
			}
			set
			{
				if ((this._EditorId != value))
				{
					this.OnEditorIdChanging(value);
					this.SendPropertyChanging();
					this._EditorId = value;
					this.SendPropertyChanged("EditorId");
					this.OnEditorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NewsDate", DbType="DateTime NOT NULL")]
		public System.DateTime NewsDate
		{
			get
			{
				return this._NewsDate;
			}
			set
			{
				if ((this._NewsDate != value))
				{
					this.OnNewsDateChanging(value);
					this.SendPropertyChanging();
					this._NewsDate = value;
					this.SendPropertyChanged("NewsDate");
					this.OnNewsDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NewsTime", DbType="BigInt NOT NULL")]
		public long NewsTime
		{
			get
			{
				return this._NewsTime;
			}
			set
			{
				if ((this._NewsTime != value))
				{
					this.OnNewsTimeChanging(value);
					this.SendPropertyChanging();
					this._NewsTime = value;
					this.SendPropertyChanged("NewsTime");
					this.OnNewsTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CalcTime", DbType="BigInt NOT NULL")]
		public long CalcTime
		{
			get
			{
				return this._CalcTime;
			}
			set
			{
				if ((this._CalcTime != value))
				{
					this.OnCalcTimeChanging(value);
					this.SendPropertyChanging();
					this._CalcTime = value;
					this.SendPropertyChanged("CalcTime");
					this.OnCalcTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TaskTime", DbType="BigInt NOT NULL")]
		public long TaskTime
		{
			get
			{
				return this._TaskTime;
			}
			set
			{
				if ((this._TaskTime != value))
				{
					this.OnTaskTimeChanging(value);
					this.SendPropertyChanging();
					this._TaskTime = value;
					this.SendPropertyChanged("TaskTime");
					this.OnTaskTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Deleted", DbType="Bit NOT NULL")]
		public bool Deleted
		{
			get
			{
				return this._Deleted;
			}
			set
			{
				if ((this._Deleted != value))
				{
					this.OnDeletedChanging(value);
					this.SendPropertyChanging();
					this._Deleted = value;
					this.SendPropertyChanged("Deleted");
					this.OnDeletedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProgramId", DbType="Int NOT NULL")]
		public int ProgramId
		{
			get
			{
				return this._ProgramId;
			}
			set
			{
				if ((this._ProgramId != value))
				{
					if (this._Program.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnProgramIdChanging(value);
					this.SendPropertyChanging();
					this._ProgramId = value;
					this.SendPropertyChanged("ProgramId");
					this.OnProgramIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Cassete", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Cassete
		{
			get
			{
				return this._Cassete;
			}
			set
			{
				if ((this._Cassete != value))
				{
					this.OnCasseteChanging(value);
					this.SendPropertyChanging();
					this._Cassete = value;
					this.SendPropertyChanged("Cassete");
					this.OnCasseteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Time_Code", DbType="BigInt NOT NULL")]
		public long Time_Code
		{
			get
			{
				return this._Time_Code;
			}
			set
			{
				if ((this._Time_Code != value))
				{
					this.OnTime_CodeChanging(value);
					this.SendPropertyChanging();
					this._Time_Code = value;
					this.SendPropertyChanged("Time_Code");
					this.OnTime_CodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Duration", DbType="BigInt NOT NULL")]
		public long Duration
		{
			get
			{
				return this._Duration;
			}
			set
			{
				if ((this._Duration != value))
				{
					this.OnDurationChanging(value);
					this.SendPropertyChanging();
					this._Duration = value;
					this.SendPropertyChanged("Duration");
					this.OnDurationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Program_New", Storage="_Program", ThisKey="ProgramId", OtherKey="id", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public Program Program
		{
			get
			{
				return this._Program.Entity;
			}
			set
			{
				Program previousValue = this._Program.Entity;
				if (((previousValue != value) 
							|| (this._Program.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Program.Entity = null;
						previousValue.News.Remove(this);
					}
					this._Program.Entity = value;
					if ((value != null))
					{
						value.News.Add(this);
						this._ProgramId = value.id;
					}
					else
					{
						this._ProgramId = default(int);
					}
					this.SendPropertyChanged("Program");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Programs")]
	public partial class Program : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _Name;
		
		private long _Director;
		
		private bool _Rustv;
		
		private bool _Deleted;
		
		private EntitySet<New> _News;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDirectorChanging(long value);
    partial void OnDirectorChanged();
    partial void OnRustvChanging(bool value);
    partial void OnRustvChanged();
    partial void OnDeletedChanging(bool value);
    partial void OnDeletedChanged();
    #endregion
		
		public Program()
		{
			this._News = new EntitySet<New>(new Action<New>(this.attach_News), new Action<New>(this.detach_News));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Director", DbType="BigInt NOT NULL")]
		public long Director
		{
			get
			{
				return this._Director;
			}
			set
			{
				if ((this._Director != value))
				{
					this.OnDirectorChanging(value);
					this.SendPropertyChanging();
					this._Director = value;
					this.SendPropertyChanged("Director");
					this.OnDirectorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rustv", DbType="Bit NOT NULL")]
		public bool Rustv
		{
			get
			{
				return this._Rustv;
			}
			set
			{
				if ((this._Rustv != value))
				{
					this.OnRustvChanging(value);
					this.SendPropertyChanging();
					this._Rustv = value;
					this.SendPropertyChanged("Rustv");
					this.OnRustvChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Deleted", DbType="Bit NOT NULL")]
		public bool Deleted
		{
			get
			{
				return this._Deleted;
			}
			set
			{
				if ((this._Deleted != value))
				{
					this.OnDeletedChanging(value);
					this.SendPropertyChanging();
					this._Deleted = value;
					this.SendPropertyChanged("Deleted");
					this.OnDeletedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Program_New", Storage="_News", ThisKey="id", OtherKey="ProgramId")]
		public EntitySet<New> News
		{
			get
			{
				return this._News;
			}
			set
			{
				this._News.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_News(New entity)
		{
			this.SendPropertyChanging();
			entity.Program = this;
		}
		
		private void detach_News(New entity)
		{
			this.SendPropertyChanging();
			entity.Program = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.PrintTemplates")]
	public partial class PrintTemplate : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private string _news;
		
		private string _block;
		
		private string _block_flag;
		
		private string _depended_block;
		
		private string _description;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OnnewsChanging(string value);
    partial void OnnewsChanged();
    partial void OnblockChanging(string value);
    partial void OnblockChanged();
    partial void Onblock_flagChanging(string value);
    partial void Onblock_flagChanged();
    partial void Ondepended_blockChanging(string value);
    partial void Ondepended_blockChanged();
    partial void OndescriptionChanging(string value);
    partial void OndescriptionChanged();
    #endregion
		
		public PrintTemplate()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_news", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string news
		{
			get
			{
				return this._news;
			}
			set
			{
				if ((this._news != value))
				{
					this.OnnewsChanging(value);
					this.SendPropertyChanging();
					this._news = value;
					this.SendPropertyChanged("news");
					this.OnnewsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_block", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string block
		{
			get
			{
				return this._block;
			}
			set
			{
				if ((this._block != value))
				{
					this.OnblockChanging(value);
					this.SendPropertyChanging();
					this._block = value;
					this.SendPropertyChanged("block");
					this.OnblockChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_block_flag", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string block_flag
		{
			get
			{
				return this._block_flag;
			}
			set
			{
				if ((this._block_flag != value))
				{
					this.Onblock_flagChanging(value);
					this.SendPropertyChanging();
					this._block_flag = value;
					this.SendPropertyChanged("block_flag");
					this.Onblock_flagChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_depended_block", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string depended_block
		{
			get
			{
				return this._depended_block;
			}
			set
			{
				if ((this._depended_block != value))
				{
					this.Ondepended_blockChanging(value);
					this.SendPropertyChanging();
					this._depended_block = value;
					this.SendPropertyChanged("depended_block");
					this.Ondepended_blockChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_description", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string description
		{
			get
			{
				return this._description;
			}
			set
			{
				if ((this._description != value))
				{
					this.OndescriptionChanging(value);
					this.SendPropertyChanging();
					this._description = value;
					this.SendPropertyChanged("description");
					this.OndescriptionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.vWeb_BlockForPrintTemplate")]
	public partial class vWeb_BlockForPrintTemplate
	{
		
		private long _Id;
		
		private string _Name;
		
		private long _NewsId;
		
		private long _ParentId;
		
		private long _CreatorId;
		
		private long _OperatorId;
		
		private long _JockeyId;
		
		private long _BlockTime;
		
		private long _TaskTime;
		
		private long _CalcTime;
		
		private string _BlockText;
		
		private int _Sort;
		
		private string _Description;
		
		private bool _Ready;
		
		private bool _Approve;
		
		private int _BLockType;
		
		private string _TypeName;
		
		private int _CutterId;
		
		private string _TextLang1;
		
		private string _TextLang2;
		
		private string _TextLang3;
		
		public vWeb_BlockForPrintTemplate()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NewsId", DbType="BigInt NOT NULL")]
		public long NewsId
		{
			get
			{
				return this._NewsId;
			}
			set
			{
				if ((this._NewsId != value))
				{
					this._NewsId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentId", DbType="BigInt NOT NULL")]
		public long ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					this._ParentId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorId", DbType="BigInt NOT NULL")]
		public long CreatorId
		{
			get
			{
				return this._CreatorId;
			}
			set
			{
				if ((this._CreatorId != value))
				{
					this._CreatorId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OperatorId", DbType="BigInt NOT NULL")]
		public long OperatorId
		{
			get
			{
				return this._OperatorId;
			}
			set
			{
				if ((this._OperatorId != value))
				{
					this._OperatorId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JockeyId", DbType="BigInt NOT NULL")]
		public long JockeyId
		{
			get
			{
				return this._JockeyId;
			}
			set
			{
				if ((this._JockeyId != value))
				{
					this._JockeyId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BlockTime", DbType="BigInt NOT NULL")]
		public long BlockTime
		{
			get
			{
				return this._BlockTime;
			}
			set
			{
				if ((this._BlockTime != value))
				{
					this._BlockTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TaskTime", DbType="BigInt NOT NULL")]
		public long TaskTime
		{
			get
			{
				return this._TaskTime;
			}
			set
			{
				if ((this._TaskTime != value))
				{
					this._TaskTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CalcTime", DbType="BigInt NOT NULL")]
		public long CalcTime
		{
			get
			{
				return this._CalcTime;
			}
			set
			{
				if ((this._CalcTime != value))
				{
					this._CalcTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BlockText", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string BlockText
		{
			get
			{
				return this._BlockText;
			}
			set
			{
				if ((this._BlockText != value))
				{
					this._BlockText = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sort", DbType="Int NOT NULL")]
		public int Sort
		{
			get
			{
				return this._Sort;
			}
			set
			{
				if ((this._Sort != value))
				{
					this._Sort = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this._Description = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Ready", DbType="Bit NOT NULL")]
		public bool Ready
		{
			get
			{
				return this._Ready;
			}
			set
			{
				if ((this._Ready != value))
				{
					this._Ready = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Approve", DbType="Bit NOT NULL")]
		public bool Approve
		{
			get
			{
				return this._Approve;
			}
			set
			{
				if ((this._Approve != value))
				{
					this._Approve = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BLockType", DbType="Int NOT NULL")]
		public int BLockType
		{
			get
			{
				return this._BLockType;
			}
			set
			{
				if ((this._BLockType != value))
				{
					this._BLockType = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeName", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string TypeName
		{
			get
			{
				return this._TypeName;
			}
			set
			{
				if ((this._TypeName != value))
				{
					this._TypeName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CutterId", DbType="Int NOT NULL")]
		public int CutterId
		{
			get
			{
				return this._CutterId;
			}
			set
			{
				if ((this._CutterId != value))
				{
					this._CutterId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TextLang1", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string TextLang1
		{
			get
			{
				return this._TextLang1;
			}
			set
			{
				if ((this._TextLang1 != value))
				{
					this._TextLang1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TextLang2", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string TextLang2
		{
			get
			{
				return this._TextLang2;
			}
			set
			{
				if ((this._TextLang2 != value))
				{
					this._TextLang2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TextLang3", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string TextLang3
		{
			get
			{
				return this._TextLang3;
			}
			set
			{
				if ((this._TextLang3 != value))
				{
					this._TextLang3 = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TemplateVariables")]
	public partial class TemplateVariable : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _Name;
		
		private string _Description;
		
		private System.Nullable<int> _Depend;
		
		private System.Nullable<int> _Type;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnDependChanging(System.Nullable<int> value);
    partial void OnDependChanged();
    partial void OnTypeChanging(System.Nullable<int> value);
    partial void OnTypeChanged();
    #endregion
		
		public TemplateVariable()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(255)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Depend", DbType="Int")]
		public System.Nullable<int> Depend
		{
			get
			{
				return this._Depend;
			}
			set
			{
				if ((this._Depend != value))
				{
					this.OnDependChanging(value);
					this.SendPropertyChanging();
					this._Depend = value;
					this.SendPropertyChanged("Depend");
					this.OnDependChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="Int")]
		public System.Nullable<int> Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
