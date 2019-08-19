using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
       // [Authorize]
        [HttpGet]
        public IEnumerable<Equipment> GetEquipment()
        {
            return _context.Equipment;
        }
       // [Authorize]
        [HttpGet("timestamp/{TimeStamp}")]
        public  IEnumerable<Equipment> GetEquipmentAsync([FromRoute] long timestamp)
        {
        
            var equipments =  _context.Equipment.Where(e=>e.TimeStamp>timestamp);
            
            return equipments;

        }


        [HttpGet("plants/{plantString}")]
        public IEnumerable<Equipment> GetEquipmentAsync([FromRoute] String plantString)
        {
            String[] plants = plantString.Split(',');
            var equipments = _context.Equipment.Where(e => Array.Exists(plants, element => element == e.plant));

            return equipments;

        }

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _context.Equipment.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        // PUT: api/Equipments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment([FromRoute] int id, [FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equipment.id)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Equipments
        [HttpPost]
        public  async Task<IActionResult> PostEquipment([FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return  BadRequest(ModelState);
            }
            if (_context.Equipment.Count() < 0)
            {
                equipment.id = 1;
                _context.Equipment.Add(equipment);
                await _context.SaveChangesAsync();
            }
            else
            {

                Equipment eqp = _context.Equipment.AsNoTracking().FirstOrDefault(s => s.EquipmentNumber == equipment.EquipmentNumber);
                if (eqp != null)
                {
                    equipment.id = eqp.id;
                    _context.Equipment.Update(equipment);
                    // _context.Entry(equipment).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {

                    }

                }
                else
                {
                    equipment.id = _context.Equipment.Last().id + 1;
                    _context.Equipment.Add(equipment);
                    await _context.SaveChangesAsync();
                }
            }
         
            return  CreatedAtAction("GetEquipment", new { id = equipment.id }, equipment);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return Ok(equipment);
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.id == id);
        }
    }
}