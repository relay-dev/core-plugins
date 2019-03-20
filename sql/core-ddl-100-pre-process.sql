PRINT 'Running DDL Pre-Process...'
GO

/*
 *  ###########################################################
 *    Create the database
 *  ###########################################################
 */

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'Core')
BEGIN
    DROP DATABASE [Core]
END
GO

CREATE DATABASE [Core]
GO

ALTER DATABASE [Core] SET READ_COMMITTED_SNAPSHOT ON;
GO

USE [Core]
GO


/*
 *  ###########################################################
 *    Create a procedure to assist with the build script
 *  ###########################################################
 */

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
GO

PRINT 'Complete'
GO