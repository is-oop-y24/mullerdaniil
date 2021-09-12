using Isu.Tools;

namespace Isu.Models.Group
{
    public class CourseNumber
    {
        public CourseNumber(int number)
        {
            if (number >= 1 && number <= 5)
                Number = number;
            else
                throw new IsuException("Invalid course number (Must between 1 and 5)");
        }

        public int Number { get; }
    }
}