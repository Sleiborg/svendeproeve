using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace svendeproeve_backend.Hubs
{
    public class productsHub: Hub
    {
        public async Task GetProducts()
        {
            await Clients.All.SendAsync("ReceiveProducts");
        }
    }
}
