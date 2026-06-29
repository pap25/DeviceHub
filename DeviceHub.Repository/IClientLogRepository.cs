using DeviceHub.Model.Entities;

namespace DeviceHub.Repository.Interfaces;

/// <summary>
/// 仪器通信日志表数据访问接口
/// </summary>
public interface IClientLogRepository
{
    Task<long> Insert(ClientLog entity, CancellationToken cancellationToken = default);

    Task<bool> Update(ClientLog entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteById(long id, CancellationToken cancellationToken = default);

    Task<ClientLog?> GetById(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClientLog>> GetAll(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClientLog>> GetByType(ClientLog.TypeEnum type, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClientLog>> GetByLevel(ClientLog.LevelEnum level, CancellationToken cancellationToken = default);

    Task<int> findCount(ClientLog.LevelEnum? level, ClientLog.TypeEnum? type,
        string message, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default);

    Task<List<ClientLog>> findPageDesc(ClientLog.LevelEnum? level, ClientLog.TypeEnum? type,
        string message, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default);
}
