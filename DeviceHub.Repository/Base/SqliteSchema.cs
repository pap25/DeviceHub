namespace DeviceHub.Repository;

/// <summary>
/// SQLite 建表脚本（由 schema.sql 转换）
/// </summary>
internal static class SqliteSchema
{
    internal const string CreateTablesSql = """
        CREATE TABLE IF NOT EXISTS receive_message (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            instrument_id INTEGER NOT NULL,
            status INTEGER NOT NULL,
            error_message TEXT NOT NULL DEFAULT '',
            create_time INTEGER NOT NULL,
            update_time INTEGER NOT NULL
        );

        CREATE INDEX IF NOT EXISTS idx_receive_message_instrument_id ON receive_message(instrument_id);
        CREATE INDEX IF NOT EXISTS idx_receive_message_status ON receive_message(status);

        CREATE TABLE IF NOT EXISTS receive_message_large (
            receive_message_id INTEGER PRIMARY KEY,
            raw_message BLOB NOT NULL
        );

        CREATE TABLE IF NOT EXISTS receive_message_decode (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            receive_message_id INTEGER NOT NULL,
            external_no TEXT NOT NULL DEFAULT '',
            type INTEGER NOT NULL,
            sample_no TEXT NOT NULL DEFAULT '',
            barcode TEXT NOT NULL DEFAULT '',
            result_json TEXT NOT NULL,
            create_time INTEGER NOT NULL,
            update_time INTEGER NOT NULL,
            UNIQUE(receive_message_id),
            UNIQUE(external_no)
        );

        CREATE TABLE IF NOT EXISTS send_message (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            instrument_id INTEGER NOT NULL,
            type INTEGER NOT NULL,
            external_no TEXT NOT NULL DEFAULT '',
            sample_no TEXT NOT NULL DEFAULT '',
            barcode TEXT NOT NULL DEFAULT '',
            status INTEGER NOT NULL,
            error_message TEXT NOT NULL DEFAULT '',
            create_time INTEGER NOT NULL,
            update_time INTEGER NOT NULL,
            UNIQUE(external_no)
        );

        CREATE INDEX IF NOT EXISTS idx_send_message_status ON send_message(status);

        CREATE TABLE IF NOT EXISTS send_message_large (
            send_message_id INTEGER PRIMARY KEY,
            send_json TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS send_message_encoder (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            send_message_id INTEGER NOT NULL,
            send_content BLOB NOT NULL,
            create_time INTEGER NOT NULL,
            update_time INTEGER NOT NULL,
            UNIQUE(send_message_id)
        );

        CREATE TABLE IF NOT EXISTS dictionary (
            ckey TEXT PRIMARY KEY,
            value TEXT NOT NULL
        );
        """;
}
