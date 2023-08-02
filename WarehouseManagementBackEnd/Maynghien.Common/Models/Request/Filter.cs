namespace MayNghien.Models.Request.Base
{
    public class Filter
    {
        public string FieldName { get; set; }

        public string Value { get; set; }

        public string? Operation { get; set; }
    }
}
