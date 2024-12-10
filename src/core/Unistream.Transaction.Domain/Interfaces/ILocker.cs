namespace Unistream.Transaction.Domain.Interfaces;

/// <summary>
/// При вызове Dispose данного объекта вызывается освобождение блокировки
/// </summary>
public interface ILocker : IDisposable, IAsyncDisposable { }