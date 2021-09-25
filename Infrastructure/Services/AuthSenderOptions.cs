using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthSenderOptions
    {
        private readonly string user = "Food Delivery";

        private readonly string key = "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";
        public string SendGridUser { get { return user; } }
        public string SendGridKey { get { return key; } }
    }
}
