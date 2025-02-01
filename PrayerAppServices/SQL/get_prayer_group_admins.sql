CREATE OR REPLACE FUNCTION get_prayer_group_admins(group_id INT)
RETURNS TABLE (
    id INT,
    full_name VARCHAR(255),
    group_role INT,
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
        a.id,
        a.full_name,
        g.role,
        a.image_file_id,
        f.file_name,
        f.url,
        f.file_type
    FROM 
        prayer_group_users g 
    INNER JOIN 
        asp_net_users a ON g.app_user_id = a.id
    LEFT JOIN 
        media_files f ON f.id = a.image_file_id
    WHERE
        role = 1 AND prayer_group_id = group_id;
    RETURN;
END;
$$
LANGUAGE plpgsql;