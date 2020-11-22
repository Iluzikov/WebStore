using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v1/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> _values = Enumerable.Range(1, 10)
            .Select(i => $"Value {i}")
            .ToList();

        [HttpGet]
        public IEnumerable<string> Get() => _values;

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id >= _values.Count)
                return NotFound();

            return _values[id];
        }

        [HttpPost]
        public void Post([FromBody] string value) => _values.Add(value);

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0) return BadRequest();
            if (id >= _values.Count) return NotFound();

            _values[id] = value;
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            if (id >= _values.Count) return NotFound();

            _values.RemoveAt(id);
            return Ok();
        }
    }
}
