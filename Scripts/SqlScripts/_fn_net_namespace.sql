IF OBJECT_ID('[sm].[_fn_net_namespace]') IS NOT NULL
	DROP FUNCTION [sm].[_fn_net_namespace]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

 SELECT [sm].[_fn_net_namespace]('tT_ASD_DFG_1_')
 SELECT [sm].[_fn_net_namespace]('s_T_ASD_DFG_1_s')
 SELECT [sm].[_fn_net_namespace]('ASD_DFG_1_y')

 */
CREATE FUNCTION [sm].[_fn_net_namespace] 
(
	@table_name nvarchar(4000)
) 
returns nvarchar(4000) as
begin

	declare 
		@namespaceNet		nvarchar(255) =  
			case upper(left(@table_name, 2))
				when 'S_' then 'Administration' 
				when 'D_' then 'Dictionary'
				when 'C_' then 'Constant'
				when 'T_' then 'Working'
				else 'Wrong table prefix'
				end

	return @namespaceNet
end
GO
