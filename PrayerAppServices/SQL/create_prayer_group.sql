CREATE OR REPLACE FUNCTION create_prayer_group(
    username VARCHAR(255),
    group_name VARCHAR(255),
    group_description TEXT,
    group_rules TEXT,
    group_color INT,
    group_image_file_id INT
) RETURNS TABLE (
    id INT,
    name VARCHAR(255),
    description TEXT,
    rules TEXT,
    color INT,
    image_file_id INT,
    admin_user_id INT,
    admin_full_name VARCHAR(255),
    admin_image_file_id INT,
    admin_image_file_name VARCHAR(255),
    admin_image_file_url VARCHAR(255)
)
AS
$$
DECLARE 
        new_group_id INT;
        admin_user_id INT;
BEGIN
    DROP TABLE IF EXISTS temp_admin_user;
    
    CREATE TEMPORARY TABLE temp_admin_user (
        id INT,
        full_name VARCHAR(255),
        image_file_id INT
    );

    INSERT INTO 
        temp_admin_user(id, full_name, image_file_id)
    SELECT
        u.id,
        full_name,
        u.image_file_id
    FROM
        asp_net_users u
    WHERE user_name = username;

    IF NOT EXISTS (SELECT 1 FROM temp_admin_user)
    THEN
        RAISE EXCEPTION 'User does not exist.';
    END IF;

    INSERT INTO
        prayer_groups (name, description, rules, color, image_file_id)
    VALUES
        (group_name, group_description, group_rules, group_color, group_image_file_id)
    RETURNING prayer_groups.id INTO new_group_id;

    INSERT INTO
        prayer_group_users (prayer_group_id, app_user_id, role)
    SELECT
        new_group_id,
        a.id,
        1
    FROM
        temp_admin_user a;

    RETURN QUERY 
        SELECT 
            new_group_id, 
            group_name, 
            group_description, 
            group_rules,
            group_color,
            group_image_file_id,
            a.id,
            a.full_name,
            a.image_file_id,
            f.name,
            f.url
        FROM temp_admin_user a INNER JOIN media_files f ON a.image_file_id = f.id;
    RETURN;
END;
$$
LANGUAGE plpgsql;
