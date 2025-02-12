using desafioAPI.DTO;
using desafioAPI.Models;
using desafioAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace desafioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly WalletService walletService;
        private readonly ILogger<WalletController> logger;

        public WalletController(WalletService service, ILogger<WalletController> logger)
        {
            walletService = service;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetBalance(int walletId)
        {

            try
            {
                logger.LogInformation("Calling GetBalance for Wallet {WalletId}", walletId);

                var balance = await walletService.GetWalletBalance(walletId);


                return Ok(new
                {
                    balance
                });

            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "No Wallet found for ID {WalletId}", walletId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error while retrieving wallet {WalletId} info", walletId);
                return StatusCode(500, "An unknown error ocurred");
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddBalance([FromBody] WalletAddBalanceDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await walletService.AddBalance(dto);

                    return Ok("Wallet balance successfully updated");

                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }


    }
}
