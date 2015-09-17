namespace Sequin.Core
{
    public interface IHandler<in T>
    {
        void Handle(T command);
    }
}