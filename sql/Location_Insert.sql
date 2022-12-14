ALTER   PROC [dbo].[Locations_Insert]
			@LocationTypeId int
			,@LineOne nvarchar(255)
			,@LineTwo nvarchar(255) = null
			,@City nvarchar(255)
			,@Zip nvarchar(50) = null
			,@StateId int
			,@Latitude float
			,@Longitude float
			,@UserId int
			,@Id int OUTPUT

AS

/*---- TEST CODE ----

DECLARE 
			@LocationTypeId int = 4
			,@LineOne nvarchar(255) = '123 S Double Ln'
			,@LineTwo nvarchar(255) = 'suite 1'
			,@City nvarchar(255) = 'San Diego'
			,@Zip nvarchar(50) = '0000'
			,@StateId int = 5
			,@Latitude float = 0
			,@Longitude float = 0
			,@UserId int = 1
			,@Id int = 0

EXECUTE dbo.Locations_Insert
			@LocationTypeId 
			,@LineOne 
			,@LineTwo 
			,@City 
			,@Zip 
			,@StateId 
			,@Latitude 
			,@Longitude 
			,@UserId
			,@Id OUTPUT

EXECUTE [dbo].[Locations_SelectById]
			@Id
			select * from dbo.locations

---- END TEST CODE ----
*/

BEGIN

	INSERT INTO [dbo].[Locations]
			([LocationTypeId]
			,[LineOne]
			,[LineTwo]
			,[City]
			,[Zip]
			,[StateId]
			,[Latitude]
			,[Longitude]
			,[CreatedBy])
	VALUES
			(@LocationTypeId
			,@LineOne
			,@LineTwo
			,@City
			,@Zip
			,@StateId
			,@Latitude
			,@Longitude
			,@UserId)

	SET @Id = SCOPE_IDENTITY()

END