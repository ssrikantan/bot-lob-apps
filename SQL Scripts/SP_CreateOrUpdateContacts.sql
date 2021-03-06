USE [dxfy17metrictracker]
GO
/****** Object:  StoredProcedure [dbo].[createOrUpdateContacts]    Script Date: 10-09-2016 19:47:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[createOrUpdateContacts] 
	-- Add the parameters for the stored procedure here
	@CustomerId varchar(50),
	@MicrosoftId varchar(50),
	@SlackId varchar(50),
	@Address varchar(500),
	@FirstName varchar(50),
	@LastName varchar(50),
	@Phone varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @vals int
	--SET @vals  = SELECT COUNT(*) FROM DBO.FY17ISVEngagementTracker WHERE ID = @ID
	--print @vals

	If(SELECT COUNT(*) FROM DBO.CrmCustomerData WHERE CustomerId = @CustomerId) > 0
	Begin
	UPDATE DBO.CrmCustomerData SET
	CustomerId=@CustomerId,
	MicrosoftId=@MicrosoftId,
	SlackId=@SlackId,
	Address=@Address,
	FirstName=@FirstName,
	LastName=@LastName,
	Phone=@Phone


	WHERE CustomerId = @CustomerId
	End
	
	Else 
	Begin
    -- Insert statements for procedure here
	INSERT INTO DBO.CrmCustomerData 
	(
		CustomerId,
		MicrosoftId,
		SlackId,
		[Address],
		FirstName,
		LastName,
		Phone
	)
	VALUES
	(
		@CustomerId	,
		@MicrosoftId,
		@SlackId	,
		@Address	,
		@FirstName	,
		@LastName	,
		@Phone	
	)
	End
End



