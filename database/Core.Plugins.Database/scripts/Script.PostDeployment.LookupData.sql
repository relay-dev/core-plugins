/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

USE [Core]
GO

DECLARE
  @createdBy       AS VARCHAR(100)        = 'sferguson'
, @createdDate     AS DATETIME            = CURRENT_TIMESTAMP

BEGIN

/*
 *  ###########################################################
 *    INSERT - CustomerStatus
 *  ###########################################################
 */

IF NOT EXISTS (SELECT 1 FROM [dbo].[Organization] WHERE [Name] = 'Admin')
BEGIN
    INSERT INTO [dbo].[Organization] ([Name], [Description], [Code], [DisplayName], [DisplayOrder], [IsActive], [CreatedBy], [CreatedDate])
    VALUES ('Admin', 'Administration account', 'ADMN', 'Admin', 1, 1, @createdBy, @createdDate)
END

END