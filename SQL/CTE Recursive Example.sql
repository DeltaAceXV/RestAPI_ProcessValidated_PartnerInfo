--CREATE TABLE material_row(
--	id BIGINT PRIMARY KEY IDENTITY(1,1),
--	parent_id BIGINT NOT NULL,
--	name varchar(30) NOT NULL,
--	level tinyint NOT NULL DEFAULT(0),
--)

--CREATE INDEX idx_id ON material_row(id) 

WITH CTE AS (
	SELECT id, name, CAST(0 AS BIGINT) AS parent_id, CAST('' AS VARCHAR(30)) AS parent_name, 0 AS row_level from material_row 
	WHERE parent_id = 0

	UNION ALL

	SELECT material_row.id, material_row.name, material_row.parent_id,  parent_row.name AS parent_name, 1 + header.row_level AS row_level from material_row 
	JOIN CTE header ON header.id = material_row.parent_id  

	JOIN material_row parent_row ON material_row.parent_id = parent_row.id

)
SELECT *, REPLICATE(' ', row_level) + name AS test FROM CTE 
ORDER BY row_level, parent_name  


SELECT * FROM material_row 

BEGIN TRANSACTION;

BEGIN TRY

	INSERT INTO material_row (parent_id, name, level)
	VALUES (1, 'SemiFG4', 1);

	--THROW 50001, 'test', 1;

END TRY
BEGIN CATCH
	ROLLBACK;
	THROW;
END CATCH

COMMIT;

--INSERT INTO material_row (parent_id, name, level) 
--VALUES 
--(4, 'Materials8', 2)
--, (3, 'Materials6', 2)
--, (2, 'Materials7', 2)



