using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Northwind2.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailQuery: IRequest<CustomerDetailModel>
    {
        public string Id { get; set; }
    }
}
