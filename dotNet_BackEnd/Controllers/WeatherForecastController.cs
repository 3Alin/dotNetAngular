using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace dotNet_BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Route("table")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        [Route("postRequest")]
        public void PostUser([FromBody] string person)
        {
            string path = "D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Users";
            Directory.CreateDirectory(path);

            using (StreamWriter sw = System.IO.File.AppendText(path + "\\users.txt"))
            {
                sw.WriteLine(person);
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadRequest")]
        public IActionResult UploadFile()
        {
            try
            {
                Directory.CreateDirectory("D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Files");
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    bool compile = fileName.Split(".")[1] == "cpp";
                    if (!compile)
                        fileName = "template.txt";

                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    if (compile)
                        new FileCompiler(fileName);

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpGet(Name = "GetOutput")]
        [Route("output")]
        public outputInfo GetOutput()
        {
            Directory.CreateDirectory("D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Files");
            string path = "D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Files";
            using (StreamWriter temp = System.IO.File.AppendText(path + "\\output.txt"));   // Creaza output.txt daca nu exista
            using (StreamWriter temp = System.IO.File.AppendText(path + "\\template.txt")); // Creaza template.txt daca nu exista

            outputInfo info = new outputInfo();
            info.file1 = System.IO.File.ReadAllText(path + "\\output.txt");
            info.file2 = System.IO.File.ReadAllText(path + "\\template.txt");
            info.result = FileCompiler.compareOutput("output.txt", "template.txt").ToString();

            return info;
        }
    }
}