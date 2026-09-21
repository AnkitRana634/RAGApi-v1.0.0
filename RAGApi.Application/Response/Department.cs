using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Application.Response
{
    public  class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }
        public decimal Budget { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
