CREATE EXTENSION IF NOT EXISTS pg_trgm;

CREATE INDEX IF NOT EXISTS group_name_trgm_idx
ON prayer_groups
USING gin (group_name gin_trgm_ops);