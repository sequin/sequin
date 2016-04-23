namespace Sequin
{
    using System.Threading.Tasks;

    public interface IHandler<in T>
    {
        Task Handle(T command);
    }
}