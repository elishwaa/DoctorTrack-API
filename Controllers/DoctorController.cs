using DoctorTrack.DTOs;
using DoctorTrack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status)
        {
            var result = await _service.GetAllAsync(search, status);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorDTO dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
        int id,
        UpdateDoctorDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return Ok(new { message = "Doctor updated successfully" });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] string status)
        {
            var updated = await _service.UpdateStatusAsync(id, status);

            if (!updated)
                return NotFound();

            return Ok(new { message = "Status updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok(new { message = "Doctor deleted successfully" });
        }
    }
}
