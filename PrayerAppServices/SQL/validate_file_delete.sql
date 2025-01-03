CREATE OR REPLACE FUNCTION validate_file_delete(file_id INT)
RETURNS TABLE (
    error VARCHAR(255)
)
AS
$$
BEGIN    
    IF EXISTS (SELECT 1 FROM asp_net_users u WHERE u.image_file_id = file_id)
    THEN
        RETURN QUERY SELECT CAST('File is associated with a user.' AS VARCHAR);
    END IF;

    IF EXISTS (SELECT 1 FROM prayer_groups p WHERE p.image_file_id = file_id)
    THEN
        RETURN QUERY SELECT CAST('File is associated with a prayer group.' AS VARCHAR);
    END IF;
    RETURN;
END;
$$
LANGUAGE plpgsql;