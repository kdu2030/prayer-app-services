CREATE OR REPLACE FUNCTION get_prayer_group_summaries_by_user(
    target_user_id INT
) RETURNS TABLE (
    prayer_group_id INT,
    group_name VARCHAR(255),
    media_file_id INT,
    file_name VARCHAR(255),
    file_url VARCHAR(255),
    file_type INT
)
AS
$$
BEGIN
    RETURN QUERY
        SELECT
            g.prayer_group_id,
            g.group_name,
            f.media_file_id,
            f.file_name,
            f.file_url,
            f.file_type
        FROM
            prayer_group_users u
        INNER JOIN
            prayer_groups g ON g.prayer_group_id = u.prayer_group_id
        LEFT JOIN
            media_files f ON f.media_file_id = g.avatar_file_id
        WHERE user_id = target_user_id;
END;
$$
LANGUAGE plpgsql;