CREATE OR REPLACE FUNCTION get_prayer_group(
    target_prayer_group_id INT,
    target_user_id INT
) RETURNS TABLE (
    prayer_group_id INT,
    group_name VARCHAR(255),
    description TEXT,
    rules TEXT,
    visibility_level INT,
    avatar_file_id INT,
    avatar_file_name VARCHAR(255),
    avatar_file_url VARCHAR(255),
    avatar_file_type INT,
    banner_file_id INT,
    banner_file_name VARCHAR(255),
    banner_file_url VARCHAR(255),
    banner_file_type INT,
    prayer_group_role INT,
    join_request_id INT
)
AS
$$
BEGIN
    RETURN QUERY
        SELECT
            g.prayer_group_id,
            g.group_name,
            g.description,
            g.rules,
            g.visibility_level,
            g.avatar_file_id,
            f1.file_name AS avatar_file_name,
            f1.file_url AS avatar_file_url,
            f1.file_type AS avatar_file_type,
            g.banner_file_id,
            f2.file_name AS banner_file_name,
            f2.file_url AS banner_file_url,
            f2.file_type AS banner_file_type,
            pgu.prayer_group_role,
            r.join_request_id
        FROM
            prayer_groups g
        LEFT JOIN
            media_files f1 ON f1.media_file_id = g.avatar_file_id
        LEFT JOIN
            media_files f2 ON f2.media_file_id = g.banner_file_id
        LEFT JOIN
            prayer_group_users pgu ON pgu.prayer_group_id = g.prayer_group_id AND pgu.user_id = target_user_id
        LEFT JOIN
            join_requests r ON r.prayer_group_id = g.prayer_group_id AND r.user_id = target_user_id
        WHERE
            g.prayer_group_id = target_prayer_group_id;
END;
$$
LANGUAGE plpgsql;