using System.Threading.Tasks;

namespace Itixo.Timesheets.Shared.Messaging;

public interface IMessageHandler<in TMessage>
{
    Task ProcessMessage(TMessage data);
}

public interface IPublisher<in TMessage>
{
    public Task PublishMessage(TMessage data);
}
