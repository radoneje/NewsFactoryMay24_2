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