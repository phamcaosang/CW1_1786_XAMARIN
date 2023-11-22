using SQLite;

namespace app1786
{
    [Table("Hike")]
    public class Hike
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public bool ParkingAvailability { get; set; }
        public string Length { get; set; }
        public string Difficulty { get; set; }
        public string Description { get; set; }

        public Hike() { }

        public Hike(int id, string name, string location, string date, bool parkingAvailability, string length, string difficulty, string description)
        {
            Id = id;
            Name = name;
            Location = location;
            Date = date;
            ParkingAvailability = parkingAvailability;
            Length = length;
            Difficulty = difficulty;
            Description = description;
        }
    }
}
