using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Korobochka.DTOs;

namespace Korobochka.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase where T : BaseDTO
    {
        // GET api/base
        [HttpGet]
        public abstract ActionResult<IEnumerable<T>> Get();

        // GET api/base/5
        [HttpGet("{id}")]
        public abstract ActionResult<T> Get(int id);

        // POST api/base
        [HttpPost]
        public abstract ActionResult<T> Post([FromBody] T value);

        // PUT api/base/5
        [HttpPut("{id}")]
        public abstract ActionResult<T> Put(int id, [FromBody] T value);

        // DELETE api/base/5
        [HttpDelete("{id}")]
        public abstract ActionResult<T> Delete(int id);

        protected BadRequestObjectResult CustomBadRequest(object error)
        {
            return BadRequest(new { Error = error });
        }

    }
}
