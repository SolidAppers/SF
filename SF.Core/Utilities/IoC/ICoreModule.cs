using Microsoft.Extensions.DependencyInjection;

namespace SF.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection collection);
    }
}
