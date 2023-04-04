using Blazor.FormBuilderComponent;

namespace Blazor.FormBuilderComponent.Data
{
    public class PeopleService
    {        
        public IEnumerable<Person> GetPeople()
        {
            return new List<Person>() {
                new Person() {
                    ID = 0,
                    FullName = "John Dingleberry",
                    Age = 32, // invoke default
                    Phone = "(555) 555-5555",
                    CreatedDate = DateTime.Now,
                    PrivateInfo = "Hidden",
                    SocialLink = "https://instagram.com",
                    Passport = "Sensitive",
                },
                new Person() {
                    ID = 0,
                    FullName = "Jason Statham",
                    Age = 18,
                    Phone = "(555) 555-5555",
                    CreatedDate = DateTime.Now,
                    PrivateInfo = "Hidden",
                    SocialLink = "https://instagram.com",
                    Passport = "Sensitive",
                }
            };
        }
    }
}