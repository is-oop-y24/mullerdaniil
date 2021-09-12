namespace Isu.Models
{
    public class Student
    {
        public Student(int id, string name, Group.Group group)
        {
            Id = id;
            Name = name;
            Group = group;
        }

        public int Id { get; }
        public string Name { get; }
        public Group.Group Group { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return Id == ((Student)obj).Id;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}