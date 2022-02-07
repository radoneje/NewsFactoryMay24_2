IF EXISTS (SELECT * FROM sys.objects  
WHERE name=('fSTR_getPassiveTimers') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
  DROP FUNCTION fSTR_getPassiveTimers
  END
