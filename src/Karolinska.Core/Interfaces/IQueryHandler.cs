namespace Karolinska.Core.Interfaces
{
    public interface IQueryHandler<U,T>
    {
        Task<T?> HandleAsync(U query, CancellationToken cancellationToken);
    }
}
