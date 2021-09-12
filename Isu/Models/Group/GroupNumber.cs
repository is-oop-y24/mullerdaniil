using Isu.Tools;

namespace Isu.Models.Group
{
    public class GroupNumber
    {
        public GroupNumber(int number)
        {
            if (number >= 0 && number <= 99)
                Number = number;
            else
                throw new IsuException("Invalid group number (Must be between 0 and 99)");
        }

        public int Number { get; }
    }
}