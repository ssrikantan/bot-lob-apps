USE [dxfy17metrictracker]
GO

/****** Object:  Table [dbo].[CrmCustomerData]    Script Date: 10-09-2016 19:46:31 ******/
DROP TABLE [dbo].[CrmCustomerData]
GO

/****** Object:  Table [dbo].[CrmCustomerData]    Script Date: 10-09-2016 19:46:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CrmCustomerData](
	[CustomerId] [nvarchar](50) NOT NULL,
	[MicrosoftId] [nvarchar](50) NULL,
	[SlackId] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_CrmCustomerData] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


