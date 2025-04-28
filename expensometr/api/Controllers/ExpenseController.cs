using expense_service.application.Services;
using expense_service.domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace expense_service.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(ExpenseService service) : ControllerBase
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
        public async Task<ActionResult> AddExpense(Expense expense)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (UIserId == expense?.RelatedUserId)
            {
                if (await service.Post(expense))
                {
                    return Ok();
                }
                return BadRequest("Шото не так");
            }
            return Forbid("Ты в чужие дела не лезь");
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateExpenseInfo(Expense expense)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (UIserId == expense?.RelatedUserId)
            {
                if (await service.Update(expense))
                {
                    return Ok();
                }
                return BadRequest("Шото не так");
            }
            return Forbid("Ты в чужие дела не лезь");
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteById(int ExpenseId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            if (await service.Delete(ExpenseId, UIserId))
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
