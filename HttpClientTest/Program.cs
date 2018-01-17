using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HttpClientTest {
    internal class Program {
        private static void Main(string[] args) {
            Console.WriteLine("HttpClient test!");
            var serverUrl = "http://localhost:54249/";
            var data = HttpClientCore.Get<List<string>>(serverUrl, "values");
            Console.WriteLine($"httpClinet get Success.count:{data.Result.Count} ");

            try {
                var error = HttpClientCore.Get<string>(serverUrl, "Values/GetError", new[] {"input"}, new[] {"错误"});
                Console.WriteLine($"获取错误：{error.Result}");
            }
            catch (Exception e) {
                Console.WriteLine(e.GetBaseException().Message);
            }

            var employees = new List<Employee> {
                new Employee {Id = Guid.NewGuid(), Name = "马虎维", Status = 0},
                new Employee {Id = Guid.NewGuid(), Name = "张智强", Status = 0}
            };

            var postData = HttpClientCore.Post<List<Employee>>(serverUrl, "values", employees);
            Console.WriteLine($"Post success.\n{JsonConvert.SerializeObject(postData.Result)}");


            try {
                postData = HttpClientCore.Post<List<Employee>>(serverUrl, "values", "  ");
                Console.WriteLine($"Post success.\n{JsonConvert.SerializeObject(postData.Result)}");
            }
            catch (Exception e) {
                Console.WriteLine($"Post Error:{e.GetBaseException().Message}");
            }
            Console.ReadLine();
        }
    }
}