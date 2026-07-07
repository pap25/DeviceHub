using DeviceHub.Model.Entities;
using DeviceHub.Model.view;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息队列表数据访问接口
/// </summary>
public interface ISendMessageRepository
{
    Task<long> Insert(SendMessage entity, CancellationToken cancellationToken = default);

    Task<bool> Update(SendMessage entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAndErrorMessageAndUpdateTimeById(
        long id,
        SendMessage.StatusEnum status,
        string errorMessage,
        long updateTime,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAndUpdateTimeById(
        long id,
        SendMessage.StatusEnum status,
        long updateTime,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<SendMessage?> GetById(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetAll(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetByStatus(SendMessage.StatusEnum status, CancellationToken cancellationToken = default);

    Task<int> findCount(long instrumentId, SendMessage.StatusEnum? status,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default);

    Task<List<SendMessageView>> findPageDesc(long instrumentId, SendMessage.StatusEnum? status,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default);
}
