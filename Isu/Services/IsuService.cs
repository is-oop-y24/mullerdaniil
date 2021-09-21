using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Models.Group;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private const int MaxStudentPerGroup = 20;
        private static int _currentId;

        private List<Student> _students = new List<Student>();
        private List<Group> _groups = new List<Group>();

        public Group AddGroup(GroupName name)
        {
            var addedGroup = new Group(name);
            if (_groups.Contains(new Group(name)))
                throw new IsuException("Group has been already added");
            _groups.Add(addedGroup);
            return addedGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (!_groups.Contains(group))
                throw new IsuException("No such group found");

            var newStudent = new Student(_currentId++, name, group);
            _students.Add(newStudent);
            if (!IsGroupValid(group))
                throw new IsuException("Too many students in the group.");
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            Student student = _students.First(student => student.Id == id);

            try
            {
                return student;
            }
            catch (InvalidOperationException)
            {
                throw new IsuException("Can't get a student by id");
            }
        }

        public Student FindStudent(string name)
        {
            return _students.Find(student => student.Name == name);
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _students.FindAll(student => student.Group.GroupName.Equals(groupName));
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _students.FindAll(student => student.Group.GroupName.CourseNumber == courseNumber);
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups.Find(group => group.GroupName.Equals(groupName));
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.FindAll(group => group.GroupName.CourseNumber.Equals(courseNumber));
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (!_students.Contains(student))
            {
                throw new IsuException("No such student found");
            }

            if (!_groups.Contains(newGroup))
            {
                throw new IsuException("No such group found");
            }

            _students.Remove(student);
            student = new Student(student.Id, student.Name, newGroup);
            _students.Add(student);

            if (!IsGroupValid(newGroup))
                throw new IsuException("Too many students in the group.");
        }

        private bool IsGroupValid(Group group)
        {
            int studentsCount = _students.Count(student => student.Group.Equals(group));
            return studentsCount <= MaxStudentPerGroup;
        }
    }
}