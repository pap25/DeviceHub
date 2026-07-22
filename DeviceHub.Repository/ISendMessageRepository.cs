using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository;

/// <summary>
/// 发送仪器消息队列表数据访问接口
/// </summary>
public interface ISendMessageRepository
{
    Task<long> Insert(SendMessage entity, CancellationToken cancellationToken = default);

    Task<long> Insert(
        SendMessage entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default);

    Task<long> InsertForUpdateByExternalNo(SendMessage entity, CancellationToken cancellationToken = default);

    Task<long> InsertForUpdateByExternalNo(
        SendMessage entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default);

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

    Task<bool> UpdateStatusAndUpdateTimeById(
        long id,
        SendMessage.StatusEnum status,
        long updateTime,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<SendMessage?> GetById(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetAll(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SendMessage>> GetByStatus(SendMessage.StatusEnum status, CancellationToken cancellationToken = default);

    Task<SendMessage?> FindFirstByInstrumentIdAndStatusOrderAsc(
        long instrumentId,
        SendMessage.StatusEnum status,
        CancellationToken cancellationToken = default);

    Task<List<SendMessage>> FindByInstrumentIdAndStatusOrderAsc(
        long instrumentId,
        SendMessage.StatusEnum status,
        int limit,
        CancellationToken cancellationToken = default);

    Task<int> findCount(long instrumentId, SendMessage.StatusEnum? status, SendMessage.TypeEnum? type,
        string barcode, string sampleNo, string externalNo, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default);

    Task<List<SendMessageView>> findPageDesc(long instrumentId, SendMessage.StatusEnum? status, SendMessage.TypeEnum? type,
        string barcode, string sampleNo, string externalNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default);
}
