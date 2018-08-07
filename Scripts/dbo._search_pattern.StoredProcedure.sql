IF OBJECT_ID('[dbo].[_search_pattern]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[_search_pattern]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    proc [dbo].[_search_pattern]
	@pattern nvarchar(2000),
	@back int = null, 
	@forth int = null
as

set @pattern = '%' + @pattern + '%'

declare @search_res table( id int, pos int ) -- pos is zero-based
declare @chunks table( id int, colid int, begpos int, endpos int ) -- pos is zero-based

declare text_cur cursor local fast_forward for 
	select id, colid, text
	from syscomments 
	where id > 15 and texttype = 2 and encrypted = 0 and compressed = 0
	order by id, colid 

declare 
	@id int
	,@colid smallint
	,@text nvarchar(4000)
	,@stext nvarchar(4000)
	,@pos int -- Position of the first symbol of the current chunk
	,@previd int -- Id of object from previous iteration
	,@prevtext nvarchar(4000) -- Chunk of text (of the same object) from previous iteration.
	,@lastfound int -- Relative pos of last found entry
	,@startpos int -- Relative pos of start symbol for seach
	,@found bit 

set @previd = 0

--- SEARCH ---

open text_cur
fetch next from text_cur into @id, @colid, @text
while @@FETCH_STATUS = 0
begin
	if @id <> @previd
	begin
		set @pos = 0
		set @prevtext = ''
		set @previd = @id
		--set @lastfound = -1
	end
	set @found = 0
	if @prevtext <> ''
	begin
		set @startpos = 1
		set @stext = right(@prevtext, 2000) + left(@text, 2000)
		while 1=1
		begin
			set @lastfound = patindex(@pattern, substring(@stext, @startpos, 4000))
			if @lastfound > 0
			begin
				insert @search_res ( id, pos ) values ( @id, @pos - 2000 + (@startpos + @lastfound - 2) ) -- @prevtext should not by much shorter then 4000 (sometimes it happens to be 3999 :-( )
				set @startpos = @startpos + @lastfound
				set @found = 1
			end else begin
				break
			end
		end
	end
	set @startpos = 1
	set @stext = @text
	while 1=1
	begin
		set @lastfound = patindex(@pattern, substring(@stext, @startpos, 4000))
		if @lastfound > 0
		begin
			insert @search_res ( id, pos ) values ( @id, @pos + (@startpos + @lastfound - 2) ) -- @prevtext should not be much shorter then 4000 (sometimes it happens to be 3999 :-( )
			set @startpos = @startpos + @lastfound
			set @found = 1
		end else begin
			break
		end
	end

	if @found = 1
		insert @chunks (id, colid, begpos, endpos) values (@id, @colid, @pos, @pos + len(@text))

	set @prevtext = @text
	set @pos = @pos + len(@text)
	fetch next from text_cur into @id, @colid, @text
end
close text_cur
deallocate text_cur

--- OUTPUT ---

--declare @search_res2 table( id int, pos int, text nvarchar(1000) ) -- pos is zero-based

--insert @search_res2(id, pos)
set @back = isnull(@back, 20)
set @forth = isnull(@forth, 50)

select 
	obj.name, obj.type, res.pos,

	case when res.pos - chnk.begpos + 1 - @back < 1 and cprev.text is not null
	then right(cprev.text, chnk.begpos - res.pos + @back ) 
	else '' end
+

	substring(c.text, 
	case when res.pos - chnk.begpos + 1 - @back < 1 then 1
	else res.pos - chnk.begpos + 1 - @back end, 
	case when res.pos - chnk.begpos + 1 - @back < 1 
	then @forth + (res.pos - chnk.begpos)
	else @back + @forth end) 
+
	case when res.pos + @forth > chnk.endpos and cnext.text is not null
	then left(cnext.text, res.pos + @forth - chnk.endpos)
	else '' end
from 
(select distinct id, pos from @search_res) res
join @chunks chnk on chnk.id = res.id and res.pos >= chnk.begpos and res.pos < chnk.endpos
join syscomments c on c.id = chnk.id and c.colid = chnk.colid
left join syscomments cprev on cprev.id = chnk.id and c.colid = chnk.colid - 1
left join syscomments cnext on cnext.id = chnk.id and c.colid = chnk.colid + 1
left join sysobjects obj on obj.id = chnk.id
order by obj.type, obj.name
GO
