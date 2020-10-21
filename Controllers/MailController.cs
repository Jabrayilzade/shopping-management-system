using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShoppingDB.Models;

namespace ShoppingDB.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly Services.IMailService mailService;
        public MailController(Services.IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
