using KalahariCollarV17.Areas.Identity.Data;

namespace KalahariCollarV17.Models
{
    public enum Type
    {
        Dog, Cat
    }
    public class Pet
    {
        public int Id { get; set; }

        public int TrackerID {  get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Type? Type { get; set; } // Add the type property (e.g., "cat" or "dog")
                                        // Add other pet properties
        public string Breed { get; set; }
        public string OwnerId { get; set; }
        public string Location { get; set; } // Add the location property
                                             // Add other pet properties

        public ApplicationUser Owner { get; set; }
    }
}
