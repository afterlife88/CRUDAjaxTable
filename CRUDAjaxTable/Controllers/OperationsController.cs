using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CRUDAjaxTable.Data;
using CRUDAjaxTable.Models;

namespace CRUDAjaxTable.Controllers
{
    public class OperationsController : ApiController
    {
        private readonly IRepository<Operation> _repository;
        public OperationsController() : this(new OperationRepository()) { }
        public OperationsController(IRepository<Operation> repository)
        {
            _repository = repository;
        }
        // GET api/operations
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            if (items != null)
                return Ok(items);
            return NotFound();
        }
        //GET api/operations/{id}
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var item = await _repository.GetAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        //POST api/operations/post

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]Operation operation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _repository.AddAsync(operation);
            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody] Operation operation)
        {
            if (operation != null)
            {
                await _repository.UpdateAsync(operation);
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var item = await _repository.GetAsync(id);
            if (item != null)
                await _repository.Delete(id);
            else
                return NotFound();
            return Ok();
        }
        [Route("api/operation/odata")]
        public IQueryable<Operation> GetAllQueryable()
        {
            var categories = _repository.GetAllAsync().Result.AsQueryable();
            return categories;
        }
      
    }
}
