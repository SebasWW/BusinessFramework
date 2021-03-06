IF OBJECT_ID('[sm].[p_reg_table]') IS NOT NULL 
	DROP PROCEDURE [sm].[p_reg_table]

/****** Object:  StoredProcedure [sm].[p_constants_restore]    Script Date: 14.02.2018 15:40:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--============================
-- create_date: 11.01.2018
-- author:		a.shamshur
-- description: Восстанавливает из соответствующего указанной таблице json-файла данные
--============================
CREATE PROCEDURE [sm].[p_reg_table]
AS
BEGIN
	INSERT 
		s_table
			(
				name,
				s_archive
			)
		SELECT
			name,
			0
		FROM
			sys.tables
		WHERE
			name not in
			(
				SELECT name FROM s_table
			)

	update t
		SET
			s_archive = 1
		FROM
			s_table t
		WHERE
			id not in
			(
				SELECT id FROM sys.tables
			)
       
END


