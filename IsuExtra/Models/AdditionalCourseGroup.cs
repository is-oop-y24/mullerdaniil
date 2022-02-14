using System.Collections.Generic;
using Isu.Models;
using IsuExtra.Models.Lesson;

namespace IsuExtra.Models
{
    public class AdditionalCourseGroup
    {
        private readonly List<Student> _registeredStudents;
        internal AdditionalCourseGroup(string name, AdditionalCourse additionalCourse, Schedule schedule)
        {
            _registeredStudents = new List<Student>();
            Name = name;
            AdditionalCourse = additionalCourse;
            Schedule = schedule;
        }

        public string Name { get; }
        public AdditionalCourse AdditionalCourse { get; }
        public Schedule Schedule { get; }
        public IReadOnlyList<Student> RegisteredStudents => _registeredStudents;

        public void Register(Student student)
        {
            _registeredStudents.Add(student);
        }

        public void Unregister(Student student)
        {
            _registeredStudents.Remove(student);
        }
    }
}