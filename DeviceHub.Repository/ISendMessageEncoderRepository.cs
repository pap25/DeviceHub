using DeviceHub.Model.Entities;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息编码记录数据访问接口
/// </summary>
public interface ISendMessageEncoderRepository
{
    Task<long> Insert(SendMessageEncoder entity, CancellationToken cancellationToken = default);

    Task<bool> Update(SendMessageEncoder entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<SendMessageEncoder?> GetById(long id, CancellationToken cancellationToken = default);

    Task<SendMessageEncoder?> GetBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessageEncoder>> GetAll(CancellationToken cancellationToken = default);
}
