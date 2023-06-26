using LeadDBManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LeadDBManagement.Model;
using System.Threading.Tasks;
using Azure.Identity;

namespace LeadDBManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadDataController : ControllerBase
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILogger _logger;
        public LeadDataController(ILeadRepository leadRepository, ILogger<LeadDataController> logger)
        {
            _leadRepository = leadRepository;
            _logger = logger;

        }
        [HttpPost]
        public async Task<ActionResult<Leaddata>> CreateLeadAsync([FromBody]Leaddata lead)
        {
            try
            {
                Leaddata newlead = new Leaddata
                {
                    id = lead.id,
                    contact = new Contact()
                    {
                        emailId = lead.contact?.emailId,
                        firstName = lead.contact?.firstName,
                        lastName = lead.contact?.lastName,
                    },
                    primaryProduct = lead.primaryProduct,
                    programId = lead.programId,
                    score = new Score()
                    {
                        score = lead.score?.score,
                        scoreUpdateTimestamp = lead.score?.scoreUpdateTimestamp,
                        rating = lead.score?.rating,
                    }

                };

                var createdTask = await _leadRepository.CreateLeadAsync(newlead);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.programId }, createdTask);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding data");

                Console.WriteLine(ex.Message);
            }
            return Ok();
               
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Leaddata>> GetTaskById(string id)
        {
            var lead = await _leadRepository.GetLeadByIdAsync(id);
            try
            {          
                if (lead == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occured");
            }
            return Ok(lead);
        }
        [HttpGet]
        public async Task<ActionResult<List<Leaddata>>> GetAllTasks()
        {
            var leads = await _leadRepository.GetAllLeadsAsync();
            return Ok(leads);
        }

    }
}
