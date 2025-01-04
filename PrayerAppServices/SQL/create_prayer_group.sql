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
    image_file_id INT
)
AS
$$
DECLARE 
        new_group_id INT;
        admin_user_id INT;
BEGIN
    SELECT
        u.id
    INTO
        admin_user_id
    FROM
        asp_net_users u
    WHERE user_name = username;

    IF admin_user_id IS NULL
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
    VALUES
        (new_group_id, admin_user_id, 1);

    RETURN QUERY SELECT 
        new_group_id, 
        group_name, 
        group_description, 
        group_rules,
        group_color,
        group_image_file_id;
    RETURN;
END;
$$
LANGUAGE plpgsql;