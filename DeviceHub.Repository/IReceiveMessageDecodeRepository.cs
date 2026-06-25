using DeviceHub.Model.Entities;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息解码结果表数据访问接口
/// </summary>
public interface IReceiveMessageDecodeRepository
{
    Task<long> InsertAsync(ReceiveMessageDecode entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ReceiveMessageDecode entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessageDecode?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessageDecode?> GetByReceiveMessageIdAsync(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessageDecode>> GetAllAsync(CancellationToken cancellationToken = default);
}
