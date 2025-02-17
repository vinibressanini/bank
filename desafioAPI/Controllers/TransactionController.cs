using desafioAPI.DTO;
using desafioAPI.Exceptions;
using desafioAPI.Models;
using desafioAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace desafioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        private readonly TransactionService _service;

        public TransactionController(TransactionService service) => _service = service;



        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post(TransactionPostRequestBody transaction)
        {

            try
            {

                await _service.MakeTransaction(transaction);

                return Ok("The transaction was successfully made");

            }
            catch (TransferException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerMessage = ex.InnerException.Message,
                    from = ex.From,
                    to = ex.To,
                    amount = ex.Amount,

                });
            }
            catch (AuthorizationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    ex.transaction
                });
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int walletId)
        {
            try
            {
                var transactions = await _service.GetAllUserTransactions(walletId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
