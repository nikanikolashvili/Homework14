using FluentValidation;
using HomeWorkforWeek14.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace HomeWorkforWeek14.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private const string FilePath = @"C:\Users\nika\Desktop\HomeWorkforWeek14\HomeWorkforWeek14\ResInfo\info.json";

        private List<Person> LoadFromFile()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return new List<Person>();
            }

            var json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
        }

        private void SaveToFile(List<Person> persons)
        {
            var json = JsonSerializer.Serialize(persons);
            System.IO.File.WriteAllText(FilePath, json);
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest("Person cannot be null.");
            }

            var validator = new Perosnvalidator();
            var validationResult = validator.Validate(person);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var persons = LoadFromFile();
            persons.Add(person);
            SaveToFile(persons);

            return Ok(persons);
        }

        [HttpGet]
        public IActionResult GetAllPersons()
        {
            var persons = LoadFromFile();
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var persons = LoadFromFile();

            if (id < 0 || id >= persons.Count)
            {
                return NotFound($"Person with index {id} not found.");
            }

            return Ok(persons[id]);
        }

        [HttpGet("filter")]
        public IActionResult GetFilteredPersons([FromQuery] double? minSalary = null, [FromQuery] string city = null)
        {
            var persons = LoadFromFile();

            if (minSalary.HasValue)
            {
                persons = persons.Where(p => p.Salary > minSalary.Value).ToList();
            }

            if (!string.IsNullOrEmpty(city))
            {
                persons = persons.Where(p => p.Address.City == city).ToList();
            }

            return Ok(persons);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            var persons = LoadFromFile();

            if (id < 0 || id >= persons.Count)
            {
                return NotFound($"Person with index {id} not found.");
            }

            persons.RemoveAt(id);
            SaveToFile(persons);

            return Ok(persons);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, [FromBody] Person person)
        {
            var persons = LoadFromFile();

            if (id < 0 || id >= persons.Count)
            {
                return NotFound($"Person with index {id} not found.");
            }

            var validator = new Perosnvalidator();
            var validationResult = validator.Validate(person);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            persons[id] = person;
            SaveToFile(persons);

            return Ok(persons);
        }
    }

}
