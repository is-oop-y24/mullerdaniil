using System;

namespace IsuExtra.Models.Lesson
{
    public class Lesson
    {
        public Lesson(DateTime startTime, TimeSpan duration, string teacher, string classroom)
        {
            StartTime = startTime;
            Duration = duration;
            Teacher = teacher;
            Classroom = classroom;
        }

        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
        public string Teacher { get; }
        public string Classroom { get; }

        public bool IntersectsWith(Lesson lesson)
        {
            return Math.Min((StartTime + Duration).Millisecond, (lesson.StartTime + lesson.Duration).Millisecond) -
                Math.Max(StartTime.Millisecond, lesson.StartTime.Millisecond) >= 0;
        }
    }
}