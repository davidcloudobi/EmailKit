using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Email.Controllers
{
    /// <summary>
    /// Version 1, User
    /// </summary>
    /// 
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class Email : ControllerBase
    {

        //###############################################
        private readonly EmailSender _emailSender;

        public Email(EmailSender emailSender)
        {

            _emailSender = emailSender;
        }



        // POST: api/email/syncEmail
        /// <summary>
        ///  Return a void
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>
        /// Sample request (this request sends mail)
        /// </remarks>
        /// <response code="200">Returns  a void </response>
        [HttpPost("syncEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult SyncEmail(MessageDTO message)
        {
            // var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();



            // var message = new Message(new string[] { "davidcloudobi@gmail.com" }, "Test email", "This is the content from our email.");
            var response =   _emailSender.SendEmail(message);
           // var response =  _emailSender.SendEmail(message, files);

           if (response == true)
           {
               return Ok("Message sent");
           }
           else
           {
               return BadRequest("Message not sent");
           }


        }



        // POST: api/email/asyncEmail
        /// <summary>
        ///  Return a void
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>
        /// Sample request (this request sends mail)
        /// </remarks>
        /// <response code="200">Returns  a void </response>

        [HttpPost("asyncEmail")]
        public async Task<IActionResult> AsyncEmail(MessageDTO message)
        {
           // var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
          

           // var message = new Message(new string[] { "davidcloudobi@gmail.com" }, "Test mail with Attachments", "This is the content from our mail with attachments.", files);
            var response = await _emailSender.SendEmailAsync(message);

            // var message = new Message(new string[] { "davidcloudobi@gmail.com" }, "Test email async", "This is the content from our async email.");
           // var response =   await _emailSender.SendEmailAsync(message,files);


         if (response == true)
         {
             return Ok("Message sent");
         }
         else
         {
             return BadRequest("Message not sent");
         }


          

        }







    }
}
