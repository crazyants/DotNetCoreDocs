using System.Collections.Generic;

namespace DotNetCoreDocs.Models
{
    public class RequestsDocument
    {
        public string ModelName { get; set; }
        public List<TestRequest> TestRequests { get; set; }
    }
}
