using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Models.Group;
using Isu.Services;
using IsuExtra.Models.Lesson;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class MegaFaculty
    {
        private readonly IIsuService _isuService;
        private readonly List<AdditionalCourse> _additionalCourses;
        private readonly Dictionary<Group, Schedule> _schedules;
        private readonly List<Group> _groups;
        internal MegaFaculty(string name)
        {
            _isuService = new IsuService();
            _additionalCourses = new List<AdditionalCourse>();
            _schedules = new Dictionary<Group, Schedule>();
            _groups = new List<Group>();
            Name = name;
        }

        public string Name { get; }
        public IReadOnlyList<AdditionalCourse> AdditionalCourses => _additionalCourses;

        public Student AddStudent(string name, Group group)
        {
            return _isuService.AddStudent(group, name);
        }

        public Group AddGroup(GroupName groupName, Schedule schedule)
        {
            Group group = _isuService.AddGroup(groupName);
            _schedules[group] = schedule;
            _groups.Add(group);
            return group;
        }

        public IReadOnlyList<Student> FindStudentsByGroupName(GroupName groupName)
        {
            return _isuService.FindStudents(groupName);
        }

        public Schedule FindScheduleByGroup(Group group)
        {
            return _schedules[group];
        }

        public bool HasGroup(Group group)
        {
            return _groups.Contains(group);
        }

        internal AdditionalCourse AddAdditionalCourse(string name)
        {
            if (_additionalCourses.Any(course => course.Name == name))
            {
                throw new IsuExtraException("Course " + name + " already exists.");
            }

            var additionalCourse = new AdditionalCourse(name, this);
            _additionalCourses.Add(additionalCourse);
            return additionalCourse;
        }
    }
}