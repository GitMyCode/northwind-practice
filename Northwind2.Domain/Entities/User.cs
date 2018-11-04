using System;
using System.Collections.Generic;
using System.Text;
using Northwind2.Domain.ValueObjects;

namespace Northwind2.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public AdAccount AdAccount { get; set; }
    }
}
