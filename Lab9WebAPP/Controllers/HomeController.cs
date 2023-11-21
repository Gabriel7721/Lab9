using Lab9Lib;
using Lab9WebAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Lab9WebAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly string url = "https://localhost:7250/api/Products";
        private HttpClient httpClient = new HttpClient();
      

        public IActionResult Index()
        {
            var model = JsonConvert.DeserializeObject<List<Product>>(httpClient.GetStringAsync(url).Result);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            try
            {
                if(file != null)
                {
                    var path = Path.Combine("wwwroot/images", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    file.CopyToAsync(stream);
                    product.Photo = "images/" + file.FileName;
                    var model = httpClient.PostAsJsonAsync(url, product).Result;
                    if(model.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
