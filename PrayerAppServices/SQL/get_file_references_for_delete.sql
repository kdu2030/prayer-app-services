CREATE OR REPLACE FUNCTION get_file_references_for_delete(target_file_id INT)
RETURNS TABLE (
    entity_id INT,
    entity_type INT
)
AS
$$
BEGIN
   RETURN QUERY
    SELECT
        id AS entity_id, 1 AS entity_type
    FROM
        asp_net_users
    WHERE
        image_file_id = target_file_id
    UNION
    SELECT
        prayer_group_id AS entity_id, 2 AS entity_type
    FROM
        prayer_groups
    WHERE
        avatar_file_id = target_file_id OR banner_file_id = target_file_id;
END;
$$
LANGUAGE plpgsql;