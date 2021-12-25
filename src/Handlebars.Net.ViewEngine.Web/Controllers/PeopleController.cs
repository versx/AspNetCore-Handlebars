namespace HandlebarsDotNet.ViewEngine.Web.Controllers
{
    using System.Collections.Generic;

    using HandlebarsDotNet.ViewEngine.Web.Models;
    using Microsoft.AspNetCore.Mvc;


    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            var people = new List<Person>
            {
                new Person
                {
                    Name = "Jane Doe",
                    Position = "Chief Operating Officer",
                    Office = "London"
                },
                new Person
                {
                    Name = "John Smith",
                    Position = "Systems Architect",
                    Office = "New York"
                },
                new Person
                {
                    Name = "Amit Saluja",
                    Position = "Business Analyst",
                    Office = "Bangalore"
                },
                new Person
                {
                    Name = "Sofia Souza",
                    Position = "Marketing Manager",
                    Office = "São Paulo"
                }
            };
            return View(people);
        }
    }
}