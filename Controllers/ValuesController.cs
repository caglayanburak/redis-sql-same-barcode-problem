using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisSample.Models;

namespace RedisSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private ApplicationDbContext _context;
        public ValuesController(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;

        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{barcode}/{jobId}")]
        public ActionResult<Task> Get(string barcode, int jobId)
        {
            var redisKey = $"task_{jobId}_{barcode}";
            List<string> barcodes = new List<string>();

            //çekilen unique data redis e atılır.
            var anyToLocationWithThisBarcode = _distributedCache.Get(redisKey).FromByteArray<List<string>>();
            IQueryable<Task> taskQuery = _context.Task;
            if (anyToLocationWithThisBarcode != null)
            {
                barcodes = anyToLocationWithThisBarcode;
                taskQuery = taskQuery.Where(x => !anyToLocationWithThisBarcode.Contains(x.ToLocation));
            }


            var task = taskQuery.FirstOrDefault(x => x.Barcode == barcode && x.TaskTypeId == 8 && x.StatusId == 1);

            if (anyToLocationWithThisBarcode != null)
            {
                barcodes.Add(task.ToLocation);

            }

            _distributedCache.Set(redisKey, barcodes.ToByteArray());

            return task;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
