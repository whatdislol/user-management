namespace UserManagementAPI.Models.DTO
{
    public class SearchOptionsDTO
    {
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? SearchQuery { get; set; }
    }
}
