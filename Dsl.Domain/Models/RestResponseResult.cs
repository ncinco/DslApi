namespace Dsl.Domain.Models
{
    public class RestResponseResult<T>
    {
        public int Count { get; set; }
        public string PageState { get; set; }
        public List<T> Data { get; set; }
    }
}