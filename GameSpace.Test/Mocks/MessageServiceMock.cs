using GameSpace.Services.Messages.Contracts;

using Moq;

namespace GameSpace.Test.Mocks
{
    public class MessageServiceMock
    {
        public static IMessageService Instance
        {
            get
            {
                var mock = new Mock<IMessageService>();

                mock.Setup(ms => ms.ClearAsync(1));

                return mock.Object;
            }
        }
    }
}