using DeviceHub.Model.Entities;
using DeviceHub.Model.view;

namespace DeviceHub.Repository;

/// <summary>
/// 接收仪器消息队列表数据访问接口
/// </summary>
public interface IReceiveMessageRepository
{
    Task<long> Insert(ReceiveMessage entity, CancellationToken cancellationToken = default);

    Task<bool> Update(ReceiveMessage entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAndErrorMessageAndUpdateTimeById(
        long id,
        ReceiveMessage.StatusEnum status,
        string errorMessage,
        long updateTime,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAndUpdateTimeById(
        long id,
        ReceiveMessage.StatusEnum status,
        long updateTime,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<ReceiveMessage?> GetById(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetAll(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetByInstrumentId(long instrumentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReceiveMessage>> GetByStatus(ReceiveMessage.StatusEnum status, CancellationToken cancellationToken = default);

    Task<List<ReceiveMessage>> FindByInstrumentIdAndStatusOrderAsc(long instrumentId, ReceiveMessage.StatusEnum status, int limit, CancellationToken cancellationToken = default);

    Task<int> findCount(long instrumentId, ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default);

    Task<List<ReceiveMessageView>> findPageDesc(long instrumentId, ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default);
}
