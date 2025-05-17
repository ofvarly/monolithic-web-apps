namespace basics.Models
{
    public class Repository
    {
        private static readonly List<Course> _courses = new();

        static Repository()
        {
            _courses = new List<Course>
            {
                new Course() {
                    Id = 1,
                    Title = "Asp.Net Core",
                    Description = "Description",
                    Image = "1.jpg",
                    Tags = new string[] {"ASP.NET", "Web"},
                    isActive = true,
                    isHome = true },
                new Course() {
                    Id = 2,
                    Title = "php",
                    Description = "Description",
                    Image = "2.jpg",
                    Tags = new string[] {"PHP", "Web"},
                    isActive = true,
                    isHome = true },
                new Course() {
                    Id = 3,
                    Title = "JS",
                    Description = "Description",
                    Image = "3.jpg",
                    Tags = new string[] {"JavaScript", "Web"},
                    isActive = true,
                    isHome = false },
                new Course() {
                    Id = 4,
                    Title = "TS",
                    Description = "Description",
                    Image = "2.jpg",
                    //Tags = new string[] {"TypeScript", "Web"},
                    isActive = true,
                    isHome = false }
            };
        }
        public static List<Course> Courses
        {
            get
            {
                return _courses;
            }
        }

        public static Course? GetById(int? id)
        {
            return _courses.FirstOrDefault(c => c.Id == id);
        }
    }
}