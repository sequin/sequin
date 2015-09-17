namespace Sequin.Core.Infrastructure
{
    public interface ITypeResolver
    {
        T Get<T>();
    }
}