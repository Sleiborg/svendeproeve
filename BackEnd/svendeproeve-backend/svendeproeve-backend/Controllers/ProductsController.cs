using Microsoft.AspNetCore.Mvc;
using svendeproeve_backend.Data;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using svendeproeve_backend.Models;
using Microsoft.AspNetCore.SignalR;
using svendeproeve_backend.Hubs;
using Microsoft.AspNetCore.Authorization;
using svendeproeve_backend.Dtos;
using Mapster;

namespace svendeproeve_backend.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IHubContext<productsHub> _hubContext;

        private readonly Databasedcontext databasedcontext;

        public ProductsController(Databasedcontext databasedcontext, IHubContext<productsHub> _hubContext)
        {
            this.databasedcontext = databasedcontext;
            this._hubContext = _hubContext;
        }

        [HttpGet("{id}")]
        public IActionResult index(int id)
        {
            var products = databasedcontext.Products.FirstOrDefault(i => i.productId == id);
            return Ok(products);
        }

        [HttpGet]
        public IActionResult index()
        {
            var products = databasedcontext.Products;
            return Ok(products);
        }

        [HttpPost("create")]
        public async Task<IActionResult> create([FromForm] CreateProjectDto dto)
        {
            var model = dto.Adapt<Product>();
            model.image = await SaveBase64File(dto.Base64image);
            databasedcontext.Products.Add(model);
            databasedcontext.SaveChanges();
            await _hubContext.Clients.All.SendAsync("ReceiveProducts", model);
            return Ok(model.productId);
        }

        [HttpDelete("delete/id")]
        public IActionResult deleteProduct(int id)
        {
            var product = databasedcontext.Products.FirstOrDefault(i => i.productId == id);
            databasedcontext.Products.Remove(product);
            databasedcontext.SaveChanges();
            return Ok();
        }

        public async Task<string> SaveBase64File(string base64File)
        {
            string ext = base64File.Split(',')[0];
            string base64 = base64File.Split(',')[1];

            string fileName = Guid.NewGuid().ToString();

            if (ext.Contains("png"))
                fileName += ".png";

            if (ext.Contains("jpg"))
                fileName += ".jpg";

            byte[] imageBytes = Convert.FromBase64String(base64);

            try
            {
                await System.IO.File.WriteAllBytesAsync($@".\wwwroot\media\images\{fileName}", imageBytes);
            }
            catch (Exception e)
            {
                throw;
            }

            return fileName;
        }
    }
}
