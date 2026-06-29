using DeviceHub.Model.Entities;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息队列表扩展表数据访问接口
/// </summary>
public interface IReceiveMessageLargeRepository
{
    Task<bool> InsertAsync(ReceiveMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ReceiveMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByReceiveMessageIdAsync(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<ReceiveMessageLarge?> GetByReceiveMessageIdAsync(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessageLarge>> GetAllAsync(CancellationToken cancellationToken = default);
}
