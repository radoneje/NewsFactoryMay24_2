﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adminSite.Models
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
	public partial class MainDataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public MainDataClassesDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["NewsFactoryConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _UserID;
		
		private string _UserName;
		
		private string _pass;
		
		private bool _Active;
		
		private System.DateTime _Last_time;
		
		private int _ReadRate;
		
		private bool _deleted;
		
		private int _PrintTemplateId;
		
		private int _BlockTypeId;
		
		private int _AbrigeBlockTypeId;
		
		private bool _OnlyMy;
		
		private bool _Enter;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIDChanging(int value);
    partial void OnUserIDChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnpassChanging(string value);
    partial void OnpassChanged();
    partial void OnActiveChanging(bool value);
    partial void OnActiveChanged();
    partial void OnLast_timeChanging(System.DateTime value);
    partial void OnLast_timeChanged();
    partial void OnReadRateChanging(int value);
    partial void OnReadRateChanged();
    partial void OndeletedChanging(bool value);
    partial void OndeletedChanged();
    partial void OnPrintTemplateIdChanging(int value);
    partial void OnPrintTemplateIdChanged();
    partial void OnBlockTypeIdChanging(int value);
    partial void OnBlockTypeIdChanged();
    partial void OnAbrigeBlockTypeIdChanging(int value);
    partial void OnAbrigeBlockTypeIdChanged();
    partial void OnOnlyMyChanging(bool value);
    partial void OnOnlyMyChanged();
    partial void OnEnterChanging(bool value);
    partial void OnEnterChanged();
    #endregion
		
		public User()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_pass", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string pass
		{
			get
			{
				return this._pass;
			}
			set
			{
				if ((this._pass != value))
				{
					this.OnpassChanging(value);
					this.SendPropertyChanging();
					this._pass = value;
					this.SendPropertyChanged("pass");
					this.OnpassChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Bit NOT NULL")]
		public bool Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Last_time", DbType="DateTime NOT NULL")]
		public System.DateTime Last_time
		{
			get
			{
				return this._Last_time;
			}
			set
			{
				if ((this._Last_time != value))
				{
					this.OnLast_timeChanging(value);
					this.SendPropertyChanging();
					this._Last_time = value;
					this.SendPropertyChanged("Last_time");
					this.OnLast_timeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReadRate", DbType="Int NOT NULL")]
		public int ReadRate
		{
			get
			{
				return this._ReadRate;
			}
			set
			{
				if ((this._ReadRate != value))
				{
					this.OnReadRateChanging(value);
					this.SendPropertyChanging();
					this._ReadRate = value;
					this.SendPropertyChanged("ReadRate");
					this.OnReadRateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_deleted", DbType="Bit NOT NULL")]
		public bool deleted
		{
			get
			{
				return this._deleted;
			}
			set
			{
				if ((this._deleted != value))
				{
					this.OndeletedChanging(value);
					this.SendPropertyChanging();
					this._deleted = value;
					this.SendPropertyChanged("deleted");
					this.OndeletedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PrintTemplateId", DbType="Int NOT NULL")]
		public int PrintTemplateId
		{
			get
			{
				return this._PrintTemplateId;
			}
			set
			{
				if ((this._PrintTemplateId != value))
				{
					this.OnPrintTemplateIdChanging(value);
					this.SendPropertyChanging();
					this._PrintTemplateId = value;
					this.SendPropertyChanged("PrintTemplateId");
					this.OnPrintTemplateIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BlockTypeId", DbType="Int NOT NULL")]
		public int BlockTypeId
		{
			get
			{
				return this._BlockTypeId;
			}
			set
			{
				if ((this._BlockTypeId != value))
				{
					this.OnBlockTypeIdChanging(value);
					this.SendPropertyChanging();
					this._BlockTypeId = value;
					this.SendPropertyChanged("BlockTypeId");
					this.OnBlockTypeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AbrigeBlockTypeId", DbType="Int NOT NULL")]
		public int AbrigeBlockTypeId
		{
			get
			{
				return this._AbrigeBlockTypeId;
			}
			set
			{
				if ((this._AbrigeBlockTypeId != value))
				{
					this.OnAbrigeBlockTypeIdChanging(value);
					this.SendPropertyChanging();
					this._AbrigeBlockTypeId = value;
					this.SendPropertyChanged("AbrigeBlockTypeId");
					this.OnAbrigeBlockTypeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OnlyMy", DbType="Bit NOT NULL")]
		public bool OnlyMy
		{
			get
			{
				return this._OnlyMy;
			}
			set
			{
				if ((this._OnlyMy != value))
				{
					this.OnOnlyMyChanging(value);
					this.SendPropertyChanging();
					this._OnlyMy = value;
					this.SendPropertyChanged("OnlyMy");
					this.OnOnlyMyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Enter", DbType="Bit NOT NULL")]
		public bool Enter
		{
			get
			{
				return this._Enter;
			}
			set
			{
				if ((this._Enter != value))
				{
					this.OnEnterChanging(value);
					this.SendPropertyChanging();
					this._Enter = value;
					this.SendPropertyChanged("Enter");
					this.OnEnterChanged();
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