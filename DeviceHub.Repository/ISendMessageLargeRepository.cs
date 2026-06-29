using DeviceHub.Model;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息队列表扩展表数据访问接口
/// </summary>
public interface ISendMessageLargeRepository
{
    Task<bool> InsertAsync(SendMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(SendMessageLarge entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteBySendMessageIdAsync(long sendMessageId, CancellationToken cancellationToken = default);

    Task<SendMessageLarge?> GetBySendMessageIdAsync(long sendMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessageLarge>> GetAllAsync(CancellationToken cancellationToken = default);
}
