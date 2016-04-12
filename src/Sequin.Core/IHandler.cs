namespace Sequin.Core
{
    using System.Threading.Tasks;

    public interface IHandler<in T>
    {
        Task Handle(T command);
    }
}