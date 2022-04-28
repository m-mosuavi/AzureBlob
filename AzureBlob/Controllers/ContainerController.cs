using AzureBlob.Models;
using AzureBlob.Services;
using Microsoft.AspNetCore.Mvc;


namespace AzureBlob.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerServices _containerServices;

        public ContainerController(IContainerServices containerServices)
        {
            _containerServices = containerServices;
        }
        public async Task<IActionResult> Index()
        {
            var allContainer = await _containerServices.GetAllContainers();
            return View(allContainer);
        }
        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerServices.DeleteContainer(containerName);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create()
        {
            return View(new Container());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerServices.CreateContainer(container.Name);
            return RedirectToAction(nameof(Index));
        }
       
    }
}
