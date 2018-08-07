-- =============================================
-- Create basic stored procedure template
-- =============================================
--
---- Drop stored procedure if it already exists
--IF EXISTS (
--  SELECT * 
--    FROM INFORMATION_SCHEMA.ROUTINES 
--   WHERE SPECIFIC_SCHEMA = N'dbo'
--     AND SPECIFIC_NAME = N'_generate_unusable_indexes' 
--)
--   DROP PROCEDURE dbo._generate_unusable_indexes
--GO
--
--CREATE PROCEDURE dbo._generate_unusable_indexes
--AS


	SELECT 

			'DROP INDEX [' + ind.name  + '] ON [' + db.name + '].[dbo].[' + OBJECT_NAME(ind.object_id) + ']' sql_text,
			OBJECT_NAME(ind.object_id)	[object_name],
			ind.name					[ind_name],
			sum(isnull(user_seeks,0))	user_seeks,
			sum(isnull(user_scans,0))	user_scans,
			sum(isnull(user_lookups,0))	user_lookups,
			sum(isnull(user_updates,0))	user_updates
	--		ind.index_id, ind.type_desc, ind.is_unique , scol.name [column_name]
	--		, iv.*--user_seeks, iv.user_scans, iv.user_lookups, iv.user_updates
		FROM 
			sys.indexes ind 
			--join sys.index_columns				col		on ind.object_id = col.object_id	and ind.index_id = col.index_id
			--join sys.columns						scol	on col.object_id = scol.object_id	and col.column_id = scol.column_id
			left join sys.dm_db_index_usage_stats	iv		on ind.object_id = iv.object_id		and ind.index_id = iv.index_id and iv.database_id = db_id()
			left join sys.databases					db		ON db.database_id = iv.database_id
		WHERE 
			db_id() = iv.database_id
			AND 
			lower(left(OBJECT_NAME(ind.object_id),3)) <> 'sys'
			AND lower(left(ind.name,3)) <> 'pk_'
		GROUP BY
			db.name ,
			OBJECT_NAME(ind.object_id),
			ind.name
--		HAVING
--			--неиспользуемые
--			(
--				sum(isnull(user_seeks,0)) <15
--				AND
--				sum(isnull(user_scans,0)) <15
--			)
----			-- превышающие затраты/использование
--			OR
--			sum(isnull(user_updates,0)) > sum(isnull(user_seeks,0)) + sum(isnull(user_scans,0)) + sum(isnull(user_lookups,0))

		ORDER BY 
			--2,
			sum(isnull(user_seeks,0)),
			sum(isnull(user_scans,0)),
			sum(isnull(user_updates,0)) desc,
			sum(isnull(user_lookups,0)) desc

