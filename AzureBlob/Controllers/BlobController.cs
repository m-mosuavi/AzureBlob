using AzureBlob.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlob.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobServices _blobServices;

        public BlobController(IBlobServices blobServices)
        {
            _blobServices = blobServices;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string containername)
        {
            var blobObj = await _blobServices.GetAllBlobs(containername);
            return View(blobObj);
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFile(string containerName, IFormFile file)
        {
            if (file == null) return View();
            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            var result = _blobServices.UploadBlob(fileName, file, containerName);
            if (result !=null)
            {
                return RedirectToAction("Index", "Container");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string name,string containerName)
        {
            return Redirect(await _blobServices.GetBlob(name, containerName));
        }

        public IActionResult DeleteFile(string name, string containerName)
        {
            _blobServices.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "home");
        }
    }
}
