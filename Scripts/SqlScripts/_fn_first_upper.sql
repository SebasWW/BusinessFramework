IF OBJECT_ID('[sm].[_fn_first_upper]') IS NOT NULL
	DROP FUNCTION [sm].[_fn_first_upper]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SELECT [sm].[_fn_first_upper] (' tT_ASD_DFG_1_')
CREATE FUNCTION [sm].[_fn_first_upper] 
(
	@name nvarchar(4000)
) 
returns nvarchar(4000) as
begin

	select @name = ltrim(rtrim(@name))
	select @name = upper(left(@name, 1)) + substring(@name, 2, len(@name) -1)
	
	return @name
end
GO
