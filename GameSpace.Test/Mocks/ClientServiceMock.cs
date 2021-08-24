//using GameSpace.Services.HttpClients.Contracts;

//using Moq;

//using MyTested.AspNetCore.Mvc;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace GameSpace.Test.Mocks
//{
//    public class ClientServiceMock
//    {
//        public static IClientService Instance
//        {
//            get
//            {
//                var mock = new Mock<IClientService>();

//                    var response = new HttpResponseMessage();

//                mock
//                    .Setup(c => c.ReadMessage(With.Any<string>(), With.Any<string>()))
//                    .Returns(response);

//                return mock.Object;
//            }
//        }
//}