using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;
using TodoLibrary.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoData _data;
        private readonly int userid;
        public TodosController(ITodoData data, ILogger<TodosController> logger)
        {
            this._data = data;
           
            
        }

        private int GetUserId()
        {
            var useridText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(useridText);
        }

        // GET: api/<Todos>
        [HttpGet]
        public async Task<ActionResult<List<TodoModel>>> Get()
        {

            try
            {
                var output = await _data.GetAllAssigned(GetUserId());

                return Ok(output);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }

           

        }

        // GET api/<Todos>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoModel>> Get(int todId)
        {
            try
            {
                var output = await _data.GetOneAssigned(GetUserId(), todId);
                return Ok(output);
            }
            catch (Exception)
            {

                return BadRequest();
            }
           
        }

        // POST api/<Todos>
        [HttpPost]
        public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
        {
            var output = await _data.Create(GetUserId(), task);
            return Ok(output);
        }

        // PUT api/<Todos>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int todoId, [FromBody] string task)
        {
            await _data.UpdateTask(GetUserId(),todoId,task);
            return Ok();
        }

        // PUT api/<Todos>/5/Complete
        [HttpPut("{id}/Complete")]
        public async Task<IActionResult> Complete(int todoId)
        {
            await _data.CompleteTodo(GetUserId(), todoId);

            return Ok();
        }

        // DELETE api/<Todos>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int todoId)
        {
            await _data.Delete(GetUserId(), todoId);

            return Ok();
        }

      
    }
}
