using DogsWorld.Logger;
using DogsWorld.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        // Mock data (in-memory list)
        private static List<Dog> _dogs = new List<Dog>
        {
            new Dog { Id = 1, Name = "Rex", Breed = "Labrador", Age = 5, BirthDate = DateTime.Now.AddYears(-5), HasOwner = true },
            new Dog { Id = 2, Name = "Max", Breed = "Poodle", Age = 3, BirthDate = DateTime.Now.AddYears(-3), HasOwner = false },
            new Dog { Id = 3, Name = "Bella", Breed = "Bulldog", Age = 2, BirthDate = DateTime.Now.AddYears(-2), HasOwner = true }
        };

        [HttpGet("connectionstring")]
        public ActionResult<string> GetConnectionString()
        {
            return "Mocked connection string";
        }

        // GET: api/Dogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDog()
        {
            return await Task.FromResult(_dogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dog>> GetDog(int id)
        {
            var dog = _dogs.FirstOrDefault(d => d.Id == id);

            // Só logue se houver escopo ativo do Datadog
            try
            {
                DatadogLogger.AddTag("username", "eminsalim");
                Guid identifier = Guid.NewGuid();
                DatadogLogger.AddTag("dog_request_id", identifier.ToString());
            }
            catch (NullReferenceException)
            {
                // Ignora se não houver escopo ativo
            }

            if (dog == null)
            {
                return NotFound();
            }

            return await Task.FromResult(dog);
        }

        // PUT: api/Dogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDog(int id, Dog dog)
        {
            if (id != dog.Id)
            {
                return BadRequest();
            }

            var existingDog = _dogs.FirstOrDefault(d => d.Id == id);
            if (existingDog == null)
            {
                return NotFound();
            }

            existingDog.Name = dog.Name;
            existingDog.Breed = dog.Breed;
            existingDog.Age = dog.Age;
            existingDog.BirthDate = dog.BirthDate;
            existingDog.HasOwner = dog.HasOwner;

            return await Task.FromResult(NoContent());
        }

        // POST: api/Dogs
        [HttpPost]
        public async Task<ActionResult<Dog>> PostDog(Dog dog)
        {
            dog.Id = _dogs.Any() ? _dogs.Max(d => d.Id) + 1 : 1;
            _dogs.Add(dog);

            return await Task.FromResult(CreatedAtAction("GetDog", new { id = dog.Id }, dog));
        }

        // DELETE: api/Dogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dog>> DeleteDog(int id)
        {
            var dog = _dogs.FirstOrDefault(d => d.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            _dogs.Remove(dog);

            return await Task.FromResult(dog);
        }
    }
}