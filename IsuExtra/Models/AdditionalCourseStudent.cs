using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    internal class AdditionalCourseStudent
    {
        private readonly List<AdditionalCourseGroup> _courseGroups;
        private readonly int _additionalCoursesLimit;
        public AdditionalCourseStudent(Student student, int additionalCoursesLimit)
        {
            _courseGroups = new List<AdditionalCourseGroup>();
            _additionalCoursesLimit = additionalCoursesLimit;
            Student = student;
        }

        public Student Student { get; }
        public IReadOnlyList<AdditionalCourseGroup> CourseGroups => _courseGroups;

        public void Register(AdditionalCourseGroup group)
        {
            if (_courseGroups.Count == _additionalCoursesLimit)
            {
                throw new IsuExtraException("Unable to register student " + Student.Name +
                                            ". Limit of courses exceeded.");
            }

            if (_courseGroups.Any(additionalGroup => additionalGroup.AdditionalCourse.Equals(group.AdditionalCourse)))
            {
                throw new IsuExtraException("Unable to register student " + Student.Name + ". Already registered.");
            }

            _courseGroups.Add(group);
        }

        public void Unregister(AdditionalCourseGroup group)
        {
            if (!_courseGroups.Contains(group))
            {
                throw new IsuExtraException("Unable to remove. Student is not registered.");
            }

            _courseGroups.Remove(group);
        }
    }
}