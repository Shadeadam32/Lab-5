using Lab5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class Author : IClassModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }

        public Author()
        {

        }

        public Author(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            SurName = lastName;
        }
    }
}
