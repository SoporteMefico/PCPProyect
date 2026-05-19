namespace PCPProyect.ViewModel
{
    public class PagedResultVM<T>
    {
        public int Total { get; set; }

        public List<T> Data { get; set; } = new();
    }
}
