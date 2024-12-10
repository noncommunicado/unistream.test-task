namespace Unistream.Transaction.Domain.Interfaces;

public interface ILockerProvider<TLockObject> : IDisposable
{
	ILocker Lock(TLockObject lockObject);
	Task<ILocker> LockAsync(TLockObject cllockObjectientId, CancellationToken ct = default);
	void ReleaseClient(TLockObject lockObject);
}