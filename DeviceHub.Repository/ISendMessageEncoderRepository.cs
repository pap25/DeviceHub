using DeviceHub.Model;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息编码记录数据访问接口
/// </summary>
public interface ISendMessageEncoderRepository
{
    Task<long> InsertAsync(SendMessageEncoder entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(SendMessageEncoder entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<SendMessageEncoder?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<SendMessageEncoder?> GetBySendMessageIdAsync(long sendMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessageEncoder>> GetAllAsync(CancellationToken cancellationToken = default);
}
