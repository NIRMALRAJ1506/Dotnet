using APIConsume.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiBaseUrl = "http://localhost:5266"; // Your API URL

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{ApiBaseUrl}/api/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();

            // Store the token in a cookie or other secure location
            var authCookie = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) }, CookieAuthenticationDefaults.AuthenticationScheme)), CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authCookie.Principal);

            return RedirectToAction("Index", "Reservations");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
