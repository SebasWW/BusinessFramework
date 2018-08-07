
	with cte_columns as (
		SELECT 
			t.name table_name
		,	c.name column_name 
		,	(
				select top 1 t_ref.name from sys.tables t_ref 
				where t_ref.[type] = 'U' 
					and t_ref.name like ( '_[_]' + reverse(substring(reverse(c.name), charindex('_', reverse(c.name)) + 1, len(c.name))) )
			 ) ref_table_name
		,	reverse(substring(reverse(c.name), charindex('_', reverse(c.name)) + 1, len(c.name))) table_name_without_prefix
		,	reverse(left(reverse(c.name), charindex('_', reverse(c.name)) - 1)) ref_column
		,	case -- проверка существования индекса по колонке 
				when EXISTS(
					select 
						null 
					from 
						sys.indexes i
						inner join sys.index_columns ic on ic.index_id = i.index_id and ic.object_id = i.object_id and ic.is_included_column = 0
						inner join sys.columns clmn on clmn.column_id = ic.column_id and clmn.object_id = ic.object_id
					where 1 = 1
						and i.object_id = t.object_id
						and clmn.name = c.name 
				)
				then 1
				else 0
			end as hasIndex
		FROM
			sys.tables t 
			inner join sys.columns c on 1 = 1
				and c.object_id = t.object_id			
		WHERE 1 = 1  
			and t.[type] = 'U'
			and ((c.name like '%[_]id') or (c.name like '%[_]nmb'))
			and not exists (
				SELECT 
					null 
				FROM 
					sys.foreign_key_columns fk
				WHERE 1 = 1
					and fk.parent_object_id = c.object_id
					and fk.parent_column_id = c.column_id			
			)
	)
	SELECT 
		cc.table_name
	,	cc.column_name	
	,	case
			when cc.ref_table_name is not null -- таблица найдена
			then 
				'alter table [dbo].[' + cc.table_name + ']  with check add  constraint [fk_' + cc.table_name + '_' + cc.ref_table_name + '] foreign key([' + cc.column_name + '])
references [dbo].[' + cc.ref_table_name + '] ([' + cc.ref_column + '])
alter table [dbo].[' + cc.table_name + '] check constraint [fk_' + cc.table_name + '_' + cc.ref_table_name +  ']'
			else -- таблица не найдена. Подставить шаблон
				'alter table [dbo].[' + cc.table_name + ']  with check add  constraint [fk_' + cc.table_name + '_<name_table, ,' + cc.table_name_without_prefix + '>] foreign key([' + cc.column_name + '])
references [dbo].[<name_table, ,' + cc.table_name_without_prefix + '>] ([' + cc.ref_column + '])
alter table [dbo].[' + cc.table_name+ '] check constraint [fk_' + cc.table_name + '_<name_table, ,' + cc.table_name_without_prefix + '>]'
				
		end fk_sql_create_script
	,	case cc.hasIndex
			when 0 then 
				'create nonclustered index [idx_' + cc.table_name + '__' + cc.column_name + '] on [dbo].[' + cc.table_name + '] (' + cc.column_name + ' asc)'
			else null 
		end index_sql_create_script
	FROM 
		cte_columns cc		
	WHERE
		cc.column_name NOT IN (
				'owner_id',
				's_changed_user_id',
				's_created_user_id',
				'user_owner_id',
				'object_id',
				'object_nmb'

			)
	ORDER BY 
		cc.table_name
	,	cc.column_name	

