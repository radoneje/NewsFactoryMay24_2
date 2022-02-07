

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tWeb_GraphicsLayers]') AND type in (N'U'))

	BEGIN
	CREATE TABLE [dbo].[tWeb_GraphicsLayers](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[title] [nvarchar](max) NOT NULL,
		[deleted] [bit] NOT NULL,
		[dateCreation] [datetime] NOT NULL,
	 CONSTRAINT [PK_tWeb_GraphicsLayers] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tWeb_GraphicsLayers] ADD  CONSTRAINT [DF_tWeb_GraphicsLayers_deleted]  DEFAULT ((0)) FOR [deleted]
	ALTER TABLE [dbo].[tWeb_GraphicsLayers] ADD  CONSTRAINT [DF_tWeb_GraphicsLayers_dateCreation]  DEFAULT (getdate()) FOR [dateCreation]
	INSERT INTO tWeb_GraphicsLayers(title) VALUES('DEFAULT')

END



IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tWeb_mediaGraphic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWeb_mediaGraphic](
	[id] [nvarchar](90) NOT NULL,
	[layerId] [int] NOT NULL,
	[MediaId] [bigint] NOT NULL,
 CONSTRAINT [PK_tWeb_mediaGraphic] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tWeb_MediaGraphicsItems]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[tWeb_MediaGraphicsItems](
	[id] [nvarchar](90) NOT NULL,
	[mediaGraphicsId] [nvarchar](90) NOT NULL,
	[timeInSec] [float] NOT NULL,
	[text] [ntext] NOT NULL,
 CONSTRAINT [PK_tWeb_MediaGraphicsItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


BEGIN TRANSACTION

ALTER TABLE dbo.tWeb_GraphicsLayers SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.tWeb_mediaGraphic ADD CONSTRAINT
	FK_tWeb_mediaGraphic_tWeb_GraphicsLayers FOREIGN KEY
	(
	layerId
	) REFERENCES dbo.tWeb_GraphicsLayers
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.tWeb_mediaGraphic SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.tWeb_MediaGraphicsItems ADD CONSTRAINT
	FK_tWeb_MediaGraphicsItems_tWeb_mediaGraphic FOREIGN KEY
	(
	mediaGraphicsId
	) REFERENCES dbo.tWeb_mediaGraphic
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.tWeb_MediaGraphicsItems SET (LOCK_ESCALATION = TABLE)

COMMIT

END







