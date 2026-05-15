namespace PCPProyect.ViewModel
{
    public class DataTableRequestVM
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public SearchVM Search { get; set; }
    }

    public class SearchVM
    {
        public string Value { get; set; }
    }
}
