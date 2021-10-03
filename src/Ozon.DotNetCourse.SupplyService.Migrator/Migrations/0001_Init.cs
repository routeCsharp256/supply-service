using FluentMigrator;

namespace Ozon.DotNetCourse.SupplyService.Migrator.Migrations
{
    [Migration(1)]
    public class Init: ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE IF NOT EXISTS supply (
                    id         BIGSERIAL   NOT NULL PRIMARY KEY,
                    created_at TIMESTAMPTZ NOT NULL,
                    state      INT         NOT NULL);");
            
            Execute.Sql(@"CREATE INDEX IF NOT EXISTS supply_created_at_state_idx ON supply (created_at, state)");
            
            Execute.Sql(@"CREATE TABLE IF NOT EXISTS supply_item(
                            supply_id BIGINT NOT NULL,
                            sku       BIGINT NOT NULL,
                            quantity  INT    NOT NULL, 
                            PRIMARY KEY (supply_id, sku));");
        }

    }
}