using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using UI.Models;

public class CiudadesController : Controller
{
    private readonly HttpClient _httpClient;

    public CiudadesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("https://localhost:44342/api/Ciudades"); // URL para API.Lecturas
        response.EnsureSuccessStatusCode();

        var ciudades = await response.Content.ReadFromJsonAsync<List<Ciudad>>();

        return View(ciudades);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Ciudad ciudad)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:44343/api/Ciudades", ciudad); // URL para API.Escrituras
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> CiudadesConNumeroClientes()
    {
        var response = await _httpClient.GetAsync("https://localhost:44342/api/Ciudades/ClientesCount");
        response.EnsureSuccessStatusCode();

        var ciudadesConClientes = await response.Content.ReadFromJsonAsync<List<CiudadConClientes>>();

        return View(ciudadesConClientes);
    }

}
