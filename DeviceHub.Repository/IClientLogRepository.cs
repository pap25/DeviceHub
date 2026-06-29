using DeviceHub.Model.Entities;

namespace DeviceHub.Repository.Interfaces;

/// <summary>
/// 仪器通信日志表数据访问接口
/// </summary>
public interface IClientLogRepository
{
    Task<long> InsertAsync(ClientLog entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ClientLog entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<ClientLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClientLog>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClientLog>> GetByLevelAsync(ClientLog.LevelEnum level, CancellationToken cancellationToken = default);
}
