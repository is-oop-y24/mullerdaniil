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
            var groupName = new GroupName(new CourseNumber(3), new GroupNumber(6));
            Group group = _isuService.AddGroup(groupName);
            
            const string studentName = "Jack Jackson";
            Student addedStudent = _isuService.AddStudent(group, studentName);
            
            try
            {
                _isuService.GetStudent(addedStudent.Id);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
            Assert.NotNull(_isuService.FindGroup(groupName));
            Assert.NotNull(_isuService.FindStudent(studentName));
            
            Group extractedGroup = _isuService.FindGroup(groupName);
            Student extractedStudent = _isuService.FindStudent(studentName);
            
            Assert.AreEqual(extractedGroup, extractedStudent.Group);
            Assert.Contains(extractedStudent, extractedGroup.Students);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                const string studentName = "John Johnson";
                Group group = _isuService.AddGroup(new GroupName(new CourseNumber(2), new GroupNumber(8)));
                

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
                _isuService.AddGroup(new GroupName(new CourseNumber(7), new GroupNumber(26)));
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            const string studentName = "Steven Stevenson";
            var oldGroupName = new GroupName(new CourseNumber(4), new GroupNumber(83));
            var newGroupName = new GroupName(new CourseNumber(4), new GroupNumber(25));

            Group oldGroup = _isuService.AddGroup(oldGroupName);
            Group newGroup = _isuService.AddGroup(newGroupName);
            
            Student student = _isuService.AddStudent(oldGroup, studentName);
            _isuService.ChangeStudentGroup(student, newGroup);
            
            Assert.AreEqual(newGroup, student.Group);
            Assert.IsFalse(oldGroup.Students.Contains(student));
            Assert.IsTrue(newGroup.Students.Contains(student));
        }
    }
}