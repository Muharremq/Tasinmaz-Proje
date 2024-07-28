using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _service;

        public LogController(ILogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetAllLog()
        {
            var logs = await _service.ListLog();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> GetLogById(int id)
        {
            var log = await _service.GetLogById(id);
            if (log == null)
            {
                return NotFound();
            }
            return log;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> AddLog(Log log)
        {
            await _service.AddLog(log);
            return CreatedAtAction(nameof(GetLogById), new { id = log.Id }, log);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLog(int id, Log log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }

            await _service.UpdateLog(log);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            await _service.DeleteLog(id);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Log>> SearchLogs([FromQuery] string term)
        {
            var logs = _service.SearchLogs(term);
            if (logs == null || !logs.Any())
            {
                return NotFound("No logs found matching the search term.");
            }

            return Ok(logs);
        }
    }
}
