namespace UserManagementAPI.Models.DTO
{
    public class AllUsersResponseDTO
    {
        public required IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; } 
        public required int TotalCount { get; set; }
    }
}
