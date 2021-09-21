using System;
using Isu.Models;
using Isu.Models.Group;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var groupName = new GroupName(CourseNumber.Course3, 6);
            Group group = _isuService.AddGroup(groupName);
            
            const string studentName = "Jack Jackson";
            Student addedStudent = _isuService.AddStudent(group, studentName);
            

                Assert.NotNull(_isuService.FindGroup(groupName));
            Assert.NotNull(_isuService.FindStudent(studentName));
            
            Group extractedGroup = _isuService.FindGroup(groupName);
            Student extractedStudent = _isuService.FindStudent(studentName);
            
            Assert.AreEqual(extractedGroup, extractedStudent.Group);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                const string studentName = "John Johnson";
                Group group = _isuService.AddGroup(new GroupName(CourseNumber.Course2, 8));
                

                for (int i = 0; i < 30; i++)
                {
                    _isuService.AddStudent(group, studentName);
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new GroupName(CourseNumber.Course4, 120));
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            const string studentName = "Steven Stevenson";
            var oldGroupName = new GroupName(CourseNumber.Course4, 83);
            var newGroupName = new GroupName(CourseNumber.Course4, 25);

            Group oldGroup = _isuService.AddGroup(oldGroupName);
            Group newGroup = _isuService.AddGroup(newGroupName);
            
            Student student = _isuService.AddStudent(oldGroup, studentName);
            _isuService.ChangeStudentGroup(student, newGroup);
            student = _isuService.GetStudent(student.Id);
            
            Assert.AreEqual(newGroup, student.Group);
        }
    }
}