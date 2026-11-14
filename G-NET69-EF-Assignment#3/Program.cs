
using G_NET69_EF_Assignment_3.Classes;
using G_NET69_EF_Assignment_3.Data;
using G_NET69_EF_Assignment_3.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace G_NET69_EF_Assignment_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using AppDBContext appContext = new AppDBContext();

            #region Seeding

            SeedData(appContext);

            #endregion

            #region Create

            int instructorId = AddInstructor(appContext);

            int categoryId = appContext.Categories
                .Select(c => c.Id).FirstOrDefault();

            int courseId = AddCourse(appContext, instructorId, categoryId);

            Students[] students = AddStudents(appContext);

            EnrollStudent(appContext, students[0].Id, courseId);
            EnrollStudent(appContext, students[1].Id, courseId);

            #endregion

            #region Read

            FindCourse(appContext, courseId, "Introduction to Programming");

            FilterCourses(appContext, 500, 1200);

            int parentId = appContext.Categories
                .Where(c => c.ParentCategoryId != null)
                .Select(c => c.ParentCategoryId)
                .FirstOrDefault() ?? 0;

            GetSubCategories(appContext, parentId);

            CourseRoster(appContext, courseId);

            #endregion

            #region Update

            ChangePrice(appContext, courseId, 1000);

            UpdateProgress(appContext, students[0].Id, courseId, 100);

            int unpublishedId = appContext.Courses
                .Where(c => !c.IsPublished)
                .Select(c => c.Id)
                .FirstOrDefault();

            PublishCourse(appContext, unpublishedId);

            #endregion

            #region Delete

            Unenroll(appContext, students[0].Id, courseId);

            DeleteCategory(appContext, parentId);

            SafeDeleteCategory(appContext, parentId);

            DeleteCourse(appContext, courseId);

            #endregion

            #region Bulk Operations

            BulkPriceRaise(appContext, CourseLevel.Beginner);

            BulkDelete(appContext);

            #endregion
        }

        #region Seeding

        static void SeedData(AppDBContext appContext)
        {
            if (!appContext.Students.Any())
            {
                Students[] students = new Students[]
                {
                    new Students
                    {
                        FullName = "Ahmed Ali",
                        EmailAddress = "ahmed@example.com",
                        DateOfBirth = new DateOnly(2000, 5, 10)
                    },
                    new Students
                    {
                        FullName = "Mohamed Hassan",
                        EmailAddress = "mohamed@example.com",
                        DateOfBirth = new DateOnly(2001, 3, 12)
                    },
                    new Students
                    {
                        FullName = "Omar Khaled",
                        EmailAddress = "omar@example.com",
                        DateOfBirth = new DateOnly(1999, 8, 20)
                    },
                    new Students
                    {
                        FullName = "Sara Ahmed",
                        EmailAddress = "sara@example.com",
                        DateOfBirth = new DateOnly(2002, 6, 15)
                    },
                    new Students
                    {
                        FullName = "Mona Ali",
                        EmailAddress = "mona@example.com",
                        DateOfBirth = new DateOnly(2000, 9, 25)
                    }
                };

                appContext.Students.AddRange(students);
                appContext.SaveChanges();

                Console.WriteLine("Students seeded successfully.");
            }

            if (!appContext.Enrollments.Any())
            {
                var students = appContext.Students
                    .Take(5).ToList();

                var courses = appContext.Courses
                    .Where(c => c.IsPublished).ToList();

                if (students.Count * courses.Count < 8)
                {
                    Console.WriteLine("Not enough students or published courses.");
                    return;
                }

                List<Enrollment> enrollments = new List<Enrollment>();

                foreach (var student in students)
                {
                    foreach (var course in courses)
                    {
                        if (enrollments.Count == 8)
                            break;

                        Enrollment enrollment = new Enrollment()
                        {
                            StudentId = student.Id,
                            CourseId = course.Id,
                            EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                            Status = EnrollmentStatus.Active,
                            Percentage = 20,
                            FinalGrade = null
                        };

                        if (enrollments.Count % 3 == 1)
                        {
                            enrollment.Status = EnrollmentStatus.Completed;
                            enrollment.Percentage = 100;
                            enrollment.FinalGrade = 90;
                        }
                        else if (enrollments.Count % 3 == 2)
                        {
                            enrollment.Status = EnrollmentStatus.Dropped;
                            enrollment.Percentage = 40;
                        }

                        enrollments.Add(enrollment);
                    }

                    if (enrollments.Count == 8)
                        break;
                }

                appContext.Enrollments.AddRange(enrollments);
                appContext.SaveChanges();

                Console.WriteLine("Enrollments seeded successfully.");
            }
        }

        #endregion

        #region Create Operations

        static int AddInstructor(AppDBContext appContext)
        {
            Instructors instructor = new Instructors()
            {
                FullName = "John Doe",
                ShortBio = "An experienced instructor in computer science.",
                EmailAddress = "john.doe@example.com",
                JoinedDate = new DateOnly(2020, 1, 15)
            };

            appContext.Instructors.Add(instructor);
            appContext.SaveChanges();

            Console.WriteLine($"Instructor Id: {instructor.Id}");

            return instructor.Id;
        }

        static int AddCourse(AppDBContext appContext, int instructorId, int categoryId)
        {
            if (!appContext.Instructors.Any(i => i.Id == instructorId))
            {
                Console.WriteLine("Instructor not found.");
                return 0;
            }

            if (!appContext.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine("Category not found.");
                return 0;
            }

            Course course = new Course()
            {
                Title = "Introduction to Programming",
                Description = "Learn the basics of programming using C#.",
                Price = 750,
                Level = CourseLevel.Beginner,
                IsPublished = true,
                PublishedDate = DateOnly.FromDateTime(DateTime.Now),
                CategoryId = categoryId,
                InstructorId = instructorId
            };

            appContext.Courses.Add(course);
            appContext.SaveChanges();

            Console.WriteLine($"Course added. Id: {course.Id}");

            return course.Id;
        }

        static Students[] AddStudents(AppDBContext appContext)
        {
            Students[] students = new Students[]
            {
                new Students
                {
                    FullName = "Alice Smith",
                    EmailAddress = "alice.smith@example.com",
                    DateOfBirth = new DateOnly(2000, 3, 15)
                },
                new Students
                {
                    FullName = "Bob Johnson",
                    EmailAddress = "bob.johnson@example.com",
                    DateOfBirth = new DateOnly(1999, 7, 22)
                },
                new Students
                {
                    FullName = "Charlie Brown",
                    EmailAddress = "charlie.brown@example.com",
                    DateOfBirth = new DateOnly(2001, 11, 5)
                }
            };

            appContext.Students.AddRange(students);
            appContext.SaveChanges();

            foreach (var student in students)
            {
                Console.WriteLine($"{student.FullName} - Id: {student.Id}");
            }

            return students;
        }

        static void EnrollStudent(AppDBContext appContext, int studentId, int courseId)
        {
            var student = appContext.Students.Find(studentId);
            var course = appContext.Courses.Find(courseId);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine("Course is not published.");
                return;
            }

            if (appContext.Enrollments.Any(e => e.StudentId == studentId
                                            && e.CourseId == courseId))
            {
                Console.WriteLine("Student is already enrolled.");
                return;
            }

            Enrollment enrollment = new Enrollment()
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Status = EnrollmentStatus.Active,
                Percentage = 0,
                FinalGrade = null
            };

            appContext.Enrollments.Add(enrollment);
            appContext.SaveChanges();

            Console.WriteLine("Student enrolled successfully.");
        }

        #endregion

        #region Read Operations

        static void FindCourse(AppDBContext appContext, int id, string title)
        {
            var course1 = appContext.Courses.Find(id);

            if (course1 != null)
            {
                Console.WriteLine($"Course: {course1.Title}");
            }
            else
            {
                Console.WriteLine("not found");
            }

            var course2 = appContext.Courses
                .FirstOrDefault(c => c.Title == title);

            if (course2 != null)
            {
                Console.WriteLine($"Course: {course2.Title}");
            }
            else
            {
                Console.WriteLine("not found");
            }
        }

        static void FilterCourses(AppDBContext appContext, decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || minPrice > maxPrice)
            {
                Console.WriteLine("Invalid price range.");
                return;
            }

            var filteredCourses = appContext.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })
                .ToList();

            if (filteredCourses.Count == 0)
            {
                Console.WriteLine("No courses found.");
                return;
            }

            foreach (var course in filteredCourses)
            {
                Console.WriteLine($"{course.Title}, {course.Level}, {course.Price}");
            }
        }

        static void GetSubCategories(AppDBContext appContext, int categoryId)
        {
            if (!appContext.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine("Category not found.");
            }
            else
            {
                var subCategories = appContext.Categories
                    .Where(c => c.ParentCategoryId == categoryId)
                    .Select(c => c.Name).ToList();

                Console.WriteLine("Sub Categories:");

                foreach (var category in subCategories)
                {
                    Console.WriteLine(category);
                }
            }

            var topCategories = appContext.Categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => c.Name).ToList();

            Console.WriteLine("Top Level Categories:");

            foreach (var category in topCategories)
            {
                Console.WriteLine(category);
            }
        }

        static void CourseRoster(AppDBContext appContext, int courseId)
        {
            if (!appContext.Courses.Any(c => c.Id == courseId))
            {
                Console.WriteLine("Course not found.");
                return;
            }

            var studentIds = appContext.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId).ToList();

            Console.WriteLine($"Number of students: {studentIds.Count}");

            var students = appContext.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => s.FullName).ToList();

            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        #endregion

        #region Update Operations

        static void ChangePrice(AppDBContext appContext, int courseId, decimal newPrice)
        {
            if (newPrice < 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            var course = appContext.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            course.Price = newPrice;

            appContext.SaveChanges();

            Console.WriteLine($"New Price: {course.Price}");
        }

        static void UpdateProgress(AppDBContext appContext, int studentId,
                                   int courseId, decimal progress)
        {
            if (progress < 0 || progress > 100)
            {
                Console.WriteLine("Progress must be between 0 and 100.");
                return;
            }

            var enrollment = appContext.Enrollments.Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            enrollment.Percentage = progress;

            if (progress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }
            else if (enrollment.Status == EnrollmentStatus.Completed)
            {
                enrollment.Status = EnrollmentStatus.Active;
                enrollment.FinalGrade = null;
            }

            appContext.SaveChanges();

            Console.WriteLine($"Progress: {enrollment.Percentage}");
            Console.WriteLine($"Status: {enrollment.Status}");
        }

        static void PublishCourse(AppDBContext appContext, int courseId)
        {
            var course = appContext.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine("Course is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedDate = DateOnly.FromDateTime(DateTime.Now);

            appContext.SaveChanges();

            Console.WriteLine("Course published successfully.");
        }

        #endregion

        #region Delete Operations

        static void Unenroll(AppDBContext appContext, int studentId, int courseId)
        {
            var enrollment = appContext.Enrollments.Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            appContext.Enrollments.Remove(enrollment);
            appContext.SaveChanges();

            Console.WriteLine("Enrollment deleted successfully.");
        }

        static void DeleteCategory(AppDBContext appContext, int categoryId)
        {
            appContext.ChangeTracker.Clear();

            var category = appContext.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            try
            {
                appContext.Categories.Remove(category);
                appContext.SaveChanges();

                Console.WriteLine("Category deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);

                appContext.Entry(category).State = EntityState.Unchanged;
            }
        }

        static void SafeDeleteCategory(AppDBContext appContext, int categoryId)
        {
            var category = appContext.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            if (appContext.Categories.Any(c => c.ParentCategoryId == categoryId))
            {
                Console.WriteLine("Category has sub categories.");
                return;
            }

            if (appContext.Courses.Any(c => c.CategoryId == categoryId))
            {
                Console.WriteLine("Category has courses.");
                return;
            }

            appContext.Categories.Remove(category);
            appContext.SaveChanges();

            Console.WriteLine("Category deleted successfully.");
        }

        static void DeleteCourse(AppDBContext appContext, int courseId)
        {
            var course = appContext.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            int before = appContext.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine($"Enrollments before delete: {before}");

            appContext.Courses.Remove(course);
            appContext.SaveChanges();

            int after = appContext.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine($"Enrollments after delete: {after}");

            // Enrollments are deleted automatically because of Cascade Delete.
        }

        #endregion

        #region Bulk Operations

        static void BulkPriceRaise(AppDBContext appContext, CourseLevel level)
        {
            int affectedRows = appContext.Courses
                .Where(c => c.IsPublished && c.Level == level)
                .ExecuteUpdate(c => c.SetProperty(p => p.Price,
                                                  p => p.Price * 1.10m));

            Console.WriteLine($"Updated courses: {affectedRows}");
        }

        static void BulkDelete(AppDBContext appContext)
        {
            int affectedRows = appContext.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();

            Console.WriteLine($"Deleted enrollments: {affectedRows}");
        }

        #endregion
    }
}
