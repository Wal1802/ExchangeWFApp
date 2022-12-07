
namespace ExchangeApp.Models
{
    public class ResponseModel
    {
        public bool unAuthorizedRequest { get; set; }
        public string error { get; set; }
        public bool success { get; set; }
        public string targetUrl { get; set; }
        public ResultModel result{ get; set; }
    }
}
