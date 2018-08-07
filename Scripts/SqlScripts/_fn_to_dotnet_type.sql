IF OBJECT_ID('[sm].[_fn_to_dotnet_type]') IS NOT NULL
	DROP FUNCTION [sm].[_fn_to_dotnet_type]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SELECT [sm].[_fn_to_dotnet_type] ('T_ASD_DFG_1')
CREATE    function [sm].[_fn_to_dotnet_type] 
(
	@name nvarchar(4000)
) 
returns nvarchar(4000) as
begin

	select @name = 
		case lower(@name)
			when 'bit' then 'Boolean'
			when 'tinyint' then 'Byte'
			when 'int' then 'Int32'
			when 'bigint' then 'Int64'

			when 'datetime' then 'DateTime'

			when 'uniqueidentifier' then 'Guid'

			when 'money' then 'Decimal'
			when 'decimal' then 'Decimal'
			when 'numeric' then 'Decimal'

			when 'single' then 'Double'
			when 'float' then 'Double'
			
			when 'nvarchar' then 'String'
			when 'nchar' then 'String'
			when 'varchar' then 'String'
			when 'char' then 'String'
			else 'Unknown sql type. [' + @name + ']. Correct _fn_to_dotnet_type sql-function.'
			end

	return @name
end
GO
