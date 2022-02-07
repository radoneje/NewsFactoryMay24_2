﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace barryArchiveFixer
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
	public partial class mainClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void InsertArcNews(ArcNews instance);
    partial void UpdateArcNews(ArcNews instance);
    partial void DeleteArcNews(ArcNews instance);
    partial void InsertArchBlocks(ArchBlocks instance);
    partial void UpdateArchBlocks(ArchBlocks instance);
    partial void DeleteArchBlocks(ArchBlocks instance);
    #endregion
		
		public mainClassesDataContext() : 
				base(global::barryArchiveFixer.Properties.Settings.Default.NewsFactoryConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public mainClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public mainClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public mainClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public mainClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ArcNews> ArcNews
		{
			get
			{
				return this.GetTable<ArcNews>();
			}
		}
		
		public System.Data.Linq.Table<ArchBlocks> ArchBlocks
		{
			get
			{
				return this.GetTable<ArchBlocks>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ArcNews")]
	public partial class ArcNews : INotifyPropertyChanging, INotifyPropertyChanged
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
		
		private long _ProgramId;
		
		private string _Cassete;
		
		private long _Time_Code;
		
		private long _Duration;
		
		private System.Nullable<System.Guid> _extId;
		
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
    partial void OnProgramIdChanging(long value);
    partial void OnProgramIdChanged();
    partial void OnCasseteChanging(string value);
    partial void OnCasseteChanged();
    partial void OnTime_CodeChanging(long value);
    partial void OnTime_CodeChanged();
    partial void OnDurationChanging(long value);
    partial void OnDurationChanged();
    partial void OnextIdChanging(System.Nullable<System.Guid> value);
    partial void OnextIdChanged();
    #endregion
		
		public ArcNews()
		{
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProgramId", DbType="BigInt NOT NULL")]
		public long ProgramId
		{
			get
			{
				return this._ProgramId;
			}
			set
			{
				if ((this._ProgramId != value))
				{
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_extId", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> extId
		{
			get
			{
				return this._extId;
			}
			set
			{
				if ((this._extId != value))
				{
					this.OnextIdChanging(value);
					this.SendPropertyChanging();
					this._extId = value;
					this.SendPropertyChanged("extId");
					this.OnextIdChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ArchBlocks")]
	public partial class ArchBlocks : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private long _NewsId;
		
		private long _ParentId;
		
		private int _BLockType;
		
		private int _CreatorId;
		
		private int _OperatorId;
		
		private int _JockeyId;
		
		private int _CutterId;
		
		private int _BlockTime;
		
		private int _TaskTime;
		
		private int _CalcTime;
		
		private string _BlockText;
		
		private int _Sort;
		
		private string _Description;
		
		private bool _Approve;
		
		private bool _Ready;
		
		private bool _Deleted;
		
		private string _TextLang1;
		
		private string _TextLang2;
		
		private string _TextLang3;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnNewsIdChanging(long value);
    partial void OnNewsIdChanged();
    partial void OnParentIdChanging(long value);
    partial void OnParentIdChanged();
    partial void OnBLockTypeChanging(int value);
    partial void OnBLockTypeChanged();
    partial void OnCreatorIdChanging(int value);
    partial void OnCreatorIdChanged();
    partial void OnOperatorIdChanging(int value);
    partial void OnOperatorIdChanged();
    partial void OnJockeyIdChanging(int value);
    partial void OnJockeyIdChanged();
    partial void OnCutterIdChanging(int value);
    partial void OnCutterIdChanged();
    partial void OnBlockTimeChanging(int value);
    partial void OnBlockTimeChanged();
    partial void OnTaskTimeChanging(int value);
    partial void OnTaskTimeChanged();
    partial void OnCalcTimeChanging(int value);
    partial void OnCalcTimeChanged();
    partial void OnBlockTextChanging(string value);
    partial void OnBlockTextChanged();
    partial void OnSortChanging(int value);
    partial void OnSortChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnApproveChanging(bool value);
    partial void OnApproveChanged();
    partial void OnReadyChanging(bool value);
    partial void OnReadyChanged();
    partial void OnDeletedChanging(bool value);
    partial void OnDeletedChanged();
    partial void OnTextLang1Changing(string value);
    partial void OnTextLang1Changed();
    partial void OnTextLang2Changing(string value);
    partial void OnTextLang2Changed();
    partial void OnTextLang3Changing(string value);
    partial void OnTextLang3Changed();
    #endregion
		
		public ArchBlocks()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
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
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
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
					this.OnNewsIdChanging(value);
					this.SendPropertyChanging();
					this._NewsId = value;
					this.SendPropertyChanged("NewsId");
					this.OnNewsIdChanged();
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
					this.OnParentIdChanging(value);
					this.SendPropertyChanging();
					this._ParentId = value;
					this.SendPropertyChanged("ParentId");
					this.OnParentIdChanged();
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
					this.OnBLockTypeChanging(value);
					this.SendPropertyChanging();
					this._BLockType = value;
					this.SendPropertyChanged("BLockType");
					this.OnBLockTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorId", DbType="Int NOT NULL")]
		public int CreatorId
		{
			get
			{
				return this._CreatorId;
			}
			set
			{
				if ((this._CreatorId != value))
				{
					this.OnCreatorIdChanging(value);
					this.SendPropertyChanging();
					this._CreatorId = value;
					this.SendPropertyChanged("CreatorId");
					this.OnCreatorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OperatorId", DbType="Int NOT NULL")]
		public int OperatorId
		{
			get
			{
				return this._OperatorId;
			}
			set
			{
				if ((this._OperatorId != value))
				{
					this.OnOperatorIdChanging(value);
					this.SendPropertyChanging();
					this._OperatorId = value;
					this.SendPropertyChanged("OperatorId");
					this.OnOperatorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JockeyId", DbType="Int NOT NULL")]
		public int JockeyId
		{
			get
			{
				return this._JockeyId;
			}
			set
			{
				if ((this._JockeyId != value))
				{
					this.OnJockeyIdChanging(value);
					this.SendPropertyChanging();
					this._JockeyId = value;
					this.SendPropertyChanged("JockeyId");
					this.OnJockeyIdChanged();
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
					this.OnCutterIdChanging(value);
					this.SendPropertyChanging();
					this._CutterId = value;
					this.SendPropertyChanged("CutterId");
					this.OnCutterIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BlockTime", DbType="Int NOT NULL")]
		public int BlockTime
		{
			get
			{
				return this._BlockTime;
			}
			set
			{
				if ((this._BlockTime != value))
				{
					this.OnBlockTimeChanging(value);
					this.SendPropertyChanging();
					this._BlockTime = value;
					this.SendPropertyChanged("BlockTime");
					this.OnBlockTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TaskTime", DbType="Int NOT NULL")]
		public int TaskTime
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CalcTime", DbType="Int NOT NULL")]
		public int CalcTime
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
					this.OnBlockTextChanging(value);
					this.SendPropertyChanging();
					this._BlockText = value;
					this.SendPropertyChanged("BlockText");
					this.OnBlockTextChanged();
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
					this.OnSortChanging(value);
					this.SendPropertyChanging();
					this._Sort = value;
					this.SendPropertyChanged("Sort");
					this.OnSortChanged();
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
					this.OnApproveChanging(value);
					this.SendPropertyChanging();
					this._Approve = value;
					this.SendPropertyChanged("Approve");
					this.OnApproveChanged();
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
					this.OnReadyChanging(value);
					this.SendPropertyChanging();
					this._Ready = value;
					this.SendPropertyChanged("Ready");
					this.OnReadyChanged();
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
					this.OnTextLang1Changing(value);
					this.SendPropertyChanging();
					this._TextLang1 = value;
					this.SendPropertyChanged("TextLang1");
					this.OnTextLang1Changed();
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
					this.OnTextLang2Changing(value);
					this.SendPropertyChanging();
					this._TextLang2 = value;
					this.SendPropertyChanged("TextLang2");
					this.OnTextLang2Changed();
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
					this.OnTextLang3Changing(value);
					this.SendPropertyChanging();
					this._TextLang3 = value;
					this.SendPropertyChanged("TextLang3");
					this.OnTextLang3Changed();
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