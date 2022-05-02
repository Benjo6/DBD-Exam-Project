using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBD_Exam_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IGraphClient _client;

        public PersonController(IGraphClient client)
        {
            _client = client;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var items = await _client.Cypher.Match("(n: Person)")
                .Return(n => n.As<Person>()).ResultsAsync;

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _client.Cypher.Match("(p:Person)")
                                                  .Where((Person p) => p.id == id)
                                                  .Return(p => p.As<Person>()).ResultsAsync;

            return Ok(item.LastOrDefault());
        }
        // POST api/<MovieController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            await _client.Cypher.Create("(p:Person $per)")
                .WithParam("per", person)
                .ExecuteWithoutResultsAsync();

            return Ok();
       }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Person person)
        {
            await _client.Cypher.Match("(p:Person)")
                .Where((Person p) => p.id == id)
                .Set("p=$per")
                .WithParam("per", person)
                .ExecuteWithoutResultsAsync();
            return Ok();
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.Cypher.Match("(p:Person)")
                .Where((Person p) => p.id == id)
                .Delete("p")
                .ExecuteWithoutResultsAsync();
            return Ok();
        }

        [HttpPost("MakeFriends")]
        public async Task<IActionResult> MakeFriends(int id1, int id2)
        {
            await _client.Cypher.Match("(a:Person)", "(b:Person)")
                 .Where((Person a) => a.id == id1)
                 .AndWhere((Person b) => b.id == id2)
                 .Create("(a)-[:friends]->(b)")
                 .ExecuteWithoutResultsAsync();

            return Ok();
            
        }
        

    }
}
