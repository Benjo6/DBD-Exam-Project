using lib.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MedicinesController : ControllerBase
{
    private readonly IMedicineStorage _storage;

    public MedicinesController(IMedicineStorage storage)
    {
        _storage = storage;
    }

    [HttpGet]
    public Task<IEnumerable<string>> GetAll([FromQuery] Page? pageInfo)
        => _storage.GetAll(pageInfo);

    [HttpGet("{name}")]
    public Task<MedicineDto> GetMedicineFromName(string name)
        => _storage.Get(name);
}
