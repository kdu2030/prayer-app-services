CREATE OR REPLACE FUNCTION get_prayer_groups_by_user(user_id INT)
RETURNS TABLE (
    id INT,
    group_name VARCHAR(255),
    image_file_id INT,
    file_name VARCHAR(255),
    url VARCHAR(255),
    file_type INT
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        g.id,
        g.group_name,
        f.id AS image_file_id,
        f.file_name,
        f.url,
        f.file_type
    FROM 
        prayer_groups g
    INNER JOIN
        prayer_group_users u ON u.prayer_group_id = g.id
    LEFT JOIN
        media_files f ON g.image_file_id = f.id
    WHERE 
        app_user_id = user_id;
END
$$ LANGUAGE plpgsql;
