IF OBJECT_ID('[sm].[_fn_to_camel_case]') IS NOT NULL
	DROP FUNCTION [sm].[_fn_to_camel_case]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SELECT [sm].[_fn_to_camel_case] ('T_ASD_DFG_1_')
CREATE FUNCTION [sm].[_fn_to_camel_case] 
(
	@name nvarchar(4000)
) 
returns nvarchar(4000) as
begin
	
	select @name = ltrim(rtrim(@name))
	select @name = lower(@name)
	select @name = replace(@name, ' ', '_')
	select @name = replace(@name, ';', '_')
	select @name = replace(@name, ':', '_')
	select @name = replace(@name, '!', '_')
	select @name = replace(@name, '?', '_')
	select @name = replace(@name, ',', '_')
	select @name = replace(@name, '.', '_')
	select @name = replace(@name, '/', '_')
	select @name = replace(@name, '&', '_')
	select @name = replace(@name, '''', '_')
	select @name = replace(@name, '"', '_')
	select @name = replace(@name, '(', '_')
	select @name = replace(@name, ')', '_')

	select @name = replace(@name, '_a', 'A')
	select @name = replace(@name, '_b', 'B')
	select @name = replace(@name, '_c', 'C')
	select @name = replace(@name, '_d', 'D')
	select @name = replace(@name, '_e', 'E')
	select @name = replace(@name, '_f', 'F')
	select @name = replace(@name, '_g', 'G')
	select @name = replace(@name, '_h', 'H')
	select @name = replace(@name, '_i', 'I')
	select @name = replace(@name, '_j', 'J')
	select @name = replace(@name, '_k', 'K')
	select @name = replace(@name, '_l', 'L')
	select @name = replace(@name, '_m', 'M')
	select @name = replace(@name, '_n', 'N')
	select @name = replace(@name, '_o', 'O')
	select @name = replace(@name, '_p', 'P')
	select @name = replace(@name, '_q', 'Q')
	select @name = replace(@name, '_r', 'R')
	select @name = replace(@name, '_s', 'S')
	select @name = replace(@name, '_t', 'T')
	select @name = replace(@name, '_u', 'U')
	select @name = replace(@name, '_v', 'V')
	select @name = replace(@name, '_w', 'W')
	select @name = replace(@name, '_x', 'X')
	select @name = replace(@name, '_y', 'Y')
	select @name = replace(@name, '_z', 'Z')

	if RIGHT(@name,1) = '_'
		select @name = left(@name, len(@name)-1)
	
	return @name
end
GO
