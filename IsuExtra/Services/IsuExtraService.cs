using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Models.Group;
using IsuExtra.Models;
using IsuExtra.Models.Lesson;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService : IIsuExtraService
    {
        private readonly List<MegaFaculty> _megaFaculties;
        private readonly List<AdditionalCourseStudent> _additionalCourseStudents;
        private readonly int _numberOfCoursesPerStudentLimit;

        public IsuExtraService(int numberOfCoursesPerStudentLimit)
        {
            _megaFaculties = new List<MegaFaculty>();
            _additionalCourseStudents = new List<AdditionalCourseStudent>();
            _numberOfCoursesPerStudentLimit = numberOfCoursesPerStudentLimit;
        }

        public MegaFaculty AddMegaFaculty(string name)
        {
            if (_megaFaculties.Any(megaFaculty => megaFaculty.Name == name))
            {
                throw new IsuExtraException("megaFaculty " + name + " already exists.");
            }

            var megaFaculty = new MegaFaculty(name);
            _megaFaculties.Add(megaFaculty);
            return megaFaculty;
        }

        public AdditionalCourse AddAdditionalCourse(string name, MegaFaculty megaFaculty)
        {
            return megaFaculty.AddAdditionalCourse(name);
        }

        public AdditionalCourseGroup AddAdditionalCourseGroup(string name, AdditionalCourse course, Schedule schedule)
        {
            return course.AddAdditionalCourseGroup(name, schedule);
        }

        public void RegisterStudent(Student student, AdditionalCourseGroup group)
        {
            MegaFaculty megaFaculty = _megaFaculties.Find(megaFaculty => megaFaculty.HasGroup(student.Group));
            if (megaFaculty.Equals(group.AdditionalCourse.MegaFaculty))
            {
                throw new IsuExtraException("Unable to register student " + student.Name + ". Megafaculties intersect.");
            }

            if (megaFaculty.FindScheduleByGroup(student.Group).IntersectsWith(group.Schedule))
            {
                throw new IsuExtraException("Unable to register student " + student.Name + ". Schedules intersect.");
            }

            AdditionalCourseStudent courseStudent;
            if (_additionalCourseStudents.Any(additionalCourseStudent => additionalCourseStudent.Student.Equals(student)))
            {
                courseStudent = _additionalCourseStudents.Find(additionalCourseStudent => additionalCourseStudent.Student.Equals(student));
            }
            else
            {
                courseStudent = new AdditionalCourseStudent(student, _numberOfCoursesPerStudentLimit);
            }

            courseStudent.Register(group);
            if (!_additionalCourseStudents.Any(additionalCourseStudent => additionalCourseStudent.Student.Equals(student)))
            {
                _additionalCourseStudents.Add(courseStudent);
            }

            group.Register(student);
        }

        public void UnregisterStudent(Student student, AdditionalCourseGroup group)
        {
            if (_additionalCourseStudents.All(registeredStudent => registeredStudent.Student.Equals(student)))
            {
                throw new IsuExtraException("Student " + student.Name + " not found.");
            }

            AdditionalCourseStudent registeredStudent =
                _additionalCourseStudents.Find(registeredStudent => registeredStudent.Student.Equals(student));
            registeredStudent.Unregister(group);
            _additionalCourseStudents.Remove(registeredStudent);
            group.Unregister(student);
        }

        public IReadOnlyList<AdditionalCourseGroup> FindGroupsByAdditionalCourse(AdditionalCourse course)
        {
            return course.Groups;
        }

        public IReadOnlyList<Student> FindStudentsByAdditionalCourseGroup(AdditionalCourseGroup group)
        {
            return group.RegisteredStudents;
        }

        public IReadOnlyList<Student> FindUnregisteredStudentByGroup(Group group, MegaFaculty megaFaculty)
        {
            IReadOnlyList<Student> allStudentsInGroup = megaFaculty.FindStudentsByGroupName(group.GroupName);
            return allStudentsInGroup.Where(student =>
                !_additionalCourseStudents.Any(additionalCourseStudent =>
                    additionalCourseStudent.Student.Equals(student))).ToList();
        }
    }
}