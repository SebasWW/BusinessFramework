IF OBJECT_ID('[sm].[_sp_generate_biz_by_table]', 'P') IS NOT NULL
	DROP PROCEDURE [sm].[_sp_generate_biz_by_table]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
d_inventory_category_inventory_type
	sm._sp_generate_biz_by_table 't_order_room_work_group'

*/

CREATE PROCEDURE [sm].[_sp_generate_biz_by_table]
	@table_name nvarchar(100)
as
	set nocount on

	declare
		@name nvarchar(100) = substring(@table_name, 3, len(@table_name) -2),
		@prefix nvarchar(10) = left(@table_name, 2)

	declare
		@camel_case_name	nvarchar(255) = sm._fn_to_camel_case(@name)


	declare
		@upper_case_name	nvarchar(255) = sm._fn_first_upper(@camel_case_name)
	,	@prefixNet			nvarchar(255) = sm._fn_first_upper(sm._fn_to_camel_case(@prefix))	

	declare 
		@namespaceNet		nvarchar(255) =  
			case @prefixNet
				when 'S' then 'Administration' 
				when 'D' then 'Dictionary'
				when 'C' then 'Constant'
				when 'T' then 'Working'
				else 'Wrong table prefix'
				end
------------------------------------------------------------------------------------------------------
-- проверки регистрации таблицы
------------------------------------------------------------------------------------------------------
	--SELECT @table_id = id FROM s_Table WHERE [name] = @table_name

	--IF @table_id IS NULL BEGIN
	--	PRINT '<row id="' + cast(newid() as nvarchar(100)) + '" name="' + @table_name + '" caption="!!!Описание таблицы!!!" is_mergeable="0" for_surcharge="0" archive="0" is_main_table_of_frame="1" proc_name="up_' + @name + '" view_name="v' + @table_name + '" />>'
	--	EXEC upu_raise_error  @error_code = 300000, @message_marker = 'Таблица не зарегистрирована в s_Table'
	--	RETURN
	--END
------------------------------------------------------------------------------------------------------
-- Collect columns
------------------------------------------------------------------------------------------------------
	declare @t table ( n int, name nvarchar(100), fulltype nvarchar(200), type nvarchar(100), isnullable int )
	insert @t (n, name, fulltype, type, isnullable)
	select 
		row_number() over(order by (select null)),
		col.name,
		tp.name,-- + case when tp.name in ('char', 'nvarchar', 'nchar', 'nvarchar') then '(' + ltrim(str(col.length)) + ')' else '' end, 
		tp.name,
		col.isnullable
	from 
		[syscolumns] col
		join [sysobjects] obj on col.id = obj.id
		join [systypes] tp on tp.xusertype = col.xusertype
	where obj.name = @table_name

	declare @t_children table(name nvarchar(200), camel_case_name nvarchar(200), upper_case_name nvarchar(200),upper_table_name nvarchar(200))
	insert @t_children
	select 
		t.name,
		camel_case_name = sm._fn_to_camel_case(substring(t.name, 3, len(t.name) -2)),
		upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(t.name, 3, len(t.name) -2))),
		upper_table_name = sm._fn_first_upper(sm._fn_to_camel_case(t.name))
	from
		sys.tables t
		join sys.foreign_keys fk on fk.parent_object_id = t.object_id
		join sys.foreign_key_columns fkc on fkc.constraint_object_id = fk.object_id
		join sys.all_columns c on c.column_id = fkc.parent_column_id AND c.object_id = fkc.parent_object_id
		join sys.all_columns  refC on refC.column_id = fkc.referenced_column_id AND refC.object_id = fkc.referenced_object_id
		join sys.tables refT on reft.object_id = refC.object_id
	where
		refT.name = @table_name	

	declare @t_parent table(name nvarchar(200), camel_case_name nvarchar(200), upper_case_name nvarchar(200),upper_table_name nvarchar(200))
	insert @t_parent
	select 
		t.name,
		camel_case_name = sm._fn_to_camel_case(substring(refT.name, 3, len(refT.name) -2)),
		upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(refT.name, 3, len(refT.name) -2))),
		upper_table_name = sm._fn_first_upper(sm._fn_to_camel_case(refT.name))
	from
		sys.tables t
		join sys.foreign_keys fk on fk.parent_object_id = t.object_id
		join sys.foreign_key_columns fkc on fkc.constraint_object_id = fk.object_id
		join sys.all_columns c on c.column_id = fkc.parent_column_id AND c.object_id = fkc.parent_object_id
		join sys.all_columns  refC on refC.column_id = fkc.referenced_column_id AND refC.object_id = fkc.referenced_object_id
		join sys.tables refT on reft.object_id = refC.object_id
	where
		t.name = @table_name	

	declare @maxn int
	select @maxn = max( n ) from @t
------------------------------------------------------------------------------------------------------
-- Блок проверки регистрации таблицы -- КОНЕЦ
------------------------------------------------------------------------------------------------------
	declare @lines table( line nvarchar(max), id int identity(1,1) )

	insert @lines ( line )
					select '
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Collection.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Tnomer.BusinessFramework;
using Tnomer.Remontica.EntityFramework;
using Tnomer.Remontica.Factory.' + @namespaceNet + ';

namespace Tnomer.Remontica.Entity.' + @namespaceNet + '
{
    public class ' + @upper_case_name + 'Collection : RemonticaCollection<' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>
    {
        internal ' + @upper_case_name + 'Collection(BusinessManager businessManager, ICollection<' + @prefixNet + @upper_case_name + '> entrySet)
             : base(businessManager, entrySet, t => t.Id, ' + @upper_case_name + 'Factory.Current) { }
    }

    public class ' + @upper_case_name + 'ReadOnlyCollection : RemonticaReadOnlyCollection<' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>
    {
        internal ' + @upper_case_name + 'ReadOnlyCollection(BusinessManager businessManager, ICollection<' + @prefixNet + @upper_case_name + '> entrySet)
             : base(businessManager, entrySet, t => t.Id, ' + @upper_case_name + 'Factory.Current) { }
    }
}
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + '.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Tnomer.BusinessFramework;
using Tnomer.Remontica.EntityFramework;
using Tnomer.Remontica.Factory.Administration;
using Tnomer.Remontica.Factory.Dictionary;
using Tnomer.Remontica.Factory.Constant;
using Tnomer.Remontica.Factory.Working;

namespace Tnomer.Remontica.Entity.' + @namespaceNet + '
{	
    public sealed class ' + @upper_case_name + case when @prefixNet ='D' then  ' : DictionaryBaseObject' else  ' : 
		RemonticaObject' end + '<' + @prefixNet + @upper_case_name + '>,
		IRemovable, IConsistencyValidator
    {
        internal ' + @upper_case_name + '(BusinessManager businessManager, ' + @prefixNet + @upper_case_name + ' entry)
            : base(businessManager, entry) { }

        internal ' + @upper_case_name + '( ' + @prefixNet + @upper_case_name + ' entry)
            : base(entry) { }

        public ' + @upper_case_name + '() : base( new ' + @prefixNet + @upper_case_name + '()){ Entry.SArchive = 0; }

        // ************************************
        // Свойства	
        // *************************************
        public Int32 Id { get => Entry.Id; }
		public Boolean IsDeleted { get => Entry.SArchive == 1; }'
		union all 	select
'		public ' + sm._fn_to_dotnet_type(fulltype) + case when isnullable = 1 then '?' else '' end + ' ' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + '{get => Entry.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + '; set => Entry.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ' = value; }
'
					from 
						@t 
					where 
						-- не обрабатываемые столбцы
						name not in ('id', 's_archive', 's_version')
------------------------------------------------------------------------------------------------------
-- Parent relation objects
------------------------------------------------------------------------------------------------------
union all 	select '
		// *************************************	
		// Родителькие связанные объекты
		// *************************************'
		union all 	select	' 
		// ' + q.name + '
		' + upper_case_name + ' _' + camel_case_name + ';
		public ' + upper_case_name + ' ' + upper_case_name + '
		{
		    get
		    {
				if (Entry.' + upper_case_name + ' == null) return null;

		        lock (objLock)
		        {
		            if (_' + camel_case_name + ' == null)
		                _' + camel_case_name + ' =
							(BusinessManager as RemonticaManager).CreateObject(' + upper_case_name + 'Factory.Current, Entry.' + upper_case_name + ');
		        }

		        return _' + camel_case_name + ';
		    }
		}
'
					from
						@t_parent q

		union all 	select	'
		// *************************************	
		// Дочерние связанные коллекции	
		// *************************************	'
		union all 	select	'
		// ' + q.name + '
		' + upper_case_name + 'Collection _' + [sm].[_fn_many_suffix](camel_case_name) + ';
		public ' + upper_case_name + 'Collection ' + [sm].[_fn_many_suffix](upper_case_name ) + '
		{
			get	
			{
               lock (objLock)
				{
					if (_' + [sm].[_fn_many_suffix](camel_case_name ) +' == null)
						_' + [sm].[_fn_many_suffix](camel_case_name) + ' =
							new ' + upper_case_name + 'Collection(BusinessManager, Entry.' + upper_table_name + ');
				}
				return _' + [sm].[_fn_many_suffix](camel_case_name ) +';
			}
		}
'
					from
						@t_children q						
  		union all	select	'
        // *************************************	
        // Checks
        // ************************************
        IQueryable<ConsistencyErrorEntry> IConsistencyValidator.CheckAsQuery(bool beforeUpdate)
        {
            IQueryable<ConsistencyErrorEntry> q, query = null;

			// shared checks ....

			// pre/post update checks
            if (beforeUpdate)
            {
                // ...
            }
            else
            {'
  		union all	select	
'				if (Entry.SArchive == 1)
                {
                    q = ((BusinessManager as RemonticaManager).DbContext as RemonticaContext).' + upper_table_name + '
                        .Where(t => t.' + @upper_case_name + 'Id == Id && t.SArchive == 0)
                        .Select(t => new ConsistencyErrorEntry(Id, "ERROR:' + upper(@table_name) + ':DELETE:' + upper(name) + '_IS_EXISTS"
                            , "Существуют неудалённые ' + upper(name) + '."));

                    query = query?.Union(q) ?? q;
                }
'
					from
						@t_children q

		union all 	select	
'			}

            return query;
        }
		// ************************************
		// Set manager
		// ************************************
  		protected override void OnBusinessManagerChange()
		{'
  		union all	select	
'			' + sm._fn_first_upper([sm].[_fn_many_suffix](camel_case_name )) + '.BusinessManager = BusinessManager;'
					from
						@t_children q
  		union all	select	
'		}
		
		// *************************************	
		// Methods
		// ************************************
        public void Remove()
        {
            OnRemove();
        }

		protected override void OnRemove()
		{'
		union all	select	
'			' + sm._fn_first_upper( [sm].[_fn_many_suffix](camel_case_name )) + '.Clear();'
					from
						@t_children q
  		union all	select	
'
			Entry.SArchive = 1;
		}
	}	
}	
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Factory.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Tnomer.Remontica.Entity.' + @namespaceNet + ';
using Tnomer.BusinessFramework;
using Tnomer.BusinessFramework.Factory;
using Tnomer.Remontica.EntityFramework;
namespace Tnomer.Remontica.Factory.' + @namespaceNet + '
{	
	public class ' + @upper_case_name + 'Factory : RemonticaObjectFactory<' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>	
	{
		protected override ' + @upper_case_name + ' OnCreateInstance(BusinessManager manager, ' + @prefixNet + @upper_case_name + ' entry)	
		{
			return new ' + @upper_case_name + '(manager, entry);
		}

		static Lazy<' + @upper_case_name + 'Factory> _factory = new Lazy<' + @upper_case_name + 'Factory>(true);
		internal static ' + @upper_case_name + 'Factory Current { get => _factory.Value; }	
	}	

   public class ' + @upper_case_name + 'CollectionFactory : RemonticaCollectionFactory<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>	
	{
		protected override ' + @upper_case_name + 'Collection OnCreateInstance(BusinessManager manager, ICollection<' + @prefixNet + @upper_case_name + '> entries)	
		{
			return new ' + @upper_case_name + 'Collection(manager, entries);
		}

		protected override ' + @upper_case_name + 'ReadOnlyCollection OnCreateReadOnlyInstance(BusinessManager manager, ICollection<' + @prefixNet + @upper_case_name + '> entries)	
		{
			return new ' + @upper_case_name + 'ReadOnlyCollection(manager, entries);
		}

		static Lazy<' + @upper_case_name + 'CollectionFactory> _factory = new Lazy<' + @upper_case_name + 'CollectionFactory>(true);	
		internal static ' + @upper_case_name + 'CollectionFactory Current { get => _factory.Value; }
	}
}
//**********************************************
// ' + @upper_case_name + '
//**********************************************
        public RemonticaQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '> ' + [sm].[_fn_many_suffix](@upper_case_name )+ '
        {
            get => new RemonticaQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>(
                new RemonticaQueryContext<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + '>(
                    this,
                    ' + @upper_case_name + 'Factory.Current,
                    ' + @upper_case_name + 'CollectionFactory.Current
                )
                , DatabaseContext.Set<' + @prefixNet + @upper_case_name + '>());
        }
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Model.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tnomer.Remontica.Api.' + @namespaceNet + '
{	
    public class ' + @upper_case_name + 'Model
    {
		[JsonProperty(PropertyName = "id")]
		public Int32 Id { get; set; }'
		union all 	select	'
		' + case when isnullable = 1 then '//	' else '' end + '[Required(AllowEmptyStrings = false, ErrorMessage = "Error! Parameter [' + sm._fn_to_camel_case(name) + '] is required.")]
		[JsonProperty(PropertyName = "' + sm._fn_to_camel_case(name) + '")]
		public ' + sm._fn_to_dotnet_type(fulltype) + case when isnullable = 1  and sm._fn_to_dotnet_type(fulltype) != 'String' then '?' else '' end +  ' ' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + '{ get; set; }
		'
					from 
						@t f
					where 
						-- не обрабатываемые столбцы
						name not in ('id', 's_archive', 's_version')


		union all 	select	'
		//' + q.upper_case_name + '
		[JsonProperty(PropertyName = "' + q.camel_case_name + '")]
		public ' + q.upper_case_name + 'Model ' + q.upper_case_name + ' { get; set; }'
					from
						@t_parent q

		union all 	select	
'
		//' + q.upper_case_name  +	'
		[JsonProperty(PropertyName = "' + [sm].[_fn_many_suffix](q.camel_case_name )+ '", NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<' + q.upper_case_name + 'Model> ' + [sm].[_fn_many_suffix](q.upper_case_name )+ ' { get; set; }' 
					from
						@t_children q						

		union all 	select 
'	}

	public class ' + @upper_case_name + 'ResponseModel
	{	
		[JsonProperty(PropertyName = "' + @camel_case_name + '")]
		public ' + @upper_case_name + 'Model ' + @upper_case_name + ' { get; set; }
	}

	public class ' + @upper_case_name + 'ListResponseModel
	{
		[JsonProperty(PropertyName = "' + [sm].[_fn_many_suffix](@camel_case_name) + '")]
		public IEnumerable<' + @upper_case_name + 'Model> ' + [sm].[_fn_many_suffix](@upper_case_name ) + ' { get; set; }
	}
}
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Parameters.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Tnomer.Remontica.Api.' + @namespaceNet + '
{
    public class ' + @upper_case_name + 'Parameters : SearchParams
    {
		public ' + @upper_case_name + 'Parameters(){ }
    	public ' + @upper_case_name + 'Parameters(String searchText) : base(searchText) { }
		public ' + @upper_case_name + 'Parameters(ConcurrentDictionary<String, Object> paramsList) :base(paramsList){ }

'
		union all	select	
'		private const string _' + lower(name) + ' = "filter_eq_' + lower(name) + '";
		public ' + sm._fn_to_dotnet_type(fulltype) 
			+ case when + sm._fn_to_dotnet_type(fulltype) = 'String' then '' else '?' end + ' ' 
			+ sm._fn_first_upper(sm._fn_to_camel_case(name)) +  ' { get => GetValue(_' + lower(name) + ') as ' + sm._fn_to_dotnet_type(fulltype) + case when + sm._fn_to_dotnet_type(fulltype) = 'String' then '' else '?' end + '; set => SetValue(_' + lower(name) + ', value); }

'
			from @t
			where 
				left(name,2) <> 's_'
				and 
				name <> 'id'

		union all	select	
'		
		public const string Include_' + q.upper_case_name + '_Key = "' + lower(q.upper_case_name) + '";
        public Boolean Include_' + q.upper_case_name + '
        {
            get => IncludeGet(Include_' + q.upper_case_name + '_Key);
            set => IncludeSet(Include_' + q.upper_case_name + '_Key, value);
        }
'
			from @t_parent q

		union all select
'
        public const string Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + '_Key = "' + [sm].[_fn_many_suffix](lower(q.upper_case_name)) + '";
        public Boolean Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + '
        {
            get => IncludeGet(Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + '_Key);
            set => IncludeSet(Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + '_Key, value);
        }'
			from @t_children q

		union all select

'
        public IEnumerable<String> GetIncludes()
        {
            var col = new Collection<String>();
'
		union all	select	
'			if (Include_' + q.upper_case_name + ') col.Add(Include_' + q.upper_case_name + '_Key);'
			from @t_parent q

		union all select
'			if (Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + ') col.Add(Include_' + [sm].[_fn_many_suffix](q.upper_case_name) + '_Key);'
			from @t_children q

		union all select

'
			// custom...


            return col.ToArray();
        }

		// custom...
    }
}	
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Extension.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Tnomer.Remontica.Entity.' + @namespaceNet + ';

namespace Tnomer.Remontica.Api.' + @namespaceNet + '
{
	public static class ' + @upper_case_name + 'Extension
	{
		public static ' + @upper_case_name + 'Model To' + @upper_case_name + 'Model(this ' + @upper_case_name + ' obj, IEnumerable<String> includes)
		{
			IEnumerable<String> arr;

			var model = new ' + @upper_case_name + 'Model();
		'

		union all 	select	
'			model.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ' = obj.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ';'+ char(13) + char(10) 
					from 
						@t 
					where 
						-- не обрабатываемые столбцы
						name not in ('s_archive', 's_version')

		union all 	select	
'
			// collections' 
		union all 	select	
'			arr = includes.Where(t => t.StartsWith(' + @upper_case_name + 'Parameters.Include_' +[sm].[_fn_many_suffix]( q.upper_case_name ) + '_Key + "_") || t == ' + @upper_case_name + 'Parameters.Include_' +[sm].[_fn_many_suffix]( q.upper_case_name ) + '_Key)
                .Select(t => ' + @upper_case_name + 'Parameters.Include_' +[sm].[_fn_many_suffix]( q.upper_case_name ) + '_Key.Length == t.Length ? "" : t.Substring(' + @upper_case_name + 'Parameters.Include_' +[sm].[_fn_many_suffix]( q.upper_case_name ) + '_Key.Length + 1));
            if (arr.Count() > 0)
            {
                model.' +[sm].[_fn_many_suffix]( q.upper_case_name ) + ' = obj.' +[sm].[_fn_many_suffix]( q.upper_case_name ) + '?.ToModelArray(t => t.To' + q.upper_case_name + 'Model(arr));
            }				

'
					from
						@t_children q

		union all 	select 
'			// parent' 
		union all 	select	
'			arr = includes.Where(t => t.StartsWith(' + @upper_case_name + 'Parameters.Include_' + q.upper_case_name + '_Key + "_") || t == ' + @upper_case_name + 'Parameters.Include_' + q.upper_case_name + '_Key)
                .Select(t =>' + @upper_case_name + 'Parameters.Include_' + q.upper_case_name + '_Key.Length == t.Length ? "" :  t.Substring(' + @upper_case_name + 'Parameters.Include_' + q.upper_case_name + '_Key.Length + 1));
            if (arr.Count() > 0)
            {
                model.' + q.upper_case_name + ' = obj.' + q.upper_case_name + '?.To' + q.upper_case_name + 'Model(arr);
            }

'
					from
						@t_parent q
		union all 	select	
'			// custom...
			
			return model;
		}

		public static void Update(this ' + @upper_case_name + ' obj, ' + @upper_case_name + 'Model model)
		{'
		union all 	select	''
						+	'			obj.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ' = model.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ';'+ char(13) + char(10) 
					from 
						@t 
					where 
						-- не обрабатываемые столбцы
						name not in ('id', 's_archive', 's_version')

		union all 	select	'
			if (model.' + [sm].[_fn_many_suffix](q.upper_case_name ) + ' != null)
			{
				obj.' + [sm].[_fn_many_suffix](q.upper_case_name ) + '.Merge
				(
					model.' + [sm].[_fn_many_suffix](q.upper_case_name ) + ',
					(m, o) => m.Id == o.Id,
					(m) => m.ToObject(),
					(m, o) => o.Update(m),
					(m) => (m.Id == 0)
				);
			}'
					from
						@t_children q

		union all 	select 
'		}

		public static ' + @upper_case_name + ' ToObject(this ' + @upper_case_name + 'Model model)
		{
			var obj = new ' + @upper_case_name + '();

			Update(obj, model);

			return obj;
		}
	}
}'
		union all 	select	'
------------------------------------------------------------------------------------------------------
-- // ' + @upper_case_name + 'Controller.cs
------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tnomer.AspNetCore.Mvc;
using Tnomer.AspNetCore.Mvc.Models;
using Tnomer.BusinessFramework;
using Tnomer.Data.Common.Repository;
using Tnomer.Remontica.Api.Controllers;
using Tnomer.Remontica.Authentification;
using Tnomer.Remontica.Entity.Administration;
using Tnomer.Remontica.Entity.Constant;
using Tnomer.Remontica.Entity.Dictionary;
using Tnomer.Remontica.Entity.Working;
using Tnomer.Remontica.EntityFramework;


namespace Tnomer.Remontica.Api.' + @namespaceNet + '
{	
    [Produces("application/json")]
    [Route("api/' + lower(@namespaceNet) + '/' + @camel_case_name + '")]
    public class ' + @upper_case_name + 'Controller : PrivateController
    {	
		//Parameters parsing;
        private ' + @upper_case_name + 'Parameters _requestParams;
        public ' + @upper_case_name + 'Parameters GetRequestParameters() 
        {
            lock (objLock)
            {
				if(_requestParams == null)
					_requestParams = RepositoryRequestParams.GetParameters<' + @upper_case_name + 'Parameters>(Request.QueryString.Value ?? "");
            }

            return _requestParams ;
        }
        private BusinessQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + ', Int32>
            Include(BusinessQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + ', Int32> query)	
		{	'
		union all	select	
'			if (GetRequestParameters().Include_' + upper_case_name + ')
				query = query.Include(t => t.' + upper_case_name + ');'
					from
						@t_parent q	

		union all	select	
'			if (GetRequestParameters().Include_' + [sm].[_fn_many_suffix]( upper_case_name) + ')
				query = query.Include(t => t.' + upper(left(q.name, 1)) + upper_case_name + ');'
					from
						@t_children q						
		union all select '
			//custom...

			return query;	
		}

		private BusinessQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + ', Int32>
			OrderBy(BusinessQuery<' + @upper_case_name + 'Collection, ' + @upper_case_name + 'ReadOnlyCollection, ' + @upper_case_name + ', ' + @prefixNet + @upper_case_name + ', Int32> query)	
		{
            foreach( var k in OrderedResources)
            {
				var o = k.ToLower();
                if (o == "id")
                    query = query.OrderBy(t => t.Id);
                else if (o == "-id")
                    query = query.OrderByDescending(t => t.Id);'
		union all	select	
'				else if (o == "' + lower(sm._fn_to_camel_case(name)) + '") 
					query = query.OrderBy(t => t.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ');
				else if (o == "-' + lower(sm._fn_to_camel_case(name)) + '")
					query = query.OrderByDescending(t => t.' + sm._fn_first_upper(sm._fn_to_camel_case(name)) + ');
'
					from
						@t 
					where left(name,2) <> 's_' and name <> 'id'

		union all	select	
'			}
			return query;	
       }

		// GET: api/' + @upper_case_name + '
		[HttpGet]
		public async Task<IActionResult> GetList
		('
		union all	select	
'			[FromQuery] ' + sm._fn_to_dotnet_type(x.type) + '? filter_eq_' + sm._fn_to_camel_case(x.name) + ','
					from
						@t x
					where
						right(x.name, 3) = '_id'
		union all	select	
'			[FromQuery] String filter_like_name,
            [FromQuery] String filter_ids
		)	
       {
			try
			{	
				//Auth
                AuthentificatedUser user = await SignInAsync();	

                //manager
                using (RemonticaManager manager = RemonticaManager(user))
                {
					//get query	
                    var query = manager.' + [sm].[_fn_many_suffix](@upper_case_name )+ '.Where(t => t.SArchive == 0);

                    //************************************
                    // LINQ block	
                    //************************************
                    if (!String.IsNullOrEmpty(filter_ids))
                    {
                       var ids = filter_ids.Split(new Char[] { '','' }, StringSplitOptions.RemoveEmptyEntries)
                            .SelectMany(t => Int32.TryParse(t, out int i) ? new Int32[] { i } : new Int32[] { });

                        query = query.Where(t => ids.Contains(t.Id));
                    }
					'
		union all	select	
'					if (filter_eq_' + sm._fn_to_camel_case(x.name) + '.HasValue)
                        query = query.Where(t => t.' + sm._fn_first_upper(sm._fn_to_camel_case(x.name) ) + ' == filter_eq_' + sm._fn_to_camel_case(x.name) + ');
'
					from
						@t x
					where
						right(x.name, 3) = '_id'
		union all	select	'
                    if (!String.IsNullOrEmpty(filter_like_name))
                        query = query.Where(t => EF.Functions.Like(t.Name, "%" + filter_like_name + "%"));

                    // order
                    query = OrderBy(query);

                    // paging	
                    query = SetQueryPaging(query);

                    // include	
                    query = Include(query);

                    //************************************
                    // LINQ	
                    //************************************

                    //get list	
                    ' + @upper_case_name + 'ReadOnlyCollection collection = await query.ToReadOnlyCollectionAsync();

                    //return model	
                    var result = new ResponseModel<' + @upper_case_name + 'ListResponseModel>	
                        (	
                           new ' + @upper_case_name + 'ListResponseModel()	
                           {
                               ' + [sm].[_fn_many_suffix](@upper_case_name )+ ' = collection.ToModelArray(t => t.To' + @upper_case_name + 'Model(GetRequestParameters().GetIncludes()))	
                           }
						);

					return new OkObjectResult(result);
                }
            }
            catch (Exception e)	
            {
                return ErrorToActionResult(e);
            }
        }

        // GET: api/' + @upper_case_name + '/5	
        [HttpGet("{id}")]	
        public async Task<IActionResult> GetItem([FromRoute] int id)	
        {
           try	
           {	
				//Model validation	
				if (!ModelState.IsValid) throw new ModelValidationException(ModelState);

	            //Auth
				AuthentificatedUser user = await SignInAsync();

                //manager
                using (RemonticaManager manager = RemonticaManager(user))	
                {
                    //get list
                    ' + @upper_case_name + 'ReadOnlyCollection collection = await Include(manager.' + [sm].[_fn_many_suffix](@upper_case_name ) + '.Where(t => t.Id == id && t.SArchive == 0)).ToReadOnlyCollectionAsync();	

                    //not found	
                    if (collection.Count != 1) throw new NotFoundException();	

                    //return model
                    return new OkObjectResult(	
                        new ResponseModel<' + @upper_case_name + 'ResponseModel>
                        (
                           new ' + @upper_case_name + 'ResponseModel()
                           {
                               ' + @upper_case_name + ' = collection.Values.First().To' + @upper_case_name + 'Model(GetRequestParameters().GetIncludes())
                           }
                        )
                    );
                }
            }
            catch (Exception e)	
            {
                return ErrorToActionResult(e);
            }
        }'

		union all select'
       // PUT: api/' + @upper_case_name + '/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ' + @upper_case_name + 'Model item)	
        {
            //catch (DbUpdateConcurrencyException)
            try	
            {
                //Model validation
                if (!ModelState.IsValid) throw new ModelValidationException(ModelState);

                //Auth	
                AuthentificatedUser user = await SignInAsync();

                //manager	
                using (RemonticaManager manager = RemonticaManager(user))
                {
					// query
                    var query = manager.' + [sm].[_fn_many_suffix](@upper_case_name )+ '.Where(t => t.Id == id && t.SArchive == 0);

                    //includes
' 
		union all	select	
'					if (item.' + [sm].[_fn_many_suffix]( upper_case_name )+ '  != null) query = query.Include(t => t.' + upper_table_name + ');'
					from
						@t_children q		

	union all select '
                    //get list	
                    ' + @upper_case_name + 'Collection collection = await query.ToCollectionAsync();

                    //not found	
                    if (collection.Count != 1) throw new NotFoundException();

                    //get object
                    ' + @upper_case_name + ' obj = collection.Values.First();

                    //update object
                    obj.Update(item);

                    //save database
                    await manager.SaveChangesAsync();

                    //return Ok
					return new OkObjectResult(GetOkModel());
                }
			}
            catch (Exception e)
            {
                return ErrorToActionResult(e);
            }
        }

        // POST: api/' + @upper_case_name + '
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] ' + @upper_case_name + 'Model item)
        {	
            try	
            {
                //Model validation
                if (!ModelState.IsValid) throw new ModelValidationException(ModelState);

                //Auth
                AuthentificatedUser user = await SignInAsync();	

                //manager
                using (RemonticaManager manager = RemonticaManager(user))	
                {
                    //update object	
                    ' + @upper_case_name + ' obj = item.ToObject();

                    //add to manager
                    ' + @upper_case_name + 'Collection.Add(manager, obj);

                    //save database
                    await manager.SaveChangesAsync();

                    //return model
                    return new OkObjectResult(	
                        new ResponseModel<' + @upper_case_name + 'ResponseModel>
                        (
                           new ' + @upper_case_name + 'ResponseModel()
                           {
                               ' + @upper_case_name + ' = obj.To' + @upper_case_name + 'Model(GetRequestParameters().GetIncludes())
                           }
                        )
                    );
                }
			}
            catch (Exception e)	
            {
                return ErrorToActionResult(e);
            }
        }

        // DELETE: api/' + @upper_case_name + '/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)	
		{
            try
            {
                // Model validation
                if (!ModelState.IsValid) throw new ModelValidationException(ModelState);

                // Auth
                AuthentificatedUser user = await SignInAsync();

                // manager
                using (RemonticaManager manager = RemonticaManager(user))	
                {
                    // get model
					var query =  manager.' +[sm].[_fn_many_suffix]( @upper_case_name)+ '.Where(t => t.Id == id && t.SArchive == 0);

' 
		union all	select	
'					query = query.Include(t => t.' + upper_table_name + ');'
					from
						@t_children q		

	union all select '
					' + @upper_case_name + 'Collection collection = await query.ToCollectionAsync();

                    // not found
                    if (collection.Count != 1) throw new NotFoundException();

                    // remove object
                    collection.Remove(collection.Values.First());

                    // save database
                    await manager.SaveChangesAsync();

                    // ok-result
                    return new OkObjectResult(GetOkModel());
                }
            }
            catch (Exception e)	
            {
                return ErrorToActionResult(e);
            }
        }
    }	
}
'
	union all select '
        //-----------------------------------------------------------------------------------------------------
        // ' + @upper_case_name + '
        //-----------------------------------------------------------------------------------------------------
        IRepository<' + @upper_case_name + 'Model, ResponseModel<' + @upper_case_name + 'ResponseModel>, ResponseModel<' + @upper_case_name + 'ListResponseModel>> _' + @camel_case_name + ';
        public IRepository<' + @upper_case_name + 'Model, ResponseModel<' + @upper_case_name + 'ResponseModel>, ResponseModel<' + @upper_case_name + 'ListResponseModel>> ' + @upper_case_name + '
        {
            get
            {
                _' + @camel_case_name + ' = _' + @camel_case_name + ' ?? new RestRepository<' + @upper_case_name + 'Model, ResponseModel<' + @upper_case_name + 'ResponseModel>, ResponseModel<' + @upper_case_name + 'ListResponseModel>>
                (
                    _config,
                    ServiceResources.' + @prefixNet + '_' + @upper_case_name + '
                );

                return _' + @camel_case_name + ';
            }
        }


'

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
	