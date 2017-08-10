CREATE FUNCTION [Shop].CheckSpecValue(@valCondition nvarchar(200), @value nvarchar(50))
RETURNS BIT
AS 
BEGIN
Declare @greater float, @less float, @equal NVARCHAR(200)
IF PATINDEX('%[_]%',@valCondition) > 0
BEGIN
 DECLARE @lowStr NVARCHAR(200)
 SET @lowStr = SUBSTRING(@valCondition, 0, PATINDEX('%[_]%', @valCondition))
 IF LEN(@lowStr) > 0
 SELECT @greater = PARSE (@lowStr AS float)
 DECLARE @highStr NVARCHAR(200)
 SET @highStr = SUBSTRING(@valCondition, LEN(@lowStr + '_') + 1, LEN(@valCondition))
 IF LEN(@highStr) > 0
 SET @less = PARSE ( @highStr AS float)
END
ELSE
BEGIN
 SET @equal = @valCondition
END

DECLARE @res BIT
IF (@equal IS NOT NULL)
BEGIN
 IF (@value=@equal)
 SET @res = 1
 ELSE
 SET @res = 0
END
ELSE IF(ISNUMERIC(@value) != 1)
 SET @res=0
ELSE
BEGIN
 
 DECLARE @floatVal float
 SET @floatVal = PARSE (@value AS float)
 IF (@greater IS NOT NULL AND @less IS NULL)
 BEGIN
 IF @floatVal >= @greater
 SET @res = 1
 ELSE
 SET @res = 0
 END
 
 IF (@greater IS NULL AND @less IS NOT NULL)
 BEGIN
 IF @floatVal <= @less
 SET @res = 1
 ELSE
 SET @res = 0
 END

 IF (@greater IS NOT NULL AND @less IS NOT NULL)
 BEGIN
 IF @floatVal BETWEEN @greater AND @less
 SET @res = 1
 ELSE
 SET @res = 0
 END
END
RETURN @res
END