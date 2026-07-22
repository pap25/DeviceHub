using DeviceHub.Model.Entities;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息解码结果表数据访问接口
/// </summary>
public interface IReceiveMessageDecodeRepository
{
    Task<long> Insert(ReceiveMessageDecode entity, CancellationToken cancellationToken = default);

    Task InsertForUpdateByReceiveMessageId(ReceiveMessageDecode entity, CancellationToken cancellationToken = default);

    Task<bool> Update(ReceiveMessageDecode entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessageDecode?> GetById(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessageDecode?> GetByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessageDecode>> GetAll(CancellationToken cancellationToken = default);
}
