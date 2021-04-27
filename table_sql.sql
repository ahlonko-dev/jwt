USE [dbUsers]
GO

/****** Object:  Table [dbo].[tUsers]    Script Date: 26-04-21 16:48:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Firstname] [nvarchar](100) NOT NULL,
	[Lastname] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Birthdate] [date] NOT NULL,
 CONSTRAINT [PK_tUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tUsers]  WITH CHECK ADD  CONSTRAINT [CK_tUsers_age] CHECK  ((datediff(year,[birthdate],getdate())>=(16)))
GO

ALTER TABLE [dbo].[tUsers] CHECK CONSTRAINT [CK_tUsers_age]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'min 16 ans' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tUsers', @level2type=N'CONSTRAINT',@level2name=N'CK_tUsers_age'
GO

