using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using CRUDAjaxTable;
using CRUDAjaxTable.Controllers;
using CRUDAjaxTable.Data;
using CRUDAjaxTable.Models;
using Moq;
using NUnit.Framework;

namespace TemplateCRUD.Tests
{
    [TestFixture]
    public class OperationsControllerTest
    {
        private Mock<IRepository<Operation>> _operationRepository;

        [SetUp]
        public void SetUp()
        {
            _operationRepository = new Mock<IRepository<Operation>>();
        }
        [Test]
        public void Get_All_Returns_Operations()
        {
            IEnumerable<Operation> fakeOperation = GetOperations();
            _operationRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeOperation);
            OperationsController controller = new OperationsController(_operationRepository.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };
            var operations = controller.GetAllQueryable();
            Assert.IsNotNull(operations, "Result is null");
            Assert.IsInstanceOf(typeof(IEnumerable<Operation>), operations, "Wrong Model");
            Assert.AreEqual(4, operations.Count(), "Got wrong number of Operations");
        }
        [Test]
        public void Get_CorrectOperationId_Returns_Operation()
        {
            IEnumerable<Operation> fakeOperation = GetOperations();
            _operationRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeOperation);
            OperationsController controller = new OperationsController(_operationRepository.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };
            var operation = controller.Get(4);
            Assert.IsNotNull(operation);
            Assert.AreEqual(4, operation.Id, "Got wrong number of Operations");
        }
        [Test]
        public void Get_InvalidOperationId_Return_NotFound()
        {
            IEnumerable<Operation> fakeOperation = GetOperations();
            _operationRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeOperation);
            OperationsController controller = new OperationsController(_operationRepository.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };
            var operation = controller.Get(4);
            Assert.IsInstanceOf<NotFoundResult>(operation.Result);
        }
        [Test]
        public void Post_Operation_Returns_CreatedStatusCode()
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary
                {
                    {"controller", "operations"}
                });
            var controller = new OperationsController(_operationRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/operations/")
                {
                    Properties =
                    {
                        {HttpPropertyKeys.HttpConfigurationKey, httpConfiguration},
                        {HttpPropertyKeys.HttpRouteDataKey, httpRouteData}
                    }
                }
            };
            Operation operation = new Operation
            {
                Author = new Author() { Name = "oleg" },
                Cost = 240,
                Id = 1,
                TypeOperation = TypeOperation.Income,
                Description = "some bla bla"
            };

            var response = controller.Post(operation).Result;
            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }
        [Test]
        public void Post_Operation_Returns_BadRequestStatusCode()
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary
                {
                    {"controller", "operations"}
                });
            var controller = new OperationsController(_operationRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/operations/")
                {
                    Properties =
                    {
                        {HttpPropertyKeys.HttpConfigurationKey, httpConfiguration},
                        {HttpPropertyKeys.HttpRouteDataKey, httpRouteData}
                    }
                }
            };
            Operation operation = new Operation();

            controller.ModelState.AddModelError("", "mock error message");

            var response = controller.Post(operation);

            Assert.IsInstanceOf<InvalidModelStateResult>(response.Result);

        }


        public static IEnumerable<Operation> GetOperations()
        {
            IEnumerable<Operation> fakeOperations = new List<Operation>()
            {
                new Operation()
                {
                    TypeOperation = TypeOperation.Income,
                    Author = new Author() {Name = "Vasya"},
                    Description = "bla bla",
                    Cost = 240,
                    Id = 1
                },
                new Operation()
                {
                    TypeOperation = TypeOperation.OutCome,
                    Author = new Author() {Name = "Petya"},
                    Description = "bla bla",
                    Cost = 320,
                    Id = 2
                },
                new Operation()
                {
                    TypeOperation = TypeOperation.OutCome,
                    Author = new Author() {Name = "Kolya"},
                    Description = "bla bla",
                    Cost = 2000,
                    Id = 3
                },
                  new Operation()
                {
                    TypeOperation = TypeOperation.Income,
                    Author = new Author() {Name = "petya"},
                    Description = "bla",
                    Cost = 2000,
                    Id = 4
                }
            }.AsEnumerable();

            return fakeOperations;
        }
    }
}
