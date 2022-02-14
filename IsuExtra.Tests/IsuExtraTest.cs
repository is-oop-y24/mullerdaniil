using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Models.Group;
using IsuExtra.Models;
using IsuExtra.Models.Lesson;
using IsuExtra.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraTest
    {
        private IIsuExtraService _isuExtraService;
        private MegaFaculty _megaFaculty;
        private AdditionalCourse _course;
        private Group _group;
        private Student _student;

        [SetUp]
        public void Setup()
        {
            _isuExtraService = new IsuExtraService(2);
            _megaFaculty = _isuExtraService.AddMegaFaculty("Psychologies");
            _course = _isuExtraService.AddAdditionalCourse("French", _megaFaculty);
            _group = _megaFaculty.AddGroup(new GroupName(CourseNumber.Course4, 8), new Schedule());
            _student = _megaFaculty.AddStudent("Jack Jackson", _group);
        }

        [Test]
        public void AddAdditionalCourse_MegaFacultyHasCourse()
        {
            AdditionalCourse course = _isuExtraService.AddAdditionalCourse("English", _megaFaculty);
            Assert.Contains(course, _megaFaculty.AdditionalCourses.ToList());
        }

        [Test]
        public void RegisterStudentToSameFacultyGroup_ThrowException()
        {
            Group mainGroup = _megaFaculty.AddGroup(new GroupName(CourseNumber.Course3, 14), new Schedule());
            Student student = _megaFaculty.AddStudent("Steve Stevenson", mainGroup);
            AdditionalCourseGroup group = _isuExtraService.AddAdditionalCourseGroup("Group 2", _course, new Schedule());

            Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.RegisterStudent(student, group);
                }
            );
        }

        [Test]
        public void RegisterStudentToGroup_GroupHasStudent()
        {
            MegaFaculty courseMegaFaculty = _isuExtraService.AddMegaFaculty("Informatics");
            AdditionalCourse course = _isuExtraService.AddAdditionalCourse("CyberSecurity", courseMegaFaculty);
            AdditionalCourseGroup courseGroup =
                _isuExtraService.AddAdditionalCourseGroup("Main group", course, new Schedule());
            _isuExtraService.RegisterStudent(_student, courseGroup);
            Assert.Contains(_student, _isuExtraService.FindStudentsByAdditionalCourseGroup(courseGroup).ToList());
        }

        [Test]
        public void ReachCoursesPerStudentLimit_ThrowException()
        {
            MegaFaculty courseMegaFaculty = _isuExtraService.AddMegaFaculty("Informatics");
            AdditionalCourse course = _isuExtraService.AddAdditionalCourse("CyberSecurity", courseMegaFaculty);
            AdditionalCourseGroup courseGroup =
                _isuExtraService.AddAdditionalCourseGroup("Main group", course, new Schedule());
            _isuExtraService.RegisterStudent(_student, courseGroup);
            
            MegaFaculty courseMegaFaculty2 = _isuExtraService.AddMegaFaculty("Physics");
            AdditionalCourse course2 = _isuExtraService.AddAdditionalCourse("Photonics", courseMegaFaculty2);
            AdditionalCourseGroup courseGroup2 =
                _isuExtraService.AddAdditionalCourseGroup("Main group", course2, new Schedule());
            _isuExtraService.RegisterStudent(_student, courseGroup2);
            
            MegaFaculty courseMegaFaculty3 = _isuExtraService.AddMegaFaculty("History");
            AdditionalCourse course3 = _isuExtraService.AddAdditionalCourse("European History", courseMegaFaculty3);
            AdditionalCourseGroup courseGroup3 =
                _isuExtraService.AddAdditionalCourseGroup("2nd group", course3, new Schedule());
            
            Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.RegisterStudent(_student, courseGroup3);
                }
            );

        }

        [Test]
        public void AddUnregisteredStudent_StudentIsInUnregisteredListOfGroup()
        {
            Student student1 = _megaFaculty.AddStudent("Aaron Jameson", _group);
            Student student2 = _megaFaculty.AddStudent("Ben Benson", _group);
            Student student3 = _megaFaculty.AddStudent("Charles Monroe", _group);
            Student student4 = _megaFaculty.AddStudent("Daryl Madison", _group);

            MegaFaculty courseMegaFaculty = _isuExtraService.AddMegaFaculty("Sport");
            AdditionalCourse course =
                _isuExtraService.AddAdditionalCourse("Sport and physic culture", courseMegaFaculty);
            AdditionalCourseGroup courseGroup =
                _isuExtraService.AddAdditionalCourseGroup("Main group", course, new Schedule());

            _isuExtraService.RegisterStudent(student2, courseGroup);
            _isuExtraService.RegisterStudent(student4, courseGroup);

            IReadOnlyList<Student> unregisteredStudents =
                _isuExtraService.FindUnregisteredStudentByGroup(_group, _megaFaculty);
            
            Assert.Contains(student1, unregisteredStudents.ToList());
            Assert.Contains(student3, unregisteredStudents.ToList());
        }

        [Test]
        public void GroupSchedulesIntersect_ThrowException()
        {
            var lesson1 = new Lesson(
                new DateTime(2005, 10, 12, 13, 0, 0),
                new TimeSpan(1, 30, 0),
                "Tyler Taylor",
                "3214b"
            );
            
            var lesson2 = new Lesson(
                new DateTime(2005, 10, 12, 14, 0, 0),
                new TimeSpan(1, 30, 0),
                "Stan Broderick",
                "4100a"
            );
            
            var mainGroupSchedule = new Schedule();
            mainGroupSchedule.AddLesson(lesson1);
            var courseGroupSchedule = new Schedule();
            courseGroupSchedule.AddLesson(lesson2);

            Group mainGroup = _megaFaculty.AddGroup(new GroupName(CourseNumber.Course1, 2), mainGroupSchedule);
            Student student = _megaFaculty.AddStudent("John Johnson", mainGroup);
            
            MegaFaculty courseMegaFaculty = _isuExtraService.AddMegaFaculty("Sport");
            AdditionalCourse course =
                _isuExtraService.AddAdditionalCourse("Sport and physic culture", courseMegaFaculty);
            AdditionalCourseGroup courseGroup =
                _isuExtraService.AddAdditionalCourseGroup("Main group", course, courseGroupSchedule);
            
            Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.RegisterStudent(student, courseGroup);
                }
            );
        }

        [Test]
        public void SchedulesHaveIntersectingLessons_SchedulesIntersect()
        {
            var lesson1 = new Lesson(
                new DateTime(2005, 10, 12, 13, 0, 0),
                new TimeSpan(1, 30, 0),
                "Tyler Taylor",
                "3214b"
            );
            
            var lesson2 = new Lesson(
                new DateTime(2005, 10, 12, 14, 0, 0),
                new TimeSpan(1, 30, 0),
                "Stan Broderick",
                "4100a"
            );

            var schedule1 = new Schedule();
            schedule1.AddLesson(lesson1);
            var schedule2 = new Schedule();
            schedule2.AddLesson(lesson2);
            
            Assert.IsTrue(schedule1.IntersectsWith(schedule2));
        }
    }
}