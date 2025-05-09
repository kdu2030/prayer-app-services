DROP FUNCTION IF EXISTS create_prayer_group;

CREATE OR REPLACE FUNCTION create_prayer_group(
    username VARCHAR(255),
    new_group_name VARCHAR(255),
    group_description TEXT,
    group_rules TEXT,
    group_color INT,
    group_image_file_id INT,
    group_banner_image_file_id INT
) RETURNS TABLE (
    id INT,
    group_name VARCHAR(255),
    description TEXT,
    rules TEXT,
    color INT,
    image_file_id INT,
    group_image_file_name VARCHAR(255),
    group_image_file_url VARCHAR(255),
    banner_image_file_id INT,
    banner_image_file_name VARCHAR(255),
    banner_image_file_url VARCHAR(255),
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
        file_name VARCHAR(255),
        url VARCHAR(255),
        file_type INT
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
        temp_relevant_files (id, file_name, url, file_type)
    SELECT
        f.id,
        f.file_name,
        f.url,
        f.file_type
    FROM media_files f, temp_admin_user a
    WHERE f.id IN (a.image_file_id, group_image_file_id, group_banner_image_file_id);

    IF NOT EXISTS (SELECT 1 FROM temp_admin_user)
    THEN
        RAISE EXCEPTION 'User does not exist.';
    END IF;

    IF group_image_file_id IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM temp_relevant_files f 
        WHERE f.id = group_image_file_id AND file_type = 1
    )
    THEN
        RAISE EXCEPTION 'Image file for group not found or file is not an image.';
    END IF;

    IF group_banner_image_file_id IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM temp_relevant_files f
        WHERE f.id = group_banner_image_file_id AND file_type = 1
    )
    THEN
        RAISE EXCEPTION 'Banner image file for group not found or file is not an image.';
    END IF;


    INSERT INTO
        prayer_groups (group_name, description, rules, color, image_file_id, banner_image_file_id)
    VALUES
        (new_group_name, group_description, group_rules, group_color, group_image_file_id, group_banner_image_file_id)
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
            new_group_name, 
            group_description, 
            group_rules,
            group_color,
            group_image_file_id,
            f2.file_name,
            f2.url,
            group_banner_image_file_id,
            f3.file_name,
            f3.url,
            a.id,
            a.full_name,
            a.image_file_id,
            f1.file_name,
            f1.url
        FROM temp_admin_user a 
        LEFT JOIN temp_relevant_files f1 ON a.image_file_id = f1.id
        LEFT JOIN temp_relevant_files f2 ON f2.id = group_image_file_id
        LEFT JOIN temp_relevant_files f3 ON f3.id = group_banner_image_file_id;
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