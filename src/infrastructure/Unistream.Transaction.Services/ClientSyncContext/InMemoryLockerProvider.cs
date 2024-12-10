using System.Collections.Concurrent;
using Unistream.Transaction.Domain.Interfaces;

namespace Unistream.Transaction.Services.ClientSyncContext;

/// <summary>
///     Данный класс призван чтобы организовать легковесную блокировку вне базы данных.
///		И, соответственно, не вызывать тяжеловесную блокировку в СУБД, да еще и на несколько таблиц,
///		при текущей архитектуре
/// </summary>
public class InMemoryLockerProvider : ILockerProvider<Guid>
{
	// semaphore-slim weight 64byte + Guid 16byte ~ 80byte per client
	// 80mb memory per 1'000'000 clients
	private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _clientSemaphores = new();
	private readonly TimeSpan _timeOut = TimeSpan.FromSeconds(10);

	public ILocker Lock(Guid lockObject)
	{
		var semaphore = _clientSemaphores.GetOrAdd(lockObject, _ => new SemaphoreSlim(1, 1));
		semaphore.Wait(_timeOut);
		return new SemaphoreLocker(semaphore);
	}

	public async Task<ILocker> LockAsync(Guid lockObject, CancellationToken ct = default)
	{
		var semaphore = _clientSemaphores.GetOrAdd(lockObject, _ => new SemaphoreSlim(1, 1));
		await semaphore.WaitAsync(_timeOut, ct);
		return new SemaphoreLocker(semaphore);
	}

	public void ReleaseClient(Guid lockObject)
	{
		if (_clientSemaphores.TryGetValue(lockObject, out var semaphore) is false) return;
		semaphore.Release();
	}

	// todo: create interval release of unused semaphores

	public void Dispose()
	{
		foreach (var keyValuePair in _clientSemaphores)
			keyValuePair.Value.Dispose();
	}
}