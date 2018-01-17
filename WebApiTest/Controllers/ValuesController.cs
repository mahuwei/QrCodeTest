using System.Collections.Generic;
using System.Linq;
using HttpClientTest;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers {
    [Route("api/[controller]")]
    public class ValuesController : Controller {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        [HttpGet]
        [Route("GetError")]
        public IActionResult GetError(string input) {
            return BadRequest($"返回错误；输入参数input={input}");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] List<Employee> employees) {
            if (employees == null || employees.Any() == false) {
                return BadRequest("传入参数错误");
            }
            foreach (var employee in employees) {
                employee.Status++;
            }

            return Ok(employees);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}