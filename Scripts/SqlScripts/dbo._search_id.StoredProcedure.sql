IF OBJECT_ID('[dbo].[_search_id]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[_search_id]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec _search_id '61F94EEB-BACE-4EAE-AB9E-3FE218A4B01E'
CREATE  proc [dbo].[_search_id]
(
	@id as uniqueidentifier
)
as
	set nocount on
	select o.name , cast(0 as int) found
	into #cols
	--select *
	from syscolumns c 
	join sysobjects o on o.id = c.id
	join systypes t on t.xtype = c.xtype
	where c.name = 'id' 
	and o.xtype = 'U'
	and t.name = 'uniqueidentifier'
	and o.name not like '%~%'	

	DECLARE @name as nvarchar(100), @cmd nvarchar(1000)
	DECLARE cur CURSOR FAST_FORWARD FOR SELECT name from #cols
	OPEN cur
	FETCH NEXT FROM cur INTO @name
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @cmd = 'if exists (select id from ' + @name + ' where id = ''' + cast(@id as nvarchar(100)) + ''' ) begin update #cols set found = 1 where name = ''' + @name + ''' select * from ' + @name + ' where id = ''' + cast(@id as nvarchar(100)) + '''  end'
		exec sp_executesql	@cmd
	   	FETCH NEXT FROM cur INTO @name
	END
	CLOSE cur
	DEALLOCATE cur
	set nocount off
	select name from #cols
	where found = 1
	order by name


	drop table #cols
GO

