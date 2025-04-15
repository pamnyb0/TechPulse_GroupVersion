using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Du måste fylla i fältet.")]
        [StringLength(50, ErrorMessage = "Namnet får inte vara längre än 50 tecken.")]
        [DisplayName("Användarnamn")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Du måste fylla i fältet.")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
        [DisplayName("E-post")]
        [StringLength(100, ErrorMessage = "E-postadressen får inte vara längre än 100 tecken.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Du måste fylla i fältet.")]
        [DisplayName("Lösenord")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Lösenordet måste innehålla minst 8 tecken, inklusive stora och små bokstäver, siffror och specialtecken.")]
        public string? Password { get; set; }

        [DisplayName("Profilbild")]
        public string? ProfileImageUrl { get; set; }

        [DisplayName("Telefonnummer")]
        [RegularExpression(@"^\+?[0-9]{1,3}[-. ]?\(?[0-9]{1,4}\)?[-. ]?[0-9]{1,4}[-. ]?[0-9]{1,9}$",
            ErrorMessage = "Ogiltigt telefonnummer, exempel: +46 70 123 54 eller 070-123 45 67.")]
        [StringLength(20, ErrorMessage = "Telefonnumret får inte vara längre än 20 tecken.")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Adress")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Du måste fylla i fältet.")]
        [DisplayName("Postnummer")]
        [RegularExpression(@"^\d{3} \d{2}$", ErrorMessage = "Fel format på postnummer. Ange 3 siffror, ett mellanslag och 2 siffror (xxx xx).")] 
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Du måste fylla i fältet.")]
        [DisplayName("Stad")]
        public string? City { get; set; }

        [Display(Name = "Roll")]
        public string Role { get; set; } = "User";

        // Password reset functionality
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenCreated { get; set; }

        // Navigation properties
        public List<PurchaseHistory>? PurchaseHistories { get; set; }
        public List<Order>? Orders { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<ForumThread>? ForumThreads { get; set; }
        public virtual ICollection<ForumReply>? ForumReplies { get; set; }
        public virtual ICollection<Follow>? Following { get; set; }
        public virtual ICollection<Follow>? Followers { get; set; }
    }
}
