using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Data
{
    public class ClientModel
    {
        public int ClientId { get; set; }

        public Guid LicenceKey { get; set; }


        [Required(ErrorMessage = "Client Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string ClientName { get; set; } = null!;

        [Required(ErrorMessage = "Licence Start Date is required")]
        public DateTime LicenceStartDate { get; set; }

        [Required(ErrorMessage = "Licence End Date is required")]
        public DateTime LicenceEndDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
