IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tSTR_timers]') AND type in (N'U'))

	BEGIN
CREATE TABLE dbo.tSTR_timers
	(
	id nvarchar(90) NOT NULL,
	itemId nvarchar(90) NOT NULL,
	dateStart datetime NOT NULL,
	deteEnd datetime NOT NULL,
	dateInsert datetime NOT NULL,
	isEnd bit NOT NULL,
	isDaily bit NOT NULL,
	deleted bit NOT NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.tSTR_timers ADD CONSTRAINT
	PK_tSTR_timers PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE dbo.tSTR_timers ADD CONSTRAINT
	FK_tSTR_timers_tSTR_items FOREIGN KEY
	(
	itemId
	) REFERENCES dbo.tSTR_items
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

END