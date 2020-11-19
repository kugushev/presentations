namespace SearchApp.Domain
{
    public class TermQueryEntity
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public double? Boost { get; set; }
        public bool IsStrict { get; set; }
        public bool IsVerbatim { get; set; }
        public bool IsWritable { get; set; }
        public string Name { get; set; }
    }
}