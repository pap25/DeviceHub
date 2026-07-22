using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息队列表扩展表数据访问接口
/// </summary>
public interface ISendMessageLargeRepository
{
    Task<bool> Insert(SendMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> Insert(
        SendMessageLarge entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default);

    Task<bool> Update(SendMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default);

    Task<SendMessageLarge?> GetBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessageLarge>> GetAll(CancellationToken cancellationToken = default);
}
