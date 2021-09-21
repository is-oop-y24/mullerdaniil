using System;
using Isu.Tools;

namespace Isu.Models.Group
{
    public class GroupName
    {
        private const int GroupNumberMinValue = 0;
        private const int GroupNumberMaxValue = 99;
        public GroupName(CourseNumber courseNumber, int groupNumber)
        {
            if (!(groupNumber >= GroupNumberMinValue && groupNumber <= GroupNumberMaxValue))
                throw new IsuException("Invalid group number (value must be between " + GroupNumberMinValue + " and " + GroupNumberMaxValue + ")");
            CourseNumber = courseNumber;
            GroupNumber = groupNumber;
        }

        public CourseNumber CourseNumber { get; }
        public int GroupNumber { get; }

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
            return HashCode.Combine(CourseNumber, GroupNumber);
        }
    }
}