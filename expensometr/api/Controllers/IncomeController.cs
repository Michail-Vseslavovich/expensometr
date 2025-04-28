using expense_service.application.Services;
using expense_service.domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


//TODO: Убери копипаст хз как (мб атрибуты)

namespace expense_service.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController(IncomeService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<List<Income>> GetAllIncomeById()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            return Ok(service.GetAllByUserId(UIserId));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddIncome(Income income)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (UIserId == income?.RelatedUserId)
            {
                if (await service.Post(income))
                {
                    return Ok();
                }
                return BadRequest("Шото не так");
            }
            return Forbid("Ты в чужие дела не лезь");
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateIncomeInfo(Income income)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if ( UIserId == income?.RelatedUserId)
            {
                if (await service.Update(income))
                {
                    return Ok();
                }
                return BadRequest("Шото не так");
            }
            return Forbid("Ты в чужие дела не лезь");
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteById(int IncomeId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (await service.Delete(IncomeId, UIserId))
            {
                return Ok();
            }
            return BadRequest("Такого нет");

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CalculateTotalStatistic()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (UIserId != 0)
            {
                return Ok(await service.CalculateTotalStatistic(UIserId));
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CalculateMonthStatistic()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (UIserId != 0)
            {
                return Ok(await service.CalculateMonthStatistic(UIserId));
            }
            return BadRequest();
        }
    }
}
