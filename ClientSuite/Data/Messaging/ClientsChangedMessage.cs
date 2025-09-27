using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClientSuite.Data.Messaging
{
    public sealed class ClientsChangedMessage : ValueChangedMessage<bool>
    {
        public ClientsChangedMessage() : base(true) { }
    }
}
