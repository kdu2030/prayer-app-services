CREATE OR REPLACE FUNCTION get_prayer_group_user(group_id INT, username VARCHAR(255))
RETURNS TABLE (
    id INT,
    full_name VARCHAR(255),
    image_file_id INT,
    file_name VARCHAR(255),
    file_url VARCHAR(255),
    file_type INT,
    prayer_group_role INT
)
AS
$$
BEGIN
    RETURN QUERY
    SELECT
        a.id,
        a.full_name,
        a.image_file_id,
        f.name,
        f.url,
        f.type,
        pgu.role
    FROM 
        asp_net_users a
    LEFT JOIN
        media_files f ON f.id = a.image_file_id
    INNER JOIN
        prayer_group_users pgu ON pgu.app_user_id = a.id
    WHERE a.user_name = username AND prayer_group_id = group_id;
END;
$$
LANGUAGE plpgsql;