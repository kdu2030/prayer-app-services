CREATE OR REPLACE FUNCTION search_prayer_groups_by_name(name_query VARCHAR(255), max_num_results INT)
RETURNS TABLE (
    id INT,
    group_name VARCHAR(255),
    image_file_id INT,
    file_name VARCHAR(255),
    file_url VARCHAR(255),
    file_type INT
)
AS
$$
BEGIN 
    RETURN QUERY
    SELECT 
        g.id,
        g.group_name,
        g.image_file_id,
        f.file_name,
        f.url,
        f.file_type
    FROM 
        prayer_groups g
    INNER JOIN 
        media_files f ON f.id = g.image_file_id
    WHERE 
        to_tsvector(g.group_name) @@ websearch_to_tsquery(name_query)
    LIMIT max_num_results;
    RETURN;
END;
$$
LANGUAGE plpgsql;