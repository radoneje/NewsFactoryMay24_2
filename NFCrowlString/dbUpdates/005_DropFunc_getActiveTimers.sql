

  IF EXISTS (SELECT * FROM sys.objects  
WHERE name=('fSTR_getActiveTimers') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION fSTR_getActiveTimers
  END
