using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blazor.FormBuilderComponent
{
    public class Person
    {
        [Required]
        [ReadOnly(true)]
        [DefaultValue(typeof(int), "1")]
        public int ID { get; set; }

        [EnumDataType(typeof(Greeting))]
        public Greeting Greeting { get; set; } // Lord, Lady, Mr., Doctor

        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid characters")]
        [MaxLength(12)]
        public string FullName { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Age { get; set; }

        [Description("A detailed description about you")]
        [DataType(DataType.MultilineText)] // .net only
        public string Description { get; set; }

        [Url]
        [DefaultValue("http://instagram.com")]
        public string SocialLink { get; set; }

        [Phone] 
        public string Phone { get; set; }

        [Category("System")]
        [Display(GroupName = "System")]
        [ReadOnly(true)]
        public DateTime CreatedDate { get; set; }

        [Browsable(false)]
        public string PrivateInfo { get; set; }

        [PasswordPropertyText]
        public string Passport { get; set; }

        //[DefaultValue(typeof(CultureInfo), "en-CA")]
        //public CultureInfo PreferedCulture { get; set; }

    }
}
