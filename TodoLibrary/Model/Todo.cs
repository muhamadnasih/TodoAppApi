using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.Model
{
    public class TodoModel
    {
        public int Id { get; set; }
        public string? Task { get; set; }
        public int AssignedTo { get; set; }
        public int IsComplete { get; set; }

    }
}
