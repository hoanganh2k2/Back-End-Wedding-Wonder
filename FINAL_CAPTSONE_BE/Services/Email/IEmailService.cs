﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(Message message);
    }
}
