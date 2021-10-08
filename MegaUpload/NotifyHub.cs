using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaUpload
{
    public class NotifyHub : Hub
    {
        public async Task SendProgress(int p)
        {
            await Clients.All.SendAsync("receiveProgress",p);
        }
    }
}
