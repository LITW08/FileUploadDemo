using FileUploadDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileUploadDemo.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=ImageUploadDemo;Integrated Security=true;";

        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewImages()
        {
            var db = new ImageDb(_connectionString);
            var vm = new ViewImagesViewModel
            {
                Images = db.GetImages()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile myfile)
        {
            Guid guid = Guid.NewGuid();
            string actualFileName = $"{guid}-{myfile.FileName}";
            string finalFileName = Path.Combine(_environment.WebRootPath, "uploads", actualFileName);
            using var fs = new FileStream(finalFileName, FileMode.CreateNew);
            myfile.CopyTo(fs);

            var db = new ImageDb(_connectionString);
            db.AddImage(actualFileName);
            
            return Redirect("/");

        }
    }
}

/*Create an application where users can upload images and share
it with their friends. However, when an image is uploaded,
the user will be prompted to create a "password" which will
protect the image from being seen by anyone that doesn't have 
the password.

Here's the flow of the application: 

On the home page, there should be a textbox and a file upload
input. The user will then put in a "password" into the textbox
and choose an image to upload. When they hit submit, they should
get taken to a page that says:

"Thank you for uploading your image, here's the link to share with your friends:
http://localhost:123/images/view?id=14

Make sure to give them the password of 'foobar'"

When a user tries to visit an images page, they should first be presented with a 
textbox where they need to put the password saved by the image uploader. If they
enter it correctly, the page should refresh (same url) and they should see the image.
Underneath the image, they should also see a little number that displays how many
times this image has already been viewed (just store this number in the database 
and keep updating it every time it's viewed).  If they put the password in incorrectly,
the page should refresh with an error message saying "please try again".

Once they've put in the password, they should never have to put in the password again
for that image.

Good luck*/
