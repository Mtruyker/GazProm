SELECT 'CREATE DATABASE gas_service_app'
WHERE NOT EXISTS (
    SELECT 1
    FROM pg_database
    WHERE datname = 'gas_service_app'
)
\gexec
