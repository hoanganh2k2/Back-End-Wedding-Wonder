using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PexelsDotNetSDK.Api;

namespace WeddingWonderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private const string UnsplashApiKey = "9UOxVh0nHk6VQlZ5YXI_hKjGNrfC9hW-bxcwxadGrik";

        [HttpGet("GenerateUnsplashImage")]
        public async Task<IActionResult> GenerateUnsplashImage(string query)
        {
            List<string> lstImg = new List<string>();
            string url = $"https://api.unsplash.com/photos/random?query={query}&client_id={UnsplashApiKey}&count=5";

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var images = JsonConvert.DeserializeObject<List<UnsplashImage>>(response);

                    foreach (var image in images)
                    {
                        lstImg.Add(image.Urls.Regular);
                    }

                    return Ok(lstImg);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }

        [HttpGet("GenerateVideos")]
        public async Task<IActionResult> GenerateVideos(string prompt)
        {
            List<string> LstVideos = new List<string>();
            var pexelsClient = new PexelsClient("sPb71yRgnqe09JoxwuuofE0wtNkN6LvKqr1J0QM7pdCmt8HgwVfavykt");

            try
            {
                var result = await pexelsClient.SearchVideosAsync(prompt);
                var lstvds = result.videos.ToList();

                foreach (var video in lstvds)
                {
                    var videoLink = video.videoFiles.FirstOrDefault()?.link;
                    if (videoLink != null)
                    {
                        LstVideos.Add(videoLink);
                    }
                }

                return Ok(LstVideos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class UnsplashImage
    {
        public UnsplashImageUrls Urls { get; set; }
    }

    public class UnsplashImageUrls
    {
        public string Regular { get; set; }
    }
}
