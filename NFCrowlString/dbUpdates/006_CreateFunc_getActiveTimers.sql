

	
CREATE FUNCTION fSTR_getActiveTimers 
(
	-- Add the parameters for the function here
	@playListItemId nvarchar(90)
)
RETURNS bit
AS
BEGIN
	DECLARE @count int
	SELECT @count=COUNT(id) FROM dbo.tSTR_timers 
	WHERE itemId=@playListItemId 
	AND deleted=0 
	AND isEnd=1
	AND deteEnd>getDate()
	
	
	DECLARE @res bit
	SET @res=0
	IF @count>0
	BEGIN 
		SET @res=1
	END
	
	RETURN @res

END



