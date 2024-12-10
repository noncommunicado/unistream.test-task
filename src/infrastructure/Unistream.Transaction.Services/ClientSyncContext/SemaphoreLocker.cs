using Unistream.Transaction.Domain.Interfaces;

namespace Unistream.Transaction.Services.ClientSyncContext;

public class SemaphoreLocker : ILocker
{
	private readonly SemaphoreSlim _semaphore;

	public SemaphoreLocker(SemaphoreSlim semaphore)
	{
		_semaphore = semaphore;
	}

	public void Dispose()
	{
		_semaphore.Release();
	}

	public async ValueTask DisposeAsync()
	{
		_semaphore.Release();
	}
}