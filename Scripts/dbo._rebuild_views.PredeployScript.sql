IF OBJECT_ID('[dbo].[_rebuild_views]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[_rebuild_views]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[_rebuild_views] 
as
	declare _rebuild_views_cursor cursor local fast_forward for	
		select name from sysobjects where type = 'V' 
		and name not like '%awb%' 
		and name not like '%v_ya_catalog%'
		and name not like 'v'

	declare @name sysname,@msg varchar(500)

	open _rebuild_views_cursor
	fetch next from _rebuild_views_cursor into @name
	while @@FETCH_STATUS = 0
	begin
		--print @@trancount
		print 'Rebuilding ' + @name
		BEGIN TRY
			exec sp_refreshview @name
		END TRY
		BEGIN CATCH
			if @@trancount >0 rollback
			set @msg='Error on rebuild '+@name
			EXEC upu_raise_error @error_code = 300100, @message_marker = @msg
		END CATCH;
		fetch next from _rebuild_views_cursor into @name
	end
GO
