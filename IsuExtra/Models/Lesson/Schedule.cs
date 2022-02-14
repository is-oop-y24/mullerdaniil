using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Models.Lesson
{
    public class Schedule
    {
        private List<Lesson> _lessons;

        public Schedule()
        {
            _lessons = new List<Lesson>();
        }

        public IReadOnlyList<Lesson> Lessons => _lessons;

        public bool IntersectsWith(Schedule schedule)
        {
            return _lessons.Any(lesson => schedule.Lessons.Any(otherScheduleLesson => otherScheduleLesson.IntersectsWith(lesson)));
        }

        public void AddLesson(Lesson lesson)
        {
            if (_lessons.Any(scheduleLesson => scheduleLesson.IntersectsWith(lesson)))
            {
                throw new IsuExtraException("Lesson intersects with other lessons.");
            }

            _lessons.Add(lesson);
        }
    }
}