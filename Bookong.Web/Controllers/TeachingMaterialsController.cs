using Microsoft.AspNetCore.Mvc;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.DTOs;

namespace Bookong.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachingMaterialsController : ControllerBase
    {
        private readonly ICreateTeachingMaterialUseCase _createTeachingMaterialUseCase;

        public TeachingMaterialsController(ICreateTeachingMaterialUseCase createTeachingMaterialUseCase)
        {
            _createTeachingMaterialUseCase = createTeachingMaterialUseCase;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTeachingMaterialDto dto)
        {
            var result = await _createTeachingMaterialUseCase.ExecuteAsync(dto);
            return Ok(result);
        }

        [HttpGet("folders")]
        public IActionResult GetFolders()
        {
            var folders = new[]
            {
                new {
                    Name = "Angličtina",
                    Description = "Materiály pro AJ",
                    CreatedBy = "Novák",
                    Files = new object[] {},
                    Links = new object[] {},
                    StudentMaterials = new object[] {}
                }
            };
            return Ok(folders);
        }
    }
}
