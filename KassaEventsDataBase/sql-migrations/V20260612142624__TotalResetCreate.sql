CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` String,
    `ProductVersion` String
)
ENGINE = MergeTree()
ORDER BY (`MigrationId`);

CREATE TABLE `kass_events` (
    `Id` UUID,
    `Timestamp` DateTime,
    `TerminalId` String,
    `Amount` Decimal(18,2),
    `Metadata` Map(String, String)
)
ENGINE = MergeTree()
ORDER BY (`Id`)
PRIMARY KEY (`id`, `timestamp`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260612142624_TotalResetCreate', '10.0.9');

