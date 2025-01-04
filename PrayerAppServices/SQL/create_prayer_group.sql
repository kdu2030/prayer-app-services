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
BEGIN
    DROP TABLE IF EXISTS temp_admin_user;
    DROP TABLE IF EXISTS temp_relevant_files;
    
    CREATE TEMPORARY TABLE temp_admin_user (
        id INT,
        full_name VARCHAR(255),
        image_file_id INT
    );

    CREATE TEMPORARY TABLE temp_relevant_files (
        id INT,
        name VARCHAR(255),
        url VARCHAR(255),
        type INT
    );

    INSERT INTO 
        temp_admin_user (id, full_name, image_file_id)
    SELECT
        u.id,
        full_name,
        u.image_file_id
    FROM
        asp_net_users u
    WHERE user_name = username;

    INSERT INTO
        temp_relevant_files (id, name, url, type)
    SELECT
        f.id,
        f.name,
        f.url,
        f.type
    FROM media_files f, temp_admin_user a
    WHERE f.id = a.image_file_id OR f.id = group_image_file_id;

    IF NOT EXISTS (SELECT 1 FROM temp_admin_user)
    THEN
        RAISE EXCEPTION 'User does not exist.';
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM temp_relevant_files f 
        WHERE f.id = group_image_file_id AND type = 1
    )
    THEN
        RAISE EXCEPTION 'Image file for group not found or file is not an image.';
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
        FROM temp_admin_user a INNER JOIN temp_relevant_files f ON a.image_file_id = f.id;
    DROP TABLE IF EXISTS temp_admin_user;
    DROP TABLE IF EXISTS temp_relevant_files;
    RETURN;
EXCEPTION
    WHEN OTHERS THEN
        DROP TABLE IF EXISTS temp_admin_user;
        DROP TABLE IF EXISTS temp_relevant_files;
        RAISE;
END;
$$
LANGUAGE plpgsql;
