CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` String,
    `ProductVersion` String
)
ENGINE = MergeTree()
ORDER BY (`MigrationId`);

CREATE TABLE `kass_events` (
    `Timestamp` DateTime,
    `TerminalId` String,
    `Good` String,
    `Amount` Decimal(18,2),
    `Metadata` Map(String, String)
)
ENGINE = MergeTree()
ORDER BY (`Timestamp`, `TerminalId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260613185303_InitialCreate', '10.0.2');

