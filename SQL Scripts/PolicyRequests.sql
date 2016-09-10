USE [dxfy17metrictracker]
GO

/****** Object:  Table [dbo].[PolicyRequests]    Script Date: 10-09-2016 19:47:26 ******/
DROP TABLE [dbo].[PolicyRequests]
GO

/****** Object:  Table [dbo].[PolicyRequests]    Script Date: 10-09-2016 19:47:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PolicyRequests](
	[PolicyRequestId] [nvarchar](50) NOT NULL,
	[CustomerId] [nvarchar](50) NULL,
	[PolicyType] [nvarchar](50) NULL,
	[ApplicationDate] [date] NULL,
	[PolicyStatus] [nvarchar](50) NULL,
	[InsuredIdentifier] [nvarchar](50) NULL,
	[AssessedValue] [decimal](18, 0) NULL,
	[Currency] [nvarchar](50) NULL,
	[PolicyStartDate] [date] NULL,
	[PolicyExpiryDate] [date] NULL,
	[PolicyId] [nvarchar](50) NULL,
	[RefDocumentIds] [nvarchar](50) NULL,
 CONSTRAINT [PK_PolicyRequests] PRIMARY KEY CLUSTERED 
(
	[PolicyRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


