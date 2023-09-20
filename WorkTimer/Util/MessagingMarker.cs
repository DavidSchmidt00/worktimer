using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WorkTimer.Util
{
    public class ActionMessage : ValueChangedMessage<string>
    {
        public ActionMessage(string value) : base(value)
        {
        }
    }
}
