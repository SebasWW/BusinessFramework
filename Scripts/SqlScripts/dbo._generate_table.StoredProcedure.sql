-- =============================================
-- Create basic stored procedure template
-- =============================================

-- Drop stored procedure if it already exists

IF OBJECT_ID('dbo._generate_table', 'P') IS NOT NULL
	   DROP PROCEDURE dbo._generate_table
GO
-- _generate_table 'c_advert_channel_type'
CREATE PROCEDURE dbo._generate_table
	@name nvarchar(100)
AS
	DECLARE @sql as nvarchar(max)
	
	SELECT @sql = '
CREATE TABLE [dbo].[' + @name + '](
	[id]			[uniqueidentifier]	NOT NULL DEFAULT NEWSEQUENTIALID(),
	[nmb]			[int] IDENTITY(1,1)	NOT NULL,
	[owner_id]		[uniqueidentifier]	NOT NULL,
	[user_owner_id]	[uniqueidentifier]	NOT NULL,
	[archive]		[tinyint]			NOT NULL,
	--[name]			[nvarchar](512)	NOT NULL,
	--[note]			[nvarchar](MAX)	NULL,
 CONSTRAINT [PK_' + @name + '] PRIMARY KEY CLUSTERED 
(
	[nmb] ASC
)WITH (PAD_INDEX  = ON, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
'

	print  @sql
GO


