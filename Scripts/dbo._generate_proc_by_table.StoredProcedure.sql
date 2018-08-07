IF OBJECT_ID('[dbo].[_generate_proc_by_table]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[_generate_proc_by_table]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec _generate_proc_by_table 'line_supplier_offer_exclude', 't_'
CREATE   procedure [dbo].[_generate_proc_by_table]
	@name nvarchar(50),
	@prefix nvarchar(10)
as
	set nocount on

	declare @t table ( n int, name nvarchar(100), fulltype nvarchar(200), type nvarchar(100) )
	declare 
		@table_name as nvarchar(60)
		,@proc_name as nvarchar(52)
	set @table_name = @prefix + @name
	set @proc_name = 'up_' + @name

	insert @t ( name, fulltype, type )
	select 
		col.name,
		tp.name + case when tp.name in ('char', 'nvarchar', 'nchar', 'nvarchar') then '(' + case when col.length >-1 then ltrim(str(col.length)) else 'MAX' end + ')' else '' end , 
		tp.name 
		from 
		[syscolumns] col
		join [sysobjects] obj on col.id = obj.id
		join [systypes] tp on tp.xusertype = col.xusertype
	where obj.name = @table_name

	declare c1 cursor local forward_only for
		select n from @t

	open c1
	declare
		@n int,
		@dummy int
	set @n = 0
	fetch next from c1
		into @dummy
	while @@fetch_status = 0
	begin
		update @t set n = @n
			where current of c1
		set @n = @n + 1
		fetch next from c1
			into @dummy
	end
	close c1
	deallocate c1

	declare @maxn int
	select @maxn = max( n ) from @t

	declare @lines table( line varchar(8000) )

	-- ********* PROCEDURE HEADER **********

	insert @lines ( line )
					select 'IF OBJECT_ID(''' + 'v' + @table_name + ''', ''V'') IS NOT NULL'
		union all	select '		DROP  VIEW dbo.v' + @table_name
		union all 	select 'GO'

		union all 	select ''
		union all 	select 'CREATE VIEW dbo.v' + @table_name
		union all 	select 'AS'
		union all 	select '  SELECT * FROM dbo.' + @table_name + ' with (nolock) '
		union all 	select ''
		union all 	select 'GO'

		union all 	select ''
		union all 	select '--============================================='
		union all 	select '-- Author:		'  +   SUSER_SNAME(SUSER_SID())
		union all 	select '-- Create date: ' +  convert(nvarchar(20), getdate(), 20)
		union all 	select '-- Description:	<Description,,>'
		union all 	select '-- ============================================='
		union all 	select 'IF OBJECT_ID(''[dbo].[' + @proc_name + '], ''P'') IS NOT NULL'
		union all 	select '	DROP PROCEDURE [dbo].[' + @proc_name + ']'
		union all 	select ''		
		union all 	select 'GO'

		union all 	select ''
		union all 	select 'CREATE PROCEDURE dbo.' + @proc_name
		union all 	select '	@mode			nvarchar(50),'

	insert @lines ( line )
					select '	@' + name + '			' + fulltype + ' = null,' from @t
		where not name in ( 'archive',	's_created_date', 's_changed_date', 's_changed_user_id', 'owner_id' )
		order by n

	if exists( select * from @t where name = 'archive' )
		insert @lines ( line )
					select '	@archive		bit				 = 0,'

	insert @lines ( line )
					select '	@user_id		uniqueidentifier = null,'
		union all 	select '	@debug			bit				 = 0'
		union all 	select 'AS'
		union all 	select 'BEGIN'
		union all 	select '	IF @debug = 1'
		union all 	select '		BEGIN'
		union all 	select '			PRINT CAST(@@NESTLEVEL AS nvarchar(100)) + ''. '' + OBJECT_NAME(@@PROCID) + '' '' + convert(nvarchar(24), getdate(), 121) + '', mode = '' + ISNULL(@mode, ''NULL'')'
		union all 	select '			PRINT ''---'''
		union all 	select '		END'
		union all 	select ''
		union all 	select '	SET NOCOUNT ON'
		--union all 	select '	-- check the rights for this user'
		--union all 	select '	DECLARE @r int, @interface uniqueidentifier'
		--union all 	select '	SELECT @r=0, @interface = (SELECT TOP 1 id FROM dbo.fn_get_interface(null, null, null, null) where name=''' + @proc_name + ''')'
		--union all 	select '	EXEC @r=up_UIPermission @mode=''get'', @user_id=@user_id, @interface_id = @interface'
		--union all 	select '	IF @r&16 = 0 EXEC upu_raise_error  @error_code = 300000, @message_marker = ''Нет прав на запуск'''
		union all 	select ''
		union all 	select '	'
		union all 	select ''
		union all 	select '	DECLARE'
		union all 	select '		@rc		int'


	insert @lines ( line )
					select '	IF upper( @mode ) IN ( ''INSERT'', ''UPDATE'', ''DELETE'')	BEGIN'
		union all 	select '		SELECT TOP 0 t.* INTO #upu_' + @name  
		union all 	select '			FROM'
		union all 	select '				 (SELECT 1 a ) x LEFT JOIN v_' + @name + ' t ON 1=0'
		union all 	select '	END'
	-- ********* INSERT **********

	insert @lines ( line )
					select '    --------------------------------'
		union all 	select '    -- INSERT --'
		union all 	select '    --------------------------------'
		union all 	select ''		
		union all 	select '	IF upper( @mode ) = ''INSERT''	BEGIN'
		--union all 	select '		IF @r&4 =0 BEGIN'
		--union all 	select '			EXEC upu_raise_error  @error_code = 300000, @message_marker = ''Нет прав на создание'''
		--union all 	select '			RETURN'
		--union all 	select '		END'
		union all 	select ''
		union all 	select '		SELECT '
		union all 	select '			@id			= isnull(@id, newid())'
		union all 	select ''

	insert @lines ( line )
					select '		--SET XACT_ABORT ON'
		union all 	select '		--SET ARITHABORT ON'
		union all 	select ''
		union all 	select '		--BEGIN TRANSACTION'
		union all 	select ''
		union all 	select '			INSERT #upu_' + @name + '('
	insert @lines ( line )
					select '					[' + name + '],' from @t where not name in ('archive', 'nmb', 'user_owner_id', 'owner_id',	's_created_date', 's_changed_date', 's_changed_user_id') order by n -- 'id', 'owner_id',
	insert @lines ( line )
					select '					[archive]'
		union all   select '				)'
		union all   select '				SELECT'
	insert @lines ( line )
					select '					@' + name + ','  from @t where not name in ('archive', 'nmb' , 'user_owner_id', 'owner_id',	's_created_date', 's_changed_date', 's_changed_user_id') order by n -- 'id', 'owner_id',
	insert @lines ( line ) 
					select '					0'
		union all 	select '				' 

	insert @lines ( line )
					select ''
		union all 	select '			EXEC @rc = upu_' + @name + '_Update'
		union all 	select '				@user_id = @user_id,'
		union all 	select '				@debug = @debug'
		union all 	select ''		
		union all 	select '			IF @@ERROR <> 0 OR @rc <> 0 BEGIN'
		union all 	select '				--IF @@trancount = 1 ROLLBACK ELSE COMMIT'
		union all 	select '				RETURN -1'
		union all 	select '			END'

	insert @lines ( line )
					select '		--COMMIT TRANSACTION'
		union all 	select ''
		union all	select '		RETURN'
		union all	select '	END'


	-- ********* UPDATE **********

	insert @lines ( line )
					select '    --------------------------------'
		union all 	select '    -- UPDATE --'
		union all 	select '    --------------------------------'
		union all 	select ''		
		union all 	select '	IF upper( @mode ) = ''UPDATE'' BEGIN'
		union all 	select '		IF @id IS NULL BEGIN'
		union all 	select '			EXEC upu_raise_error  @error_code = 300000, @message_marker = ''При обновлении не указан идентификатор'''
		union all 	select '			RETURN'
		union all 	select '		END'
		union all 	select ''
		union all 	select '		SET XACT_ABORT ON'
		union all 	select '		SET ARITHABORT ON'
		union all 	select ''
		union all 	select '		BEGIN TRANSACTION'
		union all 	select ''
		union all 	select '			INSERT #upu_' + @name  
		union all 	select '				SELECT'
		union all 	select '					*'   
		union all 	select '				FROM'
		union all 	select '					 v_' + @name 
		union all 	select '				WHERE'
		union all 	select '					id = @id'
		union all 	select ''
		union all 	select '			UPDATE #upu_' + @name
		union all 	select '				SET'
	insert @lines ( line )
					select '					[' + name + ']	= @' + name + ',' from @t
		where not name in ( 'id', 'owner_id', 'archive', 'nmb', 'user_owner_id',  's_created_date', 's_changed_date', 's_changed_user_id' )
		order by n
	insert @lines ( line )
					select '					[archive]	= isnull(@archive, [archive])'


	insert @lines ( line )
					select ''
		union all 	select '			EXEC @rc = upu_' + @name + '_Update'
		union all 	select '				@user_id = @user_id,'
		union all 	select '				@debug = @debug'
		union all 	select ''		
		union all 	select '			IF @@ERROR <> 0 OR @rc <> 0 BEGIN'
		union all 	select '				IF @@trancount = 1 ROLLBACK ELSE COMMIT'
		union all 	select '				RETURN -1'
		union all 	select '			END'

	insert @lines ( line )
					select '		COMMIT TRANSACTION'
		union all 	select ''
		union all	select '		RETURN'
		union all	select '	END'

	-- ********* DELETE **********

	insert @lines ( line )
					select '    --------------------------------'
		union all 	select '    -- DELETE --'
		union all 	select '    --------------------------------'
		union all 	select ''		
		union all 	select '	IF upper( @mode ) = ''DELETE'' BEGIN'
		union all 	select '		IF @id is null BEGIN'
		union all 	select '			EXEC upu_raise_error  @error_code = 300000, @message_marker = ''При удалении не указан идентификатор'''
		union all 	select '			RETURN'
		union all 	select '		END'
		union all 	select ''
		union all 	select '		SET XACT_ABORT ON'
		union all 	select '		SET ARITHABORT ON'
		union all 	select ''
		union all 	select '		BEGIN TRANSACTION'
		union all 	select ''
		union all 	select '			INSERT #upu_' + @name  
		union all 	select '				SELECT'
		union all 	select '					*' 
		union all 	select '				FROM'
		union all 	select '					 v_' + @name 
		union all 	select '				WHERE'
		union all 	select '					id = @id'
		union all 	select ''
		union all 	select '			UPDATE #upu_' + @name
		union all 	select '				SET'
		union all 	select '					[archive] = 1'
		union all 	select ''
		union all 	select '			EXEC @rc = upu_' + @name + '_Update'
		union all 	select '				@user_id = @user_id,'
		union all 	select '				@debug = @debug'
		union all 	select ''		
		union all 	select '			IF @@ERROR <> 0 OR @rc <> 0 BEGIN'
		union all 	select '				IF @@trancount = 1 ROLLBACK ELSE COMMIT'
		union all 	select '				RETURN -1'
		union all 	select '			END'
		union all 	select ''
		union all 	select '			PRINT ''+++'''
		union all 	select ''
		union all	select '		COMMIT TRANSACTION'
		union all	select '		RETURN'
		union all	select '	END'



	-- ********* SELECT **********

	insert @lines ( line )
					select '    --------------------------------'
		union all 	select '    -- SELECT --'
		union all 	select '    --------------------------------'
		union all 	select ''		
		union all 	select '	IF upper(@Mode) = ''SELECT'' BEGIN'

		union all 	select '		SELECT'
	insert @lines ( line )
					select '				t.' + name + case when n <> @maxn then ',' else '' end from @t
		order by n
	insert @lines ( line )
					select '			FROM '
		union all 	select '				v' + @table_name + ' t'
		union all 	select '			WHERE'
		union all 	select '				t.[id] = @id'
		union all 	select '			ORDER BY'
		union all 	select '				t.[name]'
		union all 	select ''
		union all 	select '		RETURN'
		union all 	select '	END'
		union all 	select ''

	-- ********* SELECTPLAIN **********

		union all 	select '    --------------------------------'
		union all 	select '    -- SELECTPLAIN --'
		union all 	select '    --------------------------------'
		union all 	select ''		
		union all 	select '	IF upper(@Mode) = ''SELECTPLAIN'' BEGIN'
		union all 	select '		SELECT'
		union all 	select '				t.[id],'
		union all 	select '				t.[name]'
		union all 	select '			FROM '
		union all 	select '				v' + @table_name + ' t'
		union all 	select '			WHERE'
		union all 	select '				t.[archive] = 0'
		union all 	select '			ORDER BY'
		union all 	select '				t.[name]'
		union all 	select ''
		union all 	select '		RETURN'
		union all 	select '	END'
		union all 	select ''
	-- ********* NEW **********

		union all 	select '    --------------------------------'
		union all 	select '    -- NEW --'
		union all 	select '    --------------------------------'
		union all 	select ''	
		union all 	select '	IF upper(@Mode) = ''NEW'' BEGIN'
		union all 	select ''
		union all 	select '		SELECT '
		union all 	select '			@id			= newid(),'
		union all 	select '			@archive	= 0,'
		union all 	select '			@nmb		= 0'
		union all 	select ''
		union all 	select '		SELECT'
	insert @lines ( line )
					select '			@' + name + '	[' + name + '],' FROM @t
		where not name in ( 'owner_id' )
		order by n

	if exists( select * from @t where name = 'owner_id' )

		insert @lines ( line )
					select '			dbo.owner_company_id()	[owner_id]'

	insert @lines ( line )
					select ''
		union all 	select ''
		union all 	select '		RETURN'
		union all 	select ''
		union all 	select '	END'
		union all 	select ''

	-- ********* NEW **********

		union all 	select '    --------------------------------'
		union all 	select '    -- LOOKUP --'
		union all 	select '    --------------------------------'
		union all 	select ''	
		union all 	select '	IF upper(@Mode) = ''LOOKUP'' BEGIN'
		union all 	select '		SELECT '
		union all 	select '				[id],'
		union all 	select '				[nmb],'
		union all 	select '				[name],'
		union all 	select '				[archive]'
		union all 	select '			FROM'
		union all 	select '				dbo.v' + @table_name
		union all 	select '			ORDER BY' 
		union all 	select '				[name]'
		union all 	select ''
		union all 	select '		RETURN'
		union all 	select ''
		union all 	select '	END'
		union all 	select ''
		union all 	select '    --------------------------------'
		union all 	select '    -- TREELIST --'
		union all 	select '    --------------------------------'
		union all 	select ''	
		union all 	select '	IF upper(@Mode) = ''TREELIST'' BEGIN'
		union all 	select '		SELECT '
		union all 	select '				[id],'
		union all 	select '				[nmb],'
		union all 	select '				[name],'
		union all 	select '				[archive]'
		union all 	select '			FROM'
		union all 	select '				dbo.v' + @table_name
		union all 	select '			ORDER BY' 
		union all 	select '				[name]'
		union all 	select ''
		union all 	select '		RETURN'
		union all 	select ''
		union all 	select '	END'


	-- ********** END OF PROC ***********

--	insert @lines ( line )
--					select '	EXEC upu_raise_error  @error_code = 300000, @message_marker = ''Неверный режим'''
	insert @lines ( line )
					select '	DECLARE @errmsg nvarchar(1000)'
		union all 	select '	SELECT @errmsg = ''Неверный режим ("'' + isnull(@mode, ''<null>'') + ''")'''
		union all 	select '	EXEC upu_raise_error @error_code = 300010, @message_marker = @errmsg'
		union all 	select ''
		union all 	select '	IF @debug = 1'
		union all 	select '			PRINT ''+++'''
		union all 	select ''
		union all 	select 'END'

	-- ********** GRANT EXECUTION RIGHTS ***********

	insert @lines ( line )
					select 'GO'
		union all 	select ''
		union all	select 'GRANT  EXECUTE  ON ' + @proc_name + ' TO [Adoffice_win_client]'
		union all 	select 'GO'

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
