IF OBJECT_ID('[sm].[_sp_generate_secure_features]', 'P') IS NOT NULL
	DROP PROCEDURE [sm].[_sp_generate_secure_features]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

	[sm].[_sp_generate_secure_features]

*/

CREATE PROCEDURE [sm].[_sp_generate_secure_features]
as
	set nocount on
------------------------------------------------------------------------------------------------------
-- Блок проверки регистрации таблицы -- КОНЕЦ
------------------------------------------------------------------------------------------------------
	declare @lines table( line varchar(8000) )
------------------------------------------------------------------------------------------------------
-- Business collections - BEGIN
------------------------------------------------------------------------------------------------------
	insert @lines ( line )
					select	'using System;'
		union all 	select	''
		union all 	select	'namespace Tnomer.Remontica.Security'
		union all 	select	'{'
		union all 	select	'    public static class SecurityFeatures'
		union all 	select	'    {'
		union all 	select  * from (
						select	'        /// <summary>' + char(13) + char(10)
						+	'        ///	' + name + char(13) + char(10)
						+	'        /// </summary>' + char(13) + char(10)
						+	'        public const String ' + sm._fn_first_upper(sm._fn_to_camel_case(short_name)) + ' = "' + short_name + '";' + char(13) + char(10)  + char(13) + char(10) l
						from c_secure_feature a
			--order by short_name
			) t
			
		union all 	select	'    }'
		union all 	select	'}'


	declare c1 cursor local forward_only for
		select line from @lines

	open c1
	declare
		@line varchar(8000)
	fetch next from c1
		into @line
	while @@fetch_status = 0
	begin
		print @line
		fetch next from c1
			into @line
	end
	close c1
	deallocate c1

GO
	