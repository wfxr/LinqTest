
namespace LinqTest
{
    class Student {

        public Student(string name, string id, int score) {
            Name = name;
            Id = id;
            Score = score;
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public int Score { get; set; }
    }
}
