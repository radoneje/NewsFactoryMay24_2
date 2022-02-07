
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tSTR_playlist]') AND type in (N'U'))

	BEGIN
CREATE TABLE [dbo].[tSTR_playlist](
	[id] [nvarchar](90) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[deleted] [bit] NOT NULL,
 CONSTRAINT [PK_tSTR_playlist] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[tSTR_items](
	[id] [nvarchar](90) NOT NULL,
	[playlistId] [nvarchar](90) NOT NULL,
	[text] [ntext] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isAlert] [bit] NOT NULL,
	[dateAdd] [datetime] NOT NULL,
	[dateModify] [datetime] NOT NULL,
	[deleted] [bit] NOT NULL,
	[sortOrder] [INT] NOT NULL,
 CONSTRAINT [PK_tSTR_items] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE dbo.tSTR_playlist SET (LOCK_ESCALATION = TABLE)

ALTER TABLE dbo.tSTR_items ADD CONSTRAINT
	FK_tSTR_items_tSTR_playlist FOREIGN KEY
	(
	playlistId
	) REFERENCES dbo.tSTR_playlist
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.tSTR_items SET (LOCK_ESCALATION = TABLE)

INSERT INTO dbo.tSTR_playlist(id, deleted, title) VALUES('1', 0, 'NEW ITEM'   )
END



