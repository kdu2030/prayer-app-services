CREATE OR REPLACE FUNCTION search_prayer_groups (
    group_name_query VARCHAR(255),
    max_num_results INT DEFAULT 20
) RETURNS TABLE (
    prayer_group_id INT,
    group_name VARCHAR(255),
    media_file_id INT,
    file_name VARCHAR(255),
    file_url VARCHAR(255),
    file_type INT
)
AS
$$
BEGIN
    RETURN QUERY
        SELECT
            g.prayer_group_id,
            g.group_name,
            f.media_file_id,
            f.file_name,
            f.file_url,
            f.file_type
        FROM
            prayer_groups g
        LEFT JOIN
            media_files f ON f.media_file_id = g.avatar_file_id
        WHERE
            g.group_name % group_name_query OR g.group_name ILIKE '%' || group_name_query || '%'
        ORDER BY
            similarity(g.group_name, group_name_query) DESC
        LIMIT max_num_results;
END;
$$
LANGUAGE plpgsql;