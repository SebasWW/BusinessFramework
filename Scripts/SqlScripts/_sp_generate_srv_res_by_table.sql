IF OBJECT_ID('[sm].[_sp_generate_srv_res_by_table]', 'P') IS NOT NULL
	DROP PROCEDURE [sm].[_sp_generate_srv_res_by_table]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

	sm._sp_generate_srv_res_by_table 

*/

CREATE PROCEDURE [sm].[_sp_generate_srv_res_by_table]
as
	set nocount on

	declare @x table( x nvarchar(max), id int identity(1,1) )
	insert @x(x)
	select  x = 
'		public const String ' + ref + '_' + upper_case_name + ' = "' + lower(ref) + '/' + lower(upper_case_name) + '";'
			from 
			(
				select 
					name,
					upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(name, 3, len(name) -2))),
					ref = 
						case left(name, 2) 
							when 's_' then 'Administration' 
							when 'd_' then 'Dictionary'
							when 'c_' then 'Constant'
							when 't_' then 'Working'
							end
				from sys.tables 
				where 
					left(name, 2) in ('c_', 'd_', 's_', 't_')
					and name not in('s_log' ,'s_table')
					and name not like ('%integration%')

			) t
			Order by ref + '_' + upper_case_name


	declare @y table( x nvarchar(max), id int identity(1,1) )
	insert @y(x)
	select  x = 
'		//-----------------------------------------------------------------------------------------------------
        // ' + upper_case_name + '
        //-----------------------------------------------------------------------------------------------------
        IRepository<' + upper_case_name + 'Model, ResponseModel<' + upper_case_name + 'ResponseModel>, ResponseModel<' + upper_case_name + 'ListResponseModel>> _' + camel_case_name + ';
        public IRepository<' + upper_case_name + 'Model, ResponseModel<' + upper_case_name + 'ResponseModel>, ResponseModel<' + upper_case_name + 'ListResponseModel>> ' + upper_case_name + '
        {
            get
            {
                _' + camel_case_name + ' = _' + camel_case_name + ' ?? _factory.Create' + upper_case_name + 'Repository();;

                return _' + camel_case_name + ';
            }
        }
'
			from 
			(
				select 
					name,
					upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(name, 3, len(name) -2))),
					camel_case_name = lower(sm._fn_to_camel_case(substring(name, 3, len(name) -2))),
					ref = 
						case left(name, 2) 
							when 's_' then 'Administration' 
							when 'd_' then 'Dictionary'
							when 'c_' then 'Constant'
							when 't_' then 'Working'
							end
				from sys.tables 
				where 
					left(name, 2) in ('c_', 'd_', 's_', 't_')
					and name not in('s_log' ,'s_table')
					and name not like ('%integration%')				
			) t
			Order by ref + '_' + upper_case_name


	declare @z table( x nvarchar(max), id int identity(1,1) )
	insert @z(x)
	select  x = 
'		//-----------------------------------------------------------------------------------------------------
        // ' + upper_case_name + 'Factory
        //-----------------------------------------------------------------------------------------------------
        public IRepository<' + upper_case_name + 'Model, ResponseModel<' + upper_case_name + 'ResponseModel>, ResponseModel<' + upper_case_name + 'ListResponseModel>> Create' + upper_case_name + 'Repository()
        {
            return new RestRepository<' + upper_case_name + 'Model, ResponseModel<' + upper_case_name + 'ResponseModel>, ResponseModel<' + upper_case_name + 'ListResponseModel>>
                (
                    _config,
                    ServiceResources.' + ref + '_' + upper_case_name + '
                );
        }
'
			from 
			(
				select 
					name,
					upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(name, 3, len(name) -2))),
					camel_case_name = lower(sm._fn_to_camel_case(substring(name, 3, len(name) -2))),
					ref = 
						case left(name, 2) 
							when 's_' then 'Administration' 
							when 'd_' then 'Dictionary'
							when 'c_' then 'Constant'
							when 't_' then 'Working'
							end
				from sys.tables 
				where 
					left(name, 2) in ('c_', 'd_', 's_', 't_')
					and name not in('s_log' ,'s_table')
					and name not like ('%integration%')
			) t
			Order by ref + '_' + upper_case_name


	declare @lines table( line nvarchar(max), id int identity(1,1) )
	insert @lines ( line )
					select 
'using System;

namespace Tnomer.Remontica.Api
{
    public class ServiceResources
    {
'
union all 	select x
			from 
			@x t

union all 	select '

    }
}

'
union all 	select x
			from 
			@y t

union all 	select x
			from 
			@z t


	declare c1 cursor local forward_only for
		select line from @lines order by id

	open c1
	declare
		@line nvarchar(max)
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
	