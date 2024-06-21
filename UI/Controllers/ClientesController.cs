using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using UI.Models;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using NLog;

public class ClientesController : Controller
{
    private readonly HttpClient _httpClient;

    public ClientesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        var clientesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Clientes"); // URL para API.Lecturas
        clientesResponse.EnsureSuccessStatusCode();
        var clientes = await clientesResponse.Content.ReadFromJsonAsync<List<Cliente>>();

        var ciudadesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Ciudades"); // URL para API.Lecturas
        ciudadesResponse.EnsureSuccessStatusCode();
        var ciudades = await ciudadesResponse.Content.ReadFromJsonAsync<List<Ciudad>>();

        var clientesConCiudadNombre = clientes.Select(c => new ClienteConCiudad
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Email = c.Email,
            CiudadNombre = ciudades.FirstOrDefault(ciudad => ciudad.Id == c.IdCiudad)?.Nombre
        }).ToList();

        return View(clientesConCiudadNombre);
    }

    public async Task<IActionResult> Create()
    {
        var response = await _httpClient.GetAsync("https://localhost:44342/api/Ciudades"); // URL para API.Lecturas
        response.EnsureSuccessStatusCode();

        var ciudades = await response.Content.ReadFromJsonAsync<List<Ciudad>>();

        ViewData["Ciudades"] = new SelectList(ciudades, "Id", "Nombre");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:44343/api/Clientes", cliente); // URL para API.Escrituras
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }

    // GET: Clientes/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        var clienteResponse = await _httpClient.GetAsync($"https://localhost:44342/api/Clientes/{id}");
        if (!clienteResponse.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var cliente = await clienteResponse.Content.ReadFromJsonAsync<Cliente>();

        var ciudadesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Ciudades");
        if (!ciudadesResponse.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var ciudades = await ciudadesResponse.Content.ReadFromJsonAsync<List<Ciudad>>();

        ViewBag.Ciudades = new SelectList(ciudades, "Id", "Nombre", cliente.IdCiudad);
        return View(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return BadRequest();
        }

        var response = await _httpClient.PutAsJsonAsync($"https://localhost:44343/api/Clientes/{id}", cliente);
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }

    // GET: Clientes/Delete/{id}
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:44342/api/Clientes/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        response.EnsureSuccessStatusCode();

        var cliente = await response.Content.ReadFromJsonAsync<Cliente>();

        return View(cliente);
    }

    // POST: Clientes/Delete/{id}
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:44343/api/Clientes/{id}");
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }
}

