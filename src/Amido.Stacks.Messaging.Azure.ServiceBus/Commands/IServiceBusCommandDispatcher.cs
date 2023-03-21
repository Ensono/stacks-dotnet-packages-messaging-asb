using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Commands
{
    public interface IServiceBusCommandDispatcher
    {
        Task SendAsync(IServiceBusCommand command);
    }
}
