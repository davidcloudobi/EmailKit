using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EmailService
{
   public class MessageDTO
    {
        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachments { get; set; }

    }
}
