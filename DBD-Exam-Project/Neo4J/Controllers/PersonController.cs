using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4J.Controllers
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

        [HttpGet("check")]
        public async Task<bool> PermissionCheckAsync(int userid, int enteringid)
        {
            var item = await _client.Cypher.Match("p:PersonalDatum")
                .Where((PersonalDatum p) => p.Id == userid)
                .Return(() => new {RoleId = Return.As<int>("p.RoleId")}).ResultsAsync;

            if (item.FirstOrDefault().RoleId == enteringid)
                return true;

            return false;
            
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