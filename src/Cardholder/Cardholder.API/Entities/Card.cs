using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardholder.API.Entities;

public class Card
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CardId { get; set; }
    
    [ForeignKey("CardHolderId")]
    public int CardholderId { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; }
    
    [Required]
    [MaxLength(5)]
    public string ExpiryDate { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Cvv { get; set; }

    public Card(int cardholderId, string cardNumber, string expiryDate, string cvv)
    {
        CardholderId = cardholderId;
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        Cvv = cvv;
    }
}