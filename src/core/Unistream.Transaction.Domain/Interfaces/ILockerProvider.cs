namespace Unistream.Transaction.Domain.Interfaces;

/// <summary>
/// Хоранит и распределяет объекты синхронизации по ключу типа TLockObject 
/// </summary>
public interface ILockerProvider<TLockObject> : IDisposable
{
	ILocker Lock(TLockObject lockObject);
	Task<ILocker> LockAsync(TLockObject cllockObjectientId, CancellationToken ct = default);
	void ReleaseClient(TLockObject lockObject);
}