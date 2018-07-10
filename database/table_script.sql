USE [PhoneDirectoryDB]
GO

/****** Object:  Table [dbo].[UserDetails]    Script Date: 7/10/2018 11:35:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserDetails](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](48) NOT NULL,
	[LastName] [varchar](48) NOT NULL,
	[Email] [nvarchar](96) NULL,
	[Phone] [varchar](16) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_UserDetails_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


