using System.Collections.Generic;
using Isu.Models;
using Isu.Models.Group;
using IsuExtra.Models;
using IsuExtra.Models.Lesson;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        MegaFaculty AddMegaFaculty(string name);
        AdditionalCourse AddAdditionalCourse(string name, MegaFaculty megaFaculty);
        AdditionalCourseGroup AddAdditionalCourseGroup(string name, AdditionalCourse course, Schedule schedule);
        void RegisterStudent(Student student, AdditionalCourseGroup group);
        void UnregisterStudent(Student student, AdditionalCourseGroup group);
        IReadOnlyList<AdditionalCourseGroup> FindGroupsByAdditionalCourse(AdditionalCourse course);
        IReadOnlyList<Student> FindStudentsByAdditionalCourseGroup(AdditionalCourseGroup group);
        IReadOnlyList<Student> FindUnregisteredStudentByGroup(Group group, MegaFaculty megaFaculty);
    }
}