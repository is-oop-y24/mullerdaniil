using System;

namespace Isu.Models.Group
{
    public class GroupName
    {
        public GroupName(CourseNumber courseNumber, GroupNumber groupNumber)
        {
            CourseNumber = courseNumber;
            GroupNumber = groupNumber;
        }

        public CourseNumber CourseNumber { get; }
        public GroupNumber GroupNumber { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return CourseNumber.Equals(((GroupName)obj).CourseNumber)
                       && GroupNumber.Equals(((GroupName)obj).GroupNumber);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CourseNumber.Number, GroupNumber.Number);
        }
    }
}