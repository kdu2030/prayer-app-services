CREATE OR REPLACE PROCEDURE update_prayer_group_admins(group_id INT, admin_user_ids_to_add INT[], admin_user_ids_to_remove INT[])
LANGUAGE plpgsql
AS
$$
DECLARE
    user_ids INT[];
BEGIN
    user_ids := admin_user_ids_to_add || admin_user_ids_to_remove;
    
    DROP TABLE IF EXISTS temp_prayer_group_users;

    CREATE TEMPORARY TABLE temp_prayer_group_users (
        id INT 
    );

    INSERT INTO
        temp_prayer_group_users (id)
    SELECT
        app_user_id
    FROM
        prayer_group_users u
    WHERE u.prayer_group_id = group_id;

    IF EXISTS (
        SELECT
            1
        FROM
            unnest(user_ids) u(id)
        LEFT JOIN
            temp_prayer_group_users gu ON gu.id = u.id
        WHERE
            gu.id IS NULL
    )
    THEN
        RAISE EXCEPTION 'Admin users to add and to remove must be part of the prayer group.';
    END IF; 

    UPDATE
        prayer_group_users u
    SET
        role = 1
    WHERE
        u.prayer_group_id = group_id AND u.app_user_id = ANY(admin_user_ids_to_add);

    UPDATE
        prayer_group_users u
    SET
        role = 2
    WHERE
        u.prayer_group_id = group_id AND u.app_user_id = ANY(admin_user_ids_to_remove);
    
    DROP TABLE IF EXISTS temp_prayer_group_users;
EXCEPTION
    WHEN OTHERS THEN
        DROP TABLE IF EXISTS temp_prayer_group_users;
        RAISE;
END;
$$;