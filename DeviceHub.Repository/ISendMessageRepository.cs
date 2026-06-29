using DeviceHub.Model.Entities;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息队列表数据访问接口
/// </summary>
public interface ISendMessageRepository
{
    Task<long> InsertAsync(SendMessage entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(SendMessage entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<SendMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetByStatusAsync(SendMessage.StatusEnum status, CancellationToken cancellationToken = default);
}
