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
        private static List<Dog> _dogs = new List<Dog>
        {
            new Dog { Id = 1, Name = "Rex", Breed = "Labrador", Age = 5, BirthDate = DateTime.Now.AddYears(-5), HasOwner = true },
            new Dog { Id = 2, Name = "Max", Breed = "Poodle", Age = 3, BirthDate = DateTime.Now.AddYears(-3), HasOwner = false },
            new Dog { Id = 3, Name = "Bella", Breed = "Bulldog", Age = 2, BirthDate = DateTime.Now.AddYears(-2), HasOwner = true },
            new Dog { Id = 4, Name = "Thor", Breed = "Pastor Alemão", Age = 4, BirthDate = DateTime.Now.AddYears(-4), HasOwner = true },
            new Dog { Id = 5, Name = "Luna", Breed = "Golden Retriever", Age = 1, BirthDate = DateTime.Now.AddYears(-1), HasOwner = false },
            new Dog { Id = 6, Name = "Simba", Breed = "Boxer", Age = 6, BirthDate = DateTime.Now.AddYears(-6), HasOwner = true },
            new Dog { Id = 7, Name = "Milo", Breed = "Beagle", Age = 2, BirthDate = DateTime.Now.AddYears(-2), HasOwner = false },
            new Dog { Id = 8, Name = "Nina", Breed = "Shih Tzu", Age = 3, BirthDate = DateTime.Now.AddYears(-3), HasOwner = true },
            new Dog { Id = 9, Name = "Bob", Breed = "Dachshund", Age = 7, BirthDate = DateTime.Now.AddYears(-7), HasOwner = true },
            new Dog { Id = 10, Name = "Toby", Breed = "Rottweiler", Age = 5, BirthDate = DateTime.Now.AddYears(-5), HasOwner = false },
            new Dog { Id = 11, Name = "Mel", Breed = "Yorkshire", Age = 4, BirthDate = DateTime.Now.AddYears(-4), HasOwner = true },
            new Dog { Id = 12, Name = "Fred", Breed = "Schnauzer", Age = 3, BirthDate = DateTime.Now.AddYears(-3), HasOwner = false },
            new Dog { Id = 13, Name = "Duke", Breed = "Husky Siberiano", Age = 2, BirthDate = DateTime.Now.AddYears(-2), HasOwner = true },
            new Dog { Id = 14, Name = "Lola", Breed = "Pug", Age = 1, BirthDate = DateTime.Now.AddYears(-1), HasOwner = true },
            new Dog { Id = 15, Name = "Spike", Breed = "Bulldog Francês", Age = 6, BirthDate = DateTime.Now.AddYears(-6), HasOwner = false },
            new Dog { Id = 16, Name = "Maggie", Breed = "Cocker Spaniel", Age = 5, BirthDate = DateTime.Now.AddYears(-5), HasOwner = true },
            new Dog { Id = 17, Name = "Bruce", Breed = "Doberman", Age = 4, BirthDate = DateTime.Now.AddYears(-4), HasOwner = true },
            new Dog { Id = 18, Name = "Julie", Breed = "Border Collie", Age = 3, BirthDate = DateTime.Now.AddYears(-3), HasOwner = false },
            new Dog { Id = 19, Name = "Pipoca", Breed = "Lhasa Apso", Age = 2, BirthDate = DateTime.Now.AddYears(-2), HasOwner = true },
            new Dog { Id = 20, Name = "Thor", Breed = "Akita", Age = 1, BirthDate = DateTime.Now.AddYears(-1), HasOwner = false }
        };

        [HttpGet("connectionstring")]
        public ActionResult<string> GetConnectionString()
        {
            return "Mocked connection string";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDog()
        {
            return await Task.FromResult(_dogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dog>> GetDog(int id)
        {
            var dog = _dogs.FirstOrDefault(d => d.Id == id);

            try
            {
                DatadogLogger.AddTag("username", "eminsalim");
                Guid identifier = Guid.NewGuid();
                DatadogLogger.AddTag("dog_request_id", identifier.ToString());
            }
            catch (NullReferenceException)
            {
            }

            if (dog == null)
            {
                return NotFound();
            }

            return await Task.FromResult(dog);
        }

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

        [HttpPost]
        public async Task<ActionResult<Dog>> PostDog(Dog dog)
        {
            dog.Id = _dogs.Any() ? _dogs.Max(d => d.Id) + 1 : 1;
            _dogs.Add(dog);

            return await Task.FromResult(CreatedAtAction("GetDog", new { id = dog.Id }, dog));
        }

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