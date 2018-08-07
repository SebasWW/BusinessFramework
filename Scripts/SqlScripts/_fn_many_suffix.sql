IF OBJECT_ID('[sm].[_fn_many_suffix]') IS NOT NULL
	DROP FUNCTION [sm].[_fn_many_suffix]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

 SELECT [sm].[_fn_many_suffix]('tT_ASD_DFG_1_')
 SELECT [sm].[_fn_many_suffix]('tT_ASD_DFG_1_s')
 SELECT [sm].[_fn_many_suffix]('tT_ASD_DFG_1_y')

 */
CREATE FUNCTION [sm].[_fn_many_suffix] 
(
	@name nvarchar(4000)
) 
returns nvarchar(4000) as
begin

	declare @new_name nvarchar(255) 

	if right(@name, 1) = 's' 
		select @new_name = @name + 'es'

	ELSE IF right(@name, 1) = 'y' 
		select @new_name = left(@name, len(@name) - 1) + 'ies'

	ELSE
		select @new_name = @name + 's'
	
	return @new_name
end
GO
