using APIConsume.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

[Authorize]
public class ReservationsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiBaseUrl = "http://localhost:5266"; // Updated base URL without trailing slash

    public ReservationsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await client.GetAsync($"{ApiBaseUrl}/api/reservations");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(content);
            return View(reservations);
        }

        // Log error or handle failure
        return View(new List<ReservationViewModel>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ApiBaseUrl}/api/reservations", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Log error or handle failure
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiBaseUrl}/api/reservations/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<ReservationViewModel>(content);
            return View(reservation);
        }

        return NotFound(); // Handle not found case
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return the view with validation errors
        }

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{ApiBaseUrl}/api/reservations/{model.Id}", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Error updating reservation");
        return View(model); // Return the view with the model
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync($"{ApiBaseUrl}/api/reservations/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        else
        {
            // Log error or handle failure
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await client.GetAsync($"{ApiBaseUrl}/api/reservations/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<ReservationViewModel>(content);
            return View(reservation);
        }

        // Log error or handle failure
        return RedirectToAction("Index");
    }
}
