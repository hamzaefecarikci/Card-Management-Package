using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardholder.API.Entities;

public class CardHolder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CardholderId { get; set; }
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    [Required]
    [MaxLength(Int32.MaxValue)]
    public string PaswordHash { get; set; }
    public DateTime CreatedAt { get; set; }

    public CardHolder(string fullName, string email, string paswordHash)
    {
        FullName = fullName;
        Email = email;
        PaswordHash = paswordHash;
        CreatedAt = DateTime.UtcNow;
    }
}