# PostgreSQL setup

Run from the repository root with `psql` installed:

```powershell
psql -U postgres -f database\01_create_database.sql
psql -U postgres -d gas_service_app -f database\02_schema.sql
psql -U postgres -d gas_service_app -f database\03_seed.sql
```

The schema script recreates all application tables. Existing test data in these tables will be removed.

The app uses this default connection string:

```text
Host=10.164.203.35;Port=5432;Database=gas_service_app;Username=postgres;Password=12345678
```

Override it with the `GAS_SERVICE_DB` environment variable if your PostgreSQL user or password is different.
