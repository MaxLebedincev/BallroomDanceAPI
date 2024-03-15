using BallroomDanceAPI.Controllers.TypeBallroomDanceInteraction;
using BallroomDanceAPI.DAL.Interfaces;
using BallroomDanceAPI.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BallroomDanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class TypeBallroomDanceController : ControllerBase
    {
        private readonly ILogger<TypeBallroomDanceController> _logger;
        private readonly AppSettings _conf;
        private readonly IUnitOfWork _unitOfWork;

        public TypeBallroomDanceController(IOptions<AppSettings> conf, ILogger<TypeBallroomDanceController> logger, IUnitOfWork unitOfWork)
        {
            _conf = conf.Value;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("api/[controller]/Create")]
        public async Task<ActionResult> Create([FromBody] TypeBallroomDanceRequest request)
        {
            var rep = _unitOfWork.GetRepository<TypeBallroomDance>();


            var newEntity = new TypeBallroomDance()
            {
                Name = request.Name,
            };

            rep.Create(newEntity);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("api/[controller]/Get")]
        public async Task<ActionResult<List<TypeBallroomDanceResponse>>> Get([FromBody] TypeBallroomDanceDTO request)
        {
            var rep = _unitOfWork.GetRepository<TypeBallroomDance>();

            var list = rep.GetAll();

            list = list.OrderBy(e => e.Id);

            if (!string.IsNullOrEmpty(request.Name))
                list = list
                    .Where(e => EF.Functions.Like(e.Name, $"%{request.Name}%"));

            list = list
                    .Skip(request.Offset)
                    .Take(request.Number);

            var paginatedList = await list.ToListAsync();

            var resopnse = new List<TypeBallroomDanceResponse>();

            foreach (var item in paginatedList)
                resopnse.Add(new TypeBallroomDanceResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                });

            return resopnse;
        }

        [HttpGet("api/[controller]/Get/{id}")]
        public async Task<ActionResult<TypeBallroomDanceResponse?>> GetById(int id)
        {
            var rep = _unitOfWork.GetRepository<TypeBallroomDance>();

            var entity = await rep.GetAll().Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                NotFound();

            var response = new TypeBallroomDanceResponse()
            {
                Id = entity.Id,
                Name = entity.Name,
            };

            return response;
        }

        [HttpDelete("api/[controller]/Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var rep = _unitOfWork.GetRepository<TypeBallroomDance>();

            var entity = await rep.GetAll().Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                return NotFound();

            rep.Delete(entity);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("api/[controller]/Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TypeBallroomDanceRequest newEntity)
        {
            var rep = _unitOfWork.GetRepository<TypeBallroomDance>();

            var entity = await rep.GetAll().Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                return NotFound();

            entity.Name = newEntity.Name;

            rep.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
