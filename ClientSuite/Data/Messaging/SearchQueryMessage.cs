using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClientSuite.Data.Messaging
{
    public sealed class SearchQueryMessage : ValueChangedMessage<string>
    {
        public SearchQueryMessage(string value) : base(value) { }
    }
}
