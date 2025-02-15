CREATE OR REPLACE PROCEDURE add_prayer_group_users(group_id INT, users_to_add prayer_group_user_to_add[])
LANGUAGE plpgsql
AS
$$
BEGIN
    IF EXISTS (
        SELECT
            1
        FROM
            unnest(users_to_add) AS u
        INNER JOIN prayer_group_users pgu ON pgu.app_user_id =(u).id
    )
    THEN
        RAISE EXCEPTION 'Users to add cannot contain users that are already in the Prayer Group.';
    END IF;

    INSERT INTO
        prayer_group_users (prayer_group_id, app_user_id, role)
    SELECT
        group_id,
        (u).id,
        (u).prayer_group_role
    FROM
        unnest(users_to_add) AS u;
END;
$$;