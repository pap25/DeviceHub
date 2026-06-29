using DeviceHub.Model;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息队列表数据访问接口
/// </summary>
public interface IReceiveMessageRepository
{
    Task<long> InsertAsync(ReceiveMessage entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ReceiveMessage entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetByInstrumentIdAsync(long instrumentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetByStatusAsync(ReceiveMessage.StatusEnum status, CancellationToken cancellationToken = default);
}
