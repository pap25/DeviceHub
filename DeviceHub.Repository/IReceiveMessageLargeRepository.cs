using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息队列表扩展表数据访问接口
/// </summary>
public interface IReceiveMessageLargeRepository
{
    Task<bool> Insert(ReceiveMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> Insert(
        ReceiveMessageLarge entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default);

    Task<bool> Update(ReceiveMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<ReceiveMessageLarge?> GetByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessageLarge>> GetAll(CancellationToken cancellationToken = default);
}
