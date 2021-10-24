using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinBot.Service.Options
{
    public class SecretsOptions
    {
        public string XAuthToken { get; set; }

        public string PersistentDeviceId { get; set; }

        public string UserSessionId { get; set; }

        public string AppSessionId { get; set; }
    }
}