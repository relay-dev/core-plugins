
CREATE PROCEDURE [dbo].[spDropTable] (@schemaName VARCHAR(255), @tableName VARCHAR(255))
AS
BEGIN

    DECLARE @SQL VARCHAR(MAX);

    IF OBJECT_ID(@schemaName + '.' + @tableName, 'U') IS NOT NULL
    BEGIN
        SET @SQL = 'DROP TABLE [' + @schemaName + '].[' + @tableName + ']'

        EXEC (@SQL);

        PRINT 'SUCCESS: Dropped Table [' + @schemaName + '].[' + @tableName + ']'
    END
    ELSE
    BEGIN
        PRINT 'INFO: Table [' + @schemaName + '].[' + @tableName + '] not found. Skipping Drop statement'
    END

END
