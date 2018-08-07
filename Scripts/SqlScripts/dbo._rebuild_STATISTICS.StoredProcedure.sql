IF OBJECT_ID('[dbo].[_rebuild_STATISTICS]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[_rebuild_STATISTICS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[_rebuild_STATISTICS] 
as
	declare _rebuild_views_cursor cursor local fast_forward for	
		select name from sysobjects where type = 'u' order by name

	declare @name sysname

	open _rebuild_views_cursor
	fetch next from _rebuild_views_cursor into @name
	while @@FETCH_STATUS = 0
	begin
		exec ('UPDATE STATISTICS '+ @name + ' WITH FULLSCAN ')
		print @name
		fetch next from _rebuild_views_cursor into @name
	end
GO
