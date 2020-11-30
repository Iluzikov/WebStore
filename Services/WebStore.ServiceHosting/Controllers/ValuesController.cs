using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Тестовый API</summary>
    [Route("api/v1/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> _values = Enumerable.Range(1, 10)
            .Select(i => $"Value {i}")
            .ToList();

        /// <summary>
        /// Получение списка значений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get() => _values;

        /// <summary>
        /// Получение значения по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор значения</param>
        /// <returns>Значение</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id >= _values.Count)
                return NotFound();

            return _values[id];
        }

        /// <summary>
        /// Добавить значение в список
        /// </summary>
        /// <param name="value">Значение</param>
        [HttpPost]
        public void Post([FromBody] string value) => _values.Add(value);

        /// <summary>
        /// Редактирование значения
        /// </summary>
        /// <param name="id">Идентификатор значения</param>
        /// <param name="value">Значение</param>
        /// <returns>Истина если редактирование успешно</returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0) return BadRequest();
            if (id >= _values.Count) return NotFound();

            _values[id] = value;
            return Ok();
        }

        /// <summary>
        /// Удаление значения из списка
        /// </summary>
        /// <param name="id">Идентификатор значения</param>
        /// <returns>Истина если удаление успешно</returns>
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
