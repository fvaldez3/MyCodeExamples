ALTER   PROC [dbo].[Locations_SelectByGeo]
				@Radius int
			   ,@Latitude float
			   ,@Longitude float

AS

/*---- TEST CODE ----
 -- radius defined in miles, so 100 is 100 miles
Declare @Radius int = 100
		,@Latitude float = 79.9
		,@Longitude float = 79.9

EXECUTE dbo.Locations_SelectByGeo
				@Radius
				,@Latitude
				,@Longitude


---- END TEST CODE ----
*/

BEGIN


	DECLARE @point geography = geography::Point(@Latitude, @Longitude, 4326);


	WITH g AS (
	SELECT 
	[Id]
	,'POINT(' + CAST([Longitude] AS VARCHAR(10)) + ' ' + CAST([Latitude] AS VARCHAR(10)) + ')' 
	as geography
	FROM  dbo.Locations
							)
	SELECT
			l.[Id]
			,lt.[Id]
			,lt.[Name]
			,l.[LineOne]
			,l.[LineTwo]
			,l.[City]
			,l.[Zip]
			,s.[Id]
			,s.[Name]
			,s.[Code]
			,l.[Latitude]
			,l.[Longitude]
				,g.geography
	FROM g
	INNER JOIN [dbo].[Locations] as l
	on g.Id = l.Id
	INNER JOIN [dbo].[LocationTypes] as lt
	on l.[LocationTypeId] = lt.[Id]
	INNER JOIN [dbo].[States] as s
	on l.[StateId] = s.[Id]
	WHERE  @Point.STDistance(geography) <= 1609.344 * @Radius		
			

END