namespace DotNetCoreDocs.Configuration
{
    public class DocsConfiguration
    {
        public string BaseAddress { get; set; }
        public string RequestsDirectory { get; set; }
        public string DocumentationRoute { get; set; }
        public string ReadmePath { get; set; }
        public string RootTemplatePath { get; set; }
        public string HomeTemplatePath { get; set; }
        public string ModelTemplatePath { get; set; }        

        public string GetRequestsFileName(string modelName)
        {
            return $"{RequestsDirectory}/{modelName}.json";
        }
    }
}
