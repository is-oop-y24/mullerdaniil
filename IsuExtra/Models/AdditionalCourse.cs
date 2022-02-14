using System.Collections.Generic;
using System.Linq;
using IsuExtra.Models.Lesson;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class AdditionalCourse
    {
        private readonly List<AdditionalCourseGroup> _groups;
        internal AdditionalCourse(string name, MegaFaculty megaFaculty)
        {
            _groups = new List<AdditionalCourseGroup>();
            Name = name;
            MegaFaculty = megaFaculty;
        }

        public string Name { get; }
        public MegaFaculty MegaFaculty { get; }
        public IReadOnlyList<AdditionalCourseGroup> Groups => _groups;

        internal AdditionalCourseGroup AddAdditionalCourseGroup(string name, Schedule schedule)
        {
            if (_groups.Any(group => group.Name == name))
            {
                throw new IsuExtraException("Course group " + name + " already exists.");
            }

            var group = new AdditionalCourseGroup(name, this, schedule);
            _groups.Add(group);
            return group;
        }
    }
}