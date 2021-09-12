using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Models.Group
{
    public class Group
    {
        private const int MaxStudentPerGroup = 20;
        private List<Student> _students = new List<Student>();
        public Group(GroupName groupName)
        {
            GroupName = groupName;
        }

        public List<Student> Students => _students;
        public GroupName GroupName { get; }

        public void AddStudent(Student student)
        {
            if (_students.Count >= MaxStudentPerGroup)
                throw new IsuException("Student per group exceeded");

            if (_students.Contains(student))
            {
                throw new IsuException("Student is already in the group");
            }
            else
            {
                student.Group = this;
                _students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            bool removeResult = _students.Remove(student);
            if (!removeResult)
            {
                throw new IsuException("No student in the group to remove");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return GroupName.Equals(((Group)obj).GroupName);
            }
        }

        public override int GetHashCode()
        {
            return GroupName.GetHashCode();
        }
    }
}