using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync(ICommand command);
    }
}
