
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;

namespace RestSharpTest
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        public object JsonConvert { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees/list", Method.GET);

            //act

            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>Ons the calling get API return employee list.</summary>
        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(4, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }


        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "15000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(15000, dataResponse.Salary);

        }

        [TestMethod]
        public void givenMultipleEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();

            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "15000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(15000, dataResponse.Salary);

        }

        [TestMethod]
        public void givenEmployeeSalary_OnUpdate_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees/4", Method.PUT);
            JObject jObjectbody = new JObject();

            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "30000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(30000, dataResponse.Salary);

        }
        [TestMethod]
        public void givenEmployeeID_SholudDeleteEmployee()
        {
            RestRequest request = new RestRequest("/employees/2", Method.DELETE);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

        }
    }

}