using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Models.Group
{
    public class Group
    {
        public Group(GroupName groupName)
        {
            GroupName = groupName;
        }

        public GroupName GroupName { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return GroupName.Equals(((Group)obj).GroupName);
            }
        }

        public override int GetHashCode()
        {
            return GroupName.GetHashCode();
        }
    }
}