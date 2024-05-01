using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyImageAPI.Data;
using MyImageAPI.Models;
using Newtonsoft.Json;

namespace MyImageAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var img = _context.ImgFiles;

            return View(img);
        }
        [HttpPost]
        public async Task<IActionResult> ApiFile()
        {
            //var MaxId = _context.ImgFiles.Max(i=>i.Id);https://source.unsplash.com/random/200x200?sig=1
            // string api = GetUrlToString($"http://www.splashbase.co/api/v1/images/{MaxId + 1}%22"); //to add to the database by adding plus 1 to the Max ID in the database each time.

            Uri api = GetUrlToString2("https://source.unsplash.com/random").Result; //To add images to the database as a random from the API.

            string url = api.AbsoluteUri;

            var imgFile = new ImgFile
            {
                ImgUrl = url,
            };
            await _context.ImgFiles.AddAsync(imgFile);
            await _context.SaveChangesAsync();
            return View();
        }

        # region First, images are saved in the device's memory, then pulled from the device, it can display the images Emin_ApiFile
        //[HttpPost]
        //public async Task<IActionResult> ApiFile()
        //{
        //    //string api = GetUrlToString2("https://source.unsplash.com/random").Result; //To add images to the database as a random from the API.

        //    //string imageSrc = GetUrlToString2("https://source.unsplash.com/random").Result;

        //    byte[] imageByteArr = GetUrlToByte("https://source.unsplash.com/random").Result;

        //    Guid guid = Guid.NewGuid();

        //    string fileName = $"random_image_{ guid}.jpeg";

        //    string path = $"D:\\Users\\ImageAPI\\MyImageAPI\\MyImageAPI\\wwwroot\\img\\{fileName}";


        //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        //    {
        //        fs.Write(imageByteArr, 0, imageByteArr.Length);
        //    }

        //    var imgFile = new ImgFile
        //    {
        //        ImgUrl = fileName,
        //    };
        //    await _context.ImgFiles.AddAsync(imgFile);
        //    await _context.SaveChangesAsync();
        //    return View();
        //}
#endregion

        private string GetUrlToString(string url)
        {
            String Response = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    Response = client.DownloadString(url);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return Response;
        }

        private async Task<Uri> GetUrlToString2(string url)
        {
            HttpResponseMessage response = null;
            try
            {

                // Create an HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    // Set the base address of the client
                    client.BaseAddress = new Uri(url);

                    try
                    {
                        // Make the request and get the response
                         response = await client.GetAsync(client.BaseAddress);

                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                return response.RequestMessage.RequestUri;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private async Task<byte[]> GetUrlToByte(string url)
        {
            byte[] res = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    var imageByteArr = await client.GetByteArrayAsync(url);

                    var imageSrc = Convert.ToBase64String(imageByteArr);

                    res = imageByteArr;


                }
                return res;
            }
            catch (Exception)
            {
                return null;
            }

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
