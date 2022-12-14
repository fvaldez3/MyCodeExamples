ALTER proc [dbo].[Licenses_UpdateV2]
		@Id int
		,@LicenseStateId int
		,@LicenseTypeId int
		,@LicenseNumber varchar(50)
		,@DateExpires datetime
		,@FileId int
		,@ValidationTypeId int 
		,@ValidatedBy int
		,@RejectMessage nvarchar(4000) = null
		

as
/*---Test Code---

	Declare 
		@LicenseStateId int = 8
		,@LicenseTypeId int = 2
		,@LicenseNumber varchar(50) = '777jcj'
		,@DateExpires date = '2026-01-01'
		,@FileId int = 1106
		,@ValidationTypeId int = 3
		,@ValidatedBy int = 4
		,@RejectMessage nvarchar(4000) = 'test: photo blurry'
		,@Id int = 52

	EXECUTE [dbo].[Licenses_Select_ByIdV2]
		@Id

	Execute [dbo].[Licenses_UpdateV2]
		 @Id 
		,@LicenseStateId 
		,@LicenseTypeId 
		,@LicenseNumber 
		,@DateExpires 
		,@FileId 
		,@ValidationTypeId 
		,@ValidatedBy
		,@RejectMessage

	EXECUTE [dbo].[Licenses_Select_ByIdV2]
		@Id

*/




--Runs if just a regular edit
IF (@ValidationTypeId = 1)
BEGIN
	UPDATE [dbo].[Licenses]
		SET [LicenseStateId] = @LicenseStateId
		,[LicenseTypeId] = @LicenseTypeId
		,[LicenseNumber] = @LicenseNumber
		,[DateExpires] = @DateExpires
		,[FileId] = @FileId
	WHERE Id = @Id
END
--Runs if Validation was accepted or rejected. 
ELSE 
BEGIN 
	UPDATE [dbo].[Licenses]
		SET
		[ValidationTypeId] = @ValidationTypeId
		,[ValidatedBy] = @ValidatedBy
		,[RejectMessage] = @RejectMessage
	WHERE Id = @Id
END
