---- =============================================
---- Create basic stored procedure template
---- =============================================
--
---- Drop stored procedure if it already exists
--IF EXISTS (
--  SELECT * 
--    FROM INFORMATION_SCHEMA.ROUTINES 
--   WHERE SPECIFIC_SCHEMA = N'dbo'
--     AND SPECIFIC_NAME = N'_generate_missing_indexes' 
--)
--   DROP PROCEDURE dbo._generate_missing_indexes
--GO
--
--CREATE PROCEDURE dbo._generate_missing_indexes
--AS
--SELECT 
--		--TOP 100 
--		'CREATE NONCLUSTERED INDEX idx_' + object_name(c.object_id) 
--		+ replace(replace(replace(replace(isnull('_' + c.equality_columns,'') + isnull('_' + c.inequality_columns,''), '[',''), ']',''), ',','_'),' ' ,'')
--		+ replace(replace(replace(replace(isnull('_INCLUDE_' + c.included_columns,''), '[',''), ']',''), ',','_'),' ' ,'')
--		-- + '_' + left(cast(newid() as varchar(500)),3) + char(10)
--		+ ' on ' + object_name(c.object_id)
--		+ '(' + 
--		case	
--			when c.equality_columns is not null and c.inequality_columns is not null then c.equality_columns + ',' + c.inequality_columns
--			when c.equality_columns is not null and c.inequality_columns is null then c.equality_columns
--			when c.inequality_columns is not null then c.inequality_columns
--		end
--		+ ')' + char(10) +
--		case 
--			when c.included_columns is not null then 'INCLUDE (' + c.included_columns + ')'
--			else ''
--		end 
--			includes
--	FROM 
--		sys.dm_db_missing_index_group_stats a
--		inner join sys.dm_db_missing_index_groups b on a.group_handle = b.index_group_handle
--		inner join sys.dm_db_missing_index_details c on c.index_handle = b.index_handle
--	WHERE 
--		db_name(database_id) = db_name()
--			and 
--		equality_columns is not null
--	ORDER BY 
--		a.avg_total_user_cost * a.avg_user_impact * (a.user_seeks + a.user_scans) DESC 



--------------------------------------------------------------------------------------------
-- Создание недостающих индексов баз данных на SQL Server 2005, 2008
--
-- Скрипт анализирует статистику, собранную сервером баз данных об отсутствующих индексах в базах данных и
-- предлагает создать индексы, которые могут обеспечить значительное повышение производительности. 

	SELECT 
		object_name(mid.object_id),
		[Преимущество индекса] = user_seeks * avg_total_user_cost * (avg_user_impact * 0.01),
	    [База данных] = DB_NAME(mid.database_id),
		[Transact SQL код для создания индекса] = 
			'CREATE NONCLUSTERED INDEX idx_' + isnull(object_name(mid.object_id) , '<>')
			+ replace(replace(replace(replace(isnull('__' + mid.equality_columns,'') + isnull('__' + mid.inequality_columns,''), '[',''), ']',''), ',','__'),' ' ,'')
			+ replace(replace(replace(replace(isnull('__INC__' + mid.included_columns,''), '[',''), ']',''), ',','__'),' ' ,'')
			+ ' ON ' + 
			mid.statement + ' (' + ISNULL(mid.equality_columns,'') + 
			(CASE WHEN mid.equality_columns IS NOT NULL AND mid.inequality_columns IS NOT NULL THEN ', ' ELSE '' END) + 
			(CASE WHEN mid.inequality_columns IS NOT NULL THEN + mid.inequality_columns ELSE '' END) + 
			')' + 
			(CASE WHEN mid.included_columns IS NOT NULL THEN ' INCLUDE (' + mid.included_columns + ')' ELSE '' END) + 
			';', 
		[Число компиляций] = migs.unique_compiles,
		[Количество операций поиска] = migs.user_seeks,
	    [Количество операций просмотра] = migs.user_scans,
		[Средняя стоимость ] = CAST(migs.avg_total_user_cost AS int),
		[Средний процент выигрыша] = CAST(migs.avg_user_impact AS int)

	FROM  
		sys.dm_db_missing_index_groups mig
		JOIN  sys.dm_db_missing_index_group_stats migs ON migs.group_handle = mig.index_group_handle
		JOIN  sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle AND mid.database_id = db_id()

	WHERE
		object_name(mid.object_id) like'%%'
	ORDER BY
		--1, 2 DESC
		2 DESC, 1
/*--
-- Значение ''Преимущество индекса'' выше 5000 в промышленных системах означает, что следует рассмотреть возможность создания этих индексов.

-- Если же значение превышает 10000, это обычно означает, что индекс может обеспечить значительное повышение производительности для операций чтения.

--------------------------------------------------------------------------------------------

-- Отправляем email с рекомендацией создать индекс

IF (object_id('tempdb..##IndexAdvantage2') IS NOT NULL) DROP TABLE ##IndexAdvantage2

SELECT * INTO ##IndexAdvantage2 FROM ##IndexAdvantage WHERE [Преимущество индекса] >= 5000 ORDER BY 1 DESC

 

IF ((SELECT COUNT(*) FROM ##IndexAdvantage2) >= 1) BEGIN

DECLARE @subject_str varchar(255),

@message_str varchar(1024),

@separator_str varchar(1),

@email varchar(128)

 

SET @separator_str=CHAR(9) -- Символ табуляции

SET @email = 'sebas@yandex-team.ru'

 

-- Подготовим текст сообщения

SET @subject_str = 'SQL Server '+@@SERVERNAME+': Предложение создать индексы в базе данных.'

SET @message_str = 'Сервер '+@@SERVERNAME + '. Выявлена необходимость создать индексы в базе данных!

Во вложении - таблица с кодом предлагаемых индексов.

Значение "Преимущество индекса" выше 5000 в промышленных системах означает, что следует рассмотреть возможность создания этих индексов.

Если же значение превышает 10000, это обычно означает, что индекс может обеспечить значительное повышение производительности для операций чтения.

Динамические административные представления, которые помогли нам получить информацию об отсутствующих индексах, не являются заменой помощника по настройке ядра СУБД, который также рассматривает индексированные представления и секции и обеспечивает более всесторонний анализ индексов, но они могут быть очень эффективны на начальном уровне анализа.'

 

-- Отправляем email

EXEC msdb.dbo.sp_send_dbmail

@recipients = @email,

@query = 'SELECT * FROM ##IndexAdvantage2',

@subject = @subject_str,

@body = @message_str,

@attach_query_result_as_file = 1,

@query_result_separator = @separator_str,

@query_result_width = 7000

END

 

-- Удаляем временную таблицу

IF (object_id('tempdb..##IndexAdvantage') IS NOT NULL) DROP TABLE ##IndexAdvantage

IF (object_id('tempdb..##IndexAdvantage2') IS NOT NULL) DROP TABLE ##IndexAdvantage2
--------------------------------------------------------------------------------------

*/

GO