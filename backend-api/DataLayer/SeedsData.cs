using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HrManagementSystem.DataLayer.Entities;

namespace Hr.DL
{
    public class SeedsData : IEntitySeeder
    {
        public void SeedData(ModelBuilder modelBuilder)
        {
            SeedCompanies(modelBuilder);
            SeedDepartments(modelBuilder);
            SeedEmployees(modelBuilder);
            SeedEvents(modelBuilder);
            SeedExpenses(modelBuilder);
            SeedHolidays(modelBuilder);
            SeedLeaves(modelBuilder);
            SeedNotifications(modelBuilder);
            SeedResumes(modelBuilder);
            SeedRoles(modelBuilder);
            SeedShifts(modelBuilder);
        }

        private void SeedCompanies(ModelBuilder modelBuilder)
        {
            var companies = new List<Company>
            {
                new Company {
                    Id = 1,
                    Name = "TechVista Solutions",
                    RegistrationDate = new DateTime(2022, 3, 15),
                    EstablishmentDate = new DateTime(2022, 3, 15),
                    Address = "Levent Mah. İş Cad. No: 5, Beşiktaş, İstanbul",
                    Email = "info@techvista.com",
                    PhoneNumber = "+90 212 555 1234",
                    EmployeeCount = 1,
                    IsApproved = true,
                    SubscriptionEndDate = new DateTime(2028, 3, 15)
                },
                new Company {
                    Id = 2,
                    Name = "InnovaTech Yazılım A.Ş.",
                    RegistrationDate = new DateTime(2023, 5, 20),
                    EstablishmentDate = new DateTime(2012, 8, 10),
                    Address = "Bahçelievler Mah. Teknoloji Cad. No: 42, Çankaya, Ankara",
                    Email = "contact@innovatech.com",
                    PhoneNumber = "+90 312 444 5678",
                    EmployeeCount = 6,
                    IsApproved = true,
                    SubscriptionEndDate = new DateTime(2024, 5, 20)
                },
                new Company {
                    Id = 3,
                    Name = "DataSphere Bilişim Ltd. Şti.",
                    RegistrationDate = new DateTime(2023, 9, 1),
                    EstablishmentDate = new DateTime(2017, 2, 28),
                    Address = "Alsancak Mah. Veri Sok. No: 15, Konak, İzmir",
                    Email = "info@datasphere.com",
                    PhoneNumber = "+90 232 333 9876",
                    EmployeeCount = 4,
                    IsApproved = true,
                    SubscriptionEndDate = new DateTime(2024, 9, 1)
                },
                new Company {
                    Id = 4,
                    Name = "CloudPeak Teknoloji A.Ş.",
                    RegistrationDate = new DateTime(2023, 11, 10),
                    EstablishmentDate = new DateTime(2019, 6, 5),
                    Address = "Esentepe Mah. Bulut Cad. No: 78, Şişli, İstanbul",
                    Email = "info@cloudpeak.com",
                    PhoneNumber = "+90 216 777 5432",
                    EmployeeCount = 5,
                    IsApproved = true,
                    SubscriptionEndDate = new DateTime(2024, 11, 10)
                }
            };

            modelBuilder.Entity<Company>().HasData(companies);
        }

        private void SeedDepartments(ModelBuilder modelBuilder)
        {
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Yönetim Kurulu", Description = "Şirket stratejik yönetimi", CompanyId = 1 },

                new Department { Id = 2, Name = "Ar-Ge", Description = "Yazılım araştırma ve geliştirme departmanı", CompanyId = 2 },
                new Department { Id = 3, Name = "İnsan Kaynakları", Description = "Personel yönetimi ve işe alım departmanı", CompanyId = 2 },
                new Department { Id = 4, Name = "Muhasebe", Description = "Finansal işlemler ve raporlama departmanı", CompanyId = 2 },

                new Department { Id = 5, Name = "Veri Analizi", Description = "Büyük veri analizi ve raporlama departmanı", CompanyId = 3 },
                new Department { Id = 6, Name = "Müşteri İlişkileri", Description = "Müşteri hizmetleri ve destek departmanı", CompanyId = 3 },
                new Department { Id = 7, Name = "Sistem Yönetimi", Description = "IT altyapı ve güvenlik yönetimi departmanı", CompanyId = 3 },

                new Department { Id = 8, Name = "Bulut Hizmetleri", Description = "Bulut tabanlı çözümler geliştirme departmanı", CompanyId = 4 },
                new Department { Id = 9, Name = "Proje Yönetimi", Description = "Yazılım proje yönetimi departmanı", CompanyId = 4 },
                new Department { Id = 10, Name = "Ürün Geliştirme", Description = "Yeni ürün geliştirme ve tasarım departmanı", CompanyId = 4 }
            };

            modelBuilder.Entity<Department>().HasData(departments);
        }

        private void SeedEmployees(ModelBuilder modelBuilder)
        {
            var employees = new List<Employee>();
            var applicationUsers = new List<ApplicationUser>();
            var userRoles = new List<IdentityUserRole<string>>();

            // Admin user for TechVista Solutions
            var adminUserId = "22e40406-8a9d-2d82-912c-5d6a640ee696";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@techvista.com",
                NormalizedUserName = "ADMIN@TECHVISTA.COM",
                Email = "admin@techvista.com",
                NormalizedEmail = "ADMIN@TECHVISTA.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "Sistem",
                LastName = "Yöneticisi",
                Position = "Sistem Yöneticisi",
                CompanyId = 1,
                IsManager = true,
                RefreshToken = string.Empty
            };
            applicationUsers.Add(adminUser);

            // Employees for companies
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 1, "Berk", "Yılmaz", "Genel Müdür", 2, 2, true, "704f6694-7020-48f6-bd2c-10e22732c830", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 2, "Elif", "Öztürk", "Yazılım Mühendisi", 2, 2, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 3, "Can", "Kaya", "Yazılım Mimarı", 2, 2, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 4, "Deniz", "Aydın", "İK Uzmanı", 2, 3, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 5, "Emre", "Çelik", "İK Müdürü", 2, 3, true, "704f6694-7020-48f6-bd2c-10e22732c830", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 6, "Furkan", "Demir", "Veri Analisti", 3, 5, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 7, "Gizem", "Arslan", "Veri Bilimci", 3, 5, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 8, "Hakan", "Yıldırım", "Müşteri Hizmetleri Temsilcisi", 3, 6, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 9, "İrem", "Koç", "Müşteri İlişkileri Müdürü", 3, 6, true, "704f6694-7020-48f6-bd2c-10e22732c830", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 10, "Kerem", "Özer", "Bulut Mimarı", 4, 8, true, "704f6694-7020-48f6-bd2c-10e22732c830", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee010"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 11, "Leyla", "Şahin", "DevOps Mühendisi", 4, 8, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee011"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 12, "Murat", "Akar", "Proje Yöneticisi", 4, 9, true, "704f6694-7020-48f6-bd2c-10e22732c830", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee012"));
            AddEmployeeAndUser(employees, applicationUsers, userRoles, 13, "Neslihan", "Güneş", "UX/UI Tasarımcısı", 4, 10, false, "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee013"));

            modelBuilder.Entity<Employee>().HasData(employees);
            modelBuilder.Entity<ApplicationUser>().HasData(applicationUsers);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }

        private void AddEmployeeAndUser(List<Employee> employees, List<ApplicationUser> users, List<IdentityUserRole<string>> userRoles, int id, string firstName, string lastName, string position, int companyId, int departmentId, bool isManager, string roleId, Guid employeeId)
        {
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com";
            var userId = employeeId.ToString();

            var employee = new Employee
            {
                Id = id,
                EmployeeId = employeeId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = $"+90 5{new Random().Next(10, 60):D2} {new Random().Next(100, 1000):D3} {new Random().Next(10, 100):D2} {new Random().Next(10, 100):D2}",
                Birthdate = new DateTime(new Random().Next(1980, 2000), new Random().Next(1, 13), new Random().Next(1, 29)),
                Position = position,
                HireDate = new DateTime(new Random().Next(2020, 2024), new Random().Next(1, 13), new Random().Next(1, 29)),
                DepartmentId = departmentId,
                CompanyId = companyId
            };
            employees.Add(employee);

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "User123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                CompanyId = companyId,
                IsManager = isManager,
                RefreshToken = string.Empty
            };
            users.Add(user);

            if (!userRoles.Exists(ur => ur.UserId == userId && ur.RoleId == roleId))
            {
                userRoles.Add(new IdentityUserRole<string> { UserId = userId, RoleId = roleId });
            }
        }

        private void SeedEvents(ModelBuilder modelBuilder)
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Teknoloji Zirvesi",
                    Description = "Yıllık teknoloji trendleri ve inovasyon zirvesi",
                    EventDate = new DateTime(2024, 5, 15, 9, 0, 0),
                    CompanyId = 2
                },
                new Event
                {
                    Id = 2,
                    Title = "Proje Kickoff Toplantısı",
                    Description = "Yeni bulut projesinin başlangıç toplantısı",
                    EventDate = new DateTime(2024, 6, 1, 14, 0, 0),
                    CompanyId = 4
                },
                new Event
                {
                    Id = 3,
                    Title = "Veri Güvenliği Semineri",
                    Description = "Güncel veri güvenliği uygulamaları hakkında seminer",
                    EventDate = new DateTime(2024, 7, 10, 10, 0, 0),
                    CompanyId = 3
                },
                new Event
                {
                    Id = 4,
                    Title = "Yıl Sonu Değerlendirme Toplantısı",
                    Description = "2024 yılı performans ve hedef değerlendirmesi",
                    EventDate = new DateTime(2024, 12, 20, 15, 0, 0),
                    CompanyId = 2
                },
                new Event
                {
                    Id = 5,
                    Title = "Agile Metodolojiler Eğitimi",
                    Description = "Proje yönetiminde agile metodolojiler üzerine eğitim",
                    EventDate = new DateTime(2024, 8, 5, 9, 0, 0),
                    CompanyId = 4
                },
                new Event
                {
                    Id = 6,
                    Title = "Müşteri Deneyimi Çalıştayı",
                    Description = "Müşteri memnuniyetini artırma stratejileri üzerine çalıştay",
                    EventDate = new DateTime(2024, 9, 15, 13, 0, 0),
                    CompanyId = 3
                },
                new Event
                {
                    Id = 7,
                    Title = "Yapay Zeka ve Etik Paneli",
                    Description = "AI kullanımında etik konular üzerine panel tartışması",
                    EventDate = new DateTime(2024, 10, 1, 11, 0, 0),
                    CompanyId = 2
                },
                new Event
                {
                    Id = 8,
                    Title = "Siber Güvenlik Konferansı",
                    Description = "Güncel siber tehditler ve korunma yöntemleri konferansı",
                    EventDate = new DateTime(2024, 11, 12, 10, 0, 0),
                    CompanyId = 4
                }
            };

            modelBuilder.Entity<Event>().HasData(events);
        }

        private void SeedExpenses(ModelBuilder modelBuilder)
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Id = 1,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    ExpenseType = "Konferans",
                    Amount = 1500.00m,
                    ExpenseDate = new DateTime(2023, 6, 15),
                    Description = "Yıllık Yazılım Geliştirme Konferansı katılım ücreti",
                    Status = "Approved",
                    ApproverComments = "Onaylandı, şirket için faydalı olacak",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 2,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"),
                    ExpenseType = "Ekipman",
                    Amount = 2500.50m,
                    ExpenseDate = new DateTime(2023, 7, 20),
                    Description = "Yeni geliştirme laptopı",
                    Status = "Pending",
                    ApproverComments = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 3,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"),
                    ExpenseType = "Seyahat",
                    Amount = 850.75m,
                    ExpenseDate = new DateTime(2023, 8, 5),
                    Description = "Müşteri ziyareti uçak bileti",
                    Status = "Approved",
                    ApproverComments = "Onaylandı",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 4,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"),
                    ExpenseType = "Eğitim",
                    Amount = 1200.00m,
                    ExpenseDate = new DateTime(2023, 9, 10),
                    Description = "İnsan Kaynakları Yönetimi Sertifikası",
                    Status = "Approved",
                    ApproverComments = "Onaylandı, kariyer gelişimi için önemli",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 5,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    ExpenseType = "Yazılım Lisansı",
                    Amount = 3000.00m,
                    ExpenseDate = new DateTime(2023, 10, 1),
                    Description = "Yıllık kurumsal yazılım lisansı yenilemesi",
                    Status = "Approved",
                    ApproverComments = "Onaylandı, gerekli yazılım",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 6,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"),
                    ExpenseType = "Ofis Malzemeleri",
                    Amount = 450.25m,
                    ExpenseDate = new DateTime(2023, 11, 15),
                    Description = "Kırtasiye ve ofis sarf malzemeleri",
                    Status = "Approved",
                    ApproverComments = "Onaylandı",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 7,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"),
                    ExpenseType = "Networking",
                    Amount = 300.00m,
                    ExpenseDate = new DateTime(2023, 12, 5),
                    Description = "Sektör networking etkinliği katılım ücreti",
                    Status = "Pending",
                    ApproverComments = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Expense
                {
                    Id = 8,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"),
                    ExpenseType = "Müşteri Ağırlama",
                    Amount = 550.50m,
                    ExpenseDate = new DateTime(2024, 1, 10),
                    Description = "Potansiyel müşteri ile akşam yemeği",
                    Status = "Approved",
                    ApproverComments = "Onaylandı, iş geliştirme faaliyeti",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<Expense>().HasData(expenses);
        }

        private void SeedHolidays(ModelBuilder modelBuilder)
        {
            var holidays = new List<Holiday>
            {
                new Holiday { Id = 1, Name = "Yeni Yıl", Date = new DateTime(2024, 1, 1), CompanyId = 2, Description = "Yeni yıl kutlaması" },
                new Holiday { Id = 2, Name = "Ulusal Egemenlik ve Çocuk Bayramı", Date = new DateTime(2024, 4, 23), CompanyId = 2, Description = "23 Nisan kutlamaları" },
                new Holiday { Id = 3, Name = "Emek ve Dayanışma Günü", Date = new DateTime(2024, 5, 1), CompanyId = 3, Description = "1 Mayıs İşçi Bayramı" },
                new Holiday { Id = 4, Name = "Atatürk'ü Anma, Gençlik ve Spor Bayramı", Date = new DateTime(2024, 5, 19), CompanyId = 3, Description = "19 Mayıs kutlamaları" },
                new Holiday { Id = 5, Name = "Demokrasi ve Milli Birlik Günü", Date = new DateTime(2024, 7, 15), CompanyId = 4, Description = "15 Temmuz anma etkinlikleri" },
                new Holiday { Id = 6, Name = "Zafer Bayramı", Date = new DateTime(2024, 8, 30), CompanyId = 4, Description = "30 Ağustos kutlamaları" },
                new Holiday { Id = 7, Name = "Cumhuriyet Bayramı", Date = new DateTime(2024, 10, 29), CompanyId = 2, Description = "29 Ekim kutlamaları" }
            };
            modelBuilder.Entity<Holiday>().HasData(holidays);
        }

        private void SeedLeaves(ModelBuilder modelBuilder)
        {
            var leaves = new List<Leave>
    {
        new Leave
        {
            Id = 1,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
            StartDate = new DateTime(2023, 7, 10),
            EndDate = new DateTime(2023, 7, 17),
            LeaveType = "Yıllık İzin",
            Reason = "Yaz tatili",
            Status = "Approved",
            Comments = "İyi tatiller",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 2,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"),
            StartDate = new DateTime(2023, 8, 5),
            EndDate = new DateTime(2023, 8, 7),
            LeaveType = "Hastalık İzni",
            Reason = "Grip",
            Status = "Approved",
            Comments = "Geçmiş olsun",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 3,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"),
            StartDate = new DateTime(2023, 9, 20),
            EndDate = new DateTime(2023, 9, 22),
            LeaveType = "Özel İzin",
            Reason = "Aile etkinliği",
            Status = "Approved",
            Comments = "Onaylandı",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 4,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"),
            StartDate = new DateTime(2023, 10, 15),
            EndDate = new DateTime(2023, 10, 20),
            LeaveType = "Yıllık İzin",
            Reason = "Kişisel gezi",
            Status = "Pending",
            Comments = "İnceleme aşamasında",
            ApproverId = null,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 5,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
            StartDate = new DateTime(2023, 11, 1),
            EndDate = new DateTime(2023, 11, 3),
            LeaveType = "Konferans İzni",
            Reason = "İK Konferansı katılımı",
            Status = "Approved",
            Comments = "Konferans raporu bekleniyor",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 6,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"),
            StartDate = new DateTime(2023, 12, 25),
            EndDate = new DateTime(2023, 12, 29),
            LeaveType = "Yıllık İzin",
            Reason = "Yılbaşı tatili",
            Status = "Approved",
            Comments = "İyi tatiller",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 7,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"),
            StartDate = new DateTime(2024, 1, 15),
            EndDate = new DateTime(2024, 1, 17),
            LeaveType = "Hastalık İzni",
            Reason = "Soğuk algınlığı",
            Status = "Approved",
            Comments = "Doktor raporu alındı",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 8,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"),
            StartDate = new DateTime(2024, 2, 5),
            EndDate = new DateTime(2024, 2, 9),
            LeaveType = "Yıllık İzin",
            Reason = "Kış tatili",
            Status = "Pending",
            Comments = "Değerlendirme aşamasında", 
            ApproverId = null,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        },
        new Leave
        {
            Id = 9,
            EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
            StartDate = new DateTime(2024, 3, 1),
            EndDate = new DateTime(2024, 3, 1),
            LeaveType = "Özel İzin",
            Reason = "Kişisel işler",
            Status = "Approved",
            Comments = "Onaylandı",
            ApproverId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        }
    };

            modelBuilder.Entity<Leave>().HasData(leaves);
        }

        private void SeedNotifications(ModelBuilder modelBuilder)
        {
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Message = "Yeni proje başlatıldı: CloudSync",
                    DateSent = new DateTime(2023, 6, 1),
                    DateRead = null,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    NotificationType = "Proje",
                    CompanyId = 2,
                    Priority = "High",
                    ExpiryDate = new DateTime(2023, 6, 8),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 2,
                    Message = "Yıllık performans değerlendirmeleri başlıyor",
                    DateSent = new DateTime(2023, 7, 15),
                    DateRead = new DateTime(2023, 7, 15),
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    NotificationType = "İK",
                    CompanyId = 2,
                    Priority = "Medium",
                    ExpiryDate = new DateTime(2023, 7, 30),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 3,
                    Message = "Yeni güvenlik politikası yayınlandı",
                    DateSent = new DateTime(2023, 8, 10),
                    DateRead = null,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
                    NotificationType = "Güvenlik",
                    CompanyId = 3,
                    Priority = "High",
                    ExpiryDate = new DateTime(2023, 8, 17),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 4,
                    Message = "Ofis taşınma süreci başlıyor",
                    DateSent = new DateTime(2023, 9, 1),
                    DateRead = new DateTime(2023, 9, 2),
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    NotificationType = "Genel",
                    CompanyId = 2,
                    Priority = "Medium",
                    ExpiryDate = new DateTime(2023, 9, 15),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 5,
                    Message = "Yeni müşteri kazanıldı: TechCorp",
                    DateSent = new DateTime(2023, 10, 5),
                    DateRead = null,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    NotificationType = "Satış",
                    CompanyId = 2,
                    Priority = "High",
                    ExpiryDate = new DateTime(2023, 10, 12),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 6,
                    Message = "Yeni eğitim programı başlıyor: AI ve ML Temelleri",
                    DateSent = new DateTime(2023, 11, 1),
                    DateRead = null,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    NotificationType = "Eğitim",
                    CompanyId = 3,
                    Priority = "Medium",
                    ExpiryDate = new DateTime(2023, 11, 15),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 7,
                    Message = "Sistem bakımı planlı kesinti",
                    DateSent = new DateTime(2023, 12, 10),
                    DateRead = new DateTime(2023, 12, 10),
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee010"),
                    NotificationType = "IT",
                    CompanyId = 4,
                    Priority = "High",
                    ExpiryDate = new DateTime(2023, 12, 11),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 8,
                    Message = "Yılbaşı kutlaması hatırlatması",
                    DateSent = new DateTime(2023, 12, 20),
                    DateRead = null,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    NotificationType = "Etkinlik",
                    CompanyId = 3,
                    Priority = "Low",
                    ExpiryDate = new DateTime(2023, 12, 31),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = 9,
                    Message = "Yeni yıl hedefleri toplantısı",
                    DateSent = new DateTime(2024, 1, 5),
                    DateRead = new DateTime(2024, 1, 6),
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
                    SenderId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    NotificationType = "Toplantı",
                    CompanyId = 4,
                    Priority = "Medium",
                    ExpiryDate = new DateTime(2024, 1, 10),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<Notification>().HasData(notifications);
        }

        private void SeedResumes(ModelBuilder modelBuilder)
        {
            var resumes = new List<Resume>
            {
                new Resume
                {
                    Id = 1,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    AdditionalInformation = "10+ yıl yazılım geliştirme ve yöneticilik deneyimi"
                },
                new Resume
                {
                    Id = 2,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"),
                    AdditionalInformation = "Full-stack geliştirici, mikroservis mimarileri konusunda uzman"
                },
                new Resume
                {
                    Id = 3,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"),
                    AdditionalInformation = "Yazılım mimarisi ve büyük ölçekli sistemler konusunda deneyimli"
                },
                new Resume
                {
                    Id = 4,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"),
                    AdditionalInformation = "İK süreçleri optimizasyonu ve çalışan deneyimi konularında uzman"
                },
                new Resume
                {
                    Id = 5,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    AdditionalInformation = "Organizasyonel gelişim ve yetenek yönetimi konularında deneyimli"
                },
                new Resume
                {
                    Id = 6,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"),
                    AdditionalInformation = "Makine öğrenmesi ve veri madenciliği alanlarında uzman"
                },
                new Resume
                {
                    Id = 7,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"),
                    AdditionalInformation = "Büyük veri analizi ve iş zekası çözümleri konusunda deneyimli"
                },
                new Resume
                {
                    Id = 8,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"),
                    AdditionalInformation = "Müşteri ilişkileri yönetimi ve sorun çözme becerileri güçlü"
                },
                new Resume
                {
                    Id = 9,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
                    AdditionalInformation = "Müşteri memnuniyeti stratejileri geliştirme konusunda uzman"
                },
                new Resume
                {
                    Id = 10,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee010"),
                    AdditionalInformation = "Bulut mimarisi ve DevOps uygulamaları konusunda ileri düzey bilgi"
                },
                new Resume
                {
                    Id = 11,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee011"),
                    AdditionalInformation = "CI/CD ve konteynerizasyon teknolojileri konusunda uzman"
                },
                new Resume
                {
                    Id = 12,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee012"),
                    AdditionalInformation = "Agile ve Scrum metodolojileri konusunda sertifikalı"
                },
                new Resume
                {
                    Id = 13,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee013"),
                    AdditionalInformation = "Kullanıcı deneyimi tasarımı ve kullanılabilirlik testleri konusunda deneyimli"
                }
            };

            var educations = new List<Education>
            {
                new Education
                {
                    Id = 1,
                    ResumeId = 1,
                    SchoolName = "İstanbul Teknik Üniversitesi",
                    Degree = "Yüksek Lisans",
                    FieldOfStudy = "Bilgisayar Mühendisliği",
                    StartDate = new DateTime(2008, 9, 1),
                    EndDate = new DateTime(2010, 6, 1)
                },
                new Education
                {
                    Id = 2,
                    ResumeId = 2,
                    SchoolName = "Boğaziçi Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "Yazılım Mühendisliği",
                    StartDate = new DateTime(2010, 9, 1),
                    EndDate = new DateTime(2014, 6, 1)
                },
                new Education
                {
                    Id = 3,
                    ResumeId = 3,
                    SchoolName = "Orta Doğu Teknik Üniversitesi",
                    Degree = "Doktora",
                    FieldOfStudy = "Bilgisayar Bilimleri",
                    StartDate = new DateTime(2012, 9, 1),
                    EndDate = new DateTime(2016, 6, 1)
                },
                new Education
                {
                    Id = 4,
                    ResumeId = 4,
                    SchoolName = "Marmara Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "İnsan Kaynakları Yönetimi",
                    StartDate = new DateTime(2011, 9, 1),
                    EndDate = new DateTime(2015, 6, 1)
                },
                new Education
                {
                    Id = 5,
                    ResumeId = 5,
                    SchoolName = "Yıldız Teknik Üniversitesi",
                    Degree = "Yüksek Lisans",
                    FieldOfStudy = "İşletme",
                    StartDate = new DateTime(2013, 9, 1),
                    EndDate = new DateTime(2015, 6, 1)
                },
                new Education
                {
                    Id = 6,
                    ResumeId = 6,
                    SchoolName = "Hacettepe Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "İstatistik",
                    StartDate = new DateTime(2012, 9, 1),
                    EndDate = new DateTime(2016, 6, 1)
                },
                new Education
                {
                    Id = 7,
                    ResumeId = 7,
                    SchoolName = "Ankara Üniversitesi",
                    Degree = "Yüksek Lisans",
                    FieldOfStudy = "Veri Bilimi",
                    StartDate = new DateTime(2015, 9, 1),
                    EndDate = new DateTime(2017, 6, 1)
                },
                new Education
                {
                    Id = 8,
                    ResumeId = 8,
                    SchoolName = "Ege Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "İşletme",
                    StartDate = new DateTime(2013, 9, 1),
                    EndDate = new DateTime(2017, 6, 1)
                },
                new Education
                {
                    Id = 9,
                    ResumeId = 9,
                    SchoolName = "Dokuz Eylül Üniversitesi",
                    Degree = "Yüksek Lisans",
                    FieldOfStudy = "Müşteri İlişkileri Yönetimi",
                    StartDate = new DateTime(2014, 9, 1),
                    EndDate = new DateTime(2016, 6, 1)
                },
                new Education
                {
                    Id = 10,
                    ResumeId = 10,
                    SchoolName = "Bilkent Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "Bilgisayar Mühendisliği",
                    StartDate = new DateTime(2011, 9, 1),
                    EndDate = new DateTime(2015, 6, 1)
                },
                new Education
                {
                    Id = 11,
                    ResumeId = 11,
                    SchoolName = "Sabancı Üniversitesi",
                    Degree = "Yüksek Lisans",
                    FieldOfStudy = "Bilgi Teknolojileri",
                    StartDate = new DateTime(2016, 9, 1),
                    EndDate = new DateTime(2018, 6, 1)
                },
                new Education
                {
                    Id = 12,
                    ResumeId = 12,
                    SchoolName = "Koç Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "Endüstri Mühendisliği",
                    StartDate = new DateTime(2012, 9, 1),
                    EndDate = new DateTime(2016, 6, 1)
                },
                new Education
                {
                    Id = 13,
                    ResumeId = 13,
                    SchoolName = "Galatasaray Üniversitesi",
                    Degree = "Lisans",
                    FieldOfStudy = "Bilgisayar Mühendisliği",
                    StartDate = new DateTime(2014, 9, 1),
                    EndDate = new DateTime(2018, 6, 1)
                }
            };

            var workExperiences = new List<WorkExperience>
            {
                new WorkExperience
                {
                    Id = 1,
                    ResumeId = 1,
                    CompanyName = "TechSoft A.Ş.",
                    Position = "Yazılım Geliştirme Müdürü",
                    StartDate = new DateTime(2015, 1, 1),
                    EndDate = new DateTime(2020, 12, 31),
                    Description = "Büyük ölçekli yazılım projelerini yönetti ve ekip liderliği yaptı."
                },
                new WorkExperience
                {
                    Id = 2,
                    ResumeId = 2,
                    CompanyName = "CloudTech",
                    Position = "Kıdemli Yazılım Mühendisi",
                    StartDate = new DateTime(2016, 3, 1),
                    EndDate = new DateTime(2021, 9, 30),
                    Description = "Mikroservis mimarileri üzerinde çalıştı ve DevOps süreçlerini iyileştirdi."
                },
                new WorkExperience
                {
                    Id = 3,
                    ResumeId = 3,
                    CompanyName = "DataWiz",
                    Position = "Yazılım Mimarı",
                    StartDate = new DateTime(2017, 7, 1),
                    EndDate = new DateTime(2022, 8, 30),
                    Description = "Yüksek ölçeklenebilir sistemler tasarladı ve uyguladı."
                },
                new WorkExperience
                {
                    Id = 4,
                    ResumeId = 4,
                    CompanyName = "PeopleTech",
                    Position = "İK Uzmanı",
                    StartDate = new DateTime(2015, 2, 1),
                    EndDate = new DateTime(2020, 6, 30),
                    Description = "İşe alım süreçlerini optimize etti ve çalışan memnuniyetini artırdı."
                },
                new WorkExperience
                {
                    Id = 5,
                    ResumeId = 5,
                    CompanyName = "TalentCorp",
                    Position = "İK Müdürü",
                    StartDate = new DateTime(2017, 4, 1),
                    EndDate = new DateTime(2022, 12, 31),
                    Description = "Yetenek yönetimi stratejileri geliştirdi ve uyguladı."
                },
                new WorkExperience
                {
                    Id = 6,
                    ResumeId = 6,
                    CompanyName = "AILabs",
                    Position = "Veri Bilimci",
                    StartDate = new DateTime(2018, 3, 1),
                    EndDate = new DateTime(2023, 7, 31),
                    Description = "Makine öğrenmesi modellerini geliştirdi ve uyguladı."
                },
                new WorkExperience
                {
                    Id = 7,
                    ResumeId = 7,
                    Position = "Veri Analisti",
                    CompanyName = "BigDataCo",
                    StartDate = new DateTime(2017, 1, 1),
                    EndDate = new DateTime(2022, 10, 31),
                    Description = "Büyük veri setlerini analiz etti ve iş zekası raporları oluşturdu."
                },
                new WorkExperience
                {
                    Id = 8,
                    ResumeId = 8,
                    CompanyName = "CustomerFirst",
                    Position = "Müşteri Hizmetleri Temsilcisi",
                    StartDate = new DateTime(2018, 5, 1),
                    EndDate = new DateTime(2023, 9, 30),
                    Description = "Müşteri sorunlarını çözdü ve müşteri memnuniyetini artırdı."
                },
                new WorkExperience
                {
                    Id = 9,
                    ResumeId = 9,
                    CompanyName = "ServicePro",
                    Position = "Müşteri İlişkileri Müdürü",
                    StartDate = new DateTime(2016, 4, 1),
                    EndDate = new DateTime(2022, 3, 31),
                    Description = "Müşteri hizmetleri stratejilerini geliştirdi ve uyguladı."
                },
                new WorkExperience
                {
                    Id = 10,
                    ResumeId = 10,
                    CompanyName = "CloudMasters",
                    Position = "Bulut Mimarı",
                    StartDate = new DateTime(2015, 6, 1),
                    EndDate = new DateTime(2022, 12, 31),
                    Description = "Büyük ölçekli bulut altyapıları tasarladı ve uyguladı."
                },
                new WorkExperience
                {
                    Id = 11,
                    ResumeId = 11,
                    CompanyName = "DevOpsHub",
                    Position = "DevOps Mühendisi",
                    StartDate = new DateTime(2018, 9, 1),
                    EndDate = new DateTime(2023, 8, 31),
                    Description = "CI/CD süreçlerini otomatikleştirdi ve konteyner teknolojilerini uyguladı."
                },
                new WorkExperience
                {
                    Id = 12,
                    ResumeId = 12,
                    CompanyName = "AgileWorks",
                    Position = "Proje Yöneticisi",
                    StartDate = new DateTime(2017, 2, 1),
                    EndDate = new DateTime(2023, 11, 30),
                    Description = "Agile metodolojileri kullanarak büyük projeleri yönetti."
                },
                new WorkExperience
                {
                    Id = 13,
                    ResumeId = 13,
                    CompanyName = "UXStudio",
                    Position = "UX/UI Tasarımcısı",
                    StartDate = new DateTime(2019, 1, 1),
                    EndDate = new DateTime(2023, 10, 31),
                    Description = "Kullanıcı odaklı arayüzler tasarladı ve kullanılabilirlik testleri yaptı."
                }
            };

            var skills = new List<Skill>
            {
                new Skill
                {
                    Id = 1,
                    ResumeId = 1,
                    Name = "Proje Yönetimi",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 2,
                    ResumeId = 2,
                    Name = "Mikroservis Mimarileri",
                    Proficiency = "İleri Düzey"
                },
                new Skill
                {
                    Id = 3,
                    ResumeId = 3,
                    Name = "Sistem Tasarımı",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 4,
                    ResumeId = 4,
                    Name = "İşe Alım Süreçleri",
                    Proficiency = "İleri Düzey"
                },
                new Skill
                {
                    Id = 5,
                    ResumeId = 5,
                    Name = "Yetenek Yönetimi",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 6,
                    ResumeId = 6,
                    Name = "Makine Öğrenmesi",
                    Proficiency = "İleri Düzey"
                },
                new Skill
                {
                    Id = 7,
                    ResumeId = 7,
                    Name = "Veri Analizi",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 8,
                    ResumeId = 8,
                    Name = "Müşteri İlişkileri",
                    Proficiency = "İleri Düzey"
                },
                new Skill
                {
                    Id = 9,
                    ResumeId = 9,
                    Name = "Müşteri Hizmetleri Stratejisi",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 10,
                    ResumeId = 10,
                    Name = "Bulut Mimarisi",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 11,
                    ResumeId = 11,
                    Name = "DevOps",
                    Proficiency = "İleri Düzey"
                },
                new Skill
                {
                    Id = 12,
                    ResumeId = 12,
                    Name = "Agile Metodolojileri",
                    Proficiency = "Uzman"
                },
                new Skill
                {
                    Id = 13,
                    ResumeId = 13,
                    Name = "UX/UI Tasarımı",
                    Proficiency = "İleri Düzey"
                }
            };

            var certifications = new List<Certification>
            {
                new Certification
                {
                    Id = 1,
                    ResumeId = 1,
                    Name = "PMP (Project Management Professional)",
                    Issuer = "PMI",
                    DateIssued = new DateTime(2018, 5, 15)
                },
                new Certification
                {
                    Id = 2,
                    ResumeId = 2,
                    Name = "AWS Certified Solutions Architect",
                    Issuer = "Amazon Web Services",
                    DateIssued = new DateTime(2019, 8, 20)
                },
                new Certification
                {
                    Id = 3,
                    ResumeId = 3,
                    Name = "TOGAF Certified",
                    Issuer = "The Open Group",
                    DateIssued = new DateTime(2020, 11, 1)
                },
                new Certification
                {
                    Id = 4,
                    ResumeId = 4,
                    Name = "SHRM-CP (Certified Professional)",
                    Issuer = "SHRM",
                    DateIssued = new DateTime(2017, 10, 10)
                },
                new Certification
                {
                    Id = 5,
                    ResumeId = 5,
                    Name = "SPHR (Senior Professional in Human Resources)",
                    Issuer = "HRCI",
                    DateIssued = new DateTime(2019, 3, 15)
                },
                new Certification
                {
                    Id = 6,
                    ResumeId = 6,
                    Name = "TensorFlow Developer Certificate",
                    Issuer = "Google",
                    DateIssued = new DateTime(2021, 6, 20)
                },
                new Certification
                {
                    Id = 7,
                    ResumeId = 7,
                    Name = "Certified Analytics Professional",
                    Issuer = "INFORMS",
                    DateIssued = new DateTime(2020, 2, 1)
                },
                new Certification
                {
                    Id = 8,
                    ResumeId = 8,
                    Name = "CCXP (Certified Customer Experience Professional)",
                    Issuer = "CXPA",
                    DateIssued = new DateTime(2022, 7, 25)
                },
                new Certification
                {
                    Id = 9,
                    ResumeId = 9,
                    Name = "CCSP (Certified Customer Service Professional)",
                    Issuer = "ICMI",
                    DateIssued = new DateTime(2019, 9, 30)
                },
                new Certification
                {
                    Id = 10,
                    ResumeId = 10,
                    Name = "Google Cloud Professional Cloud Architect",
                    Issuer = "Google",
                    DateIssued = new DateTime(2021, 5, 5)
                },
                new Certification
                {
                    Id = 11,
                    ResumeId = 11,
                    Name = "Certified Kubernetes Administrator",
                    Issuer = "The Linux Foundation",
                    DateIssued = new DateTime(2022, 11, 10)
                },
                new Certification
                {
                    Id = 12,
                    ResumeId = 12,
                    Name = "PMI-ACP (Agile Certified Practitioner)",
                    Issuer = "PMI",
                    DateIssued = new DateTime(2020, 4, 15)
                },
                new Certification
                {
                    Id = 13,
                    ResumeId = 13,
                    Name = "Adobe Certified Expert - Experience Designer",
                    Issuer = "Adobe",
                    DateIssued = new DateTime(2021, 8, 30)
                }
            };

            var languages = new List<Language>
            {
                new Language
                {
                    Id = 1,
                    ResumeId = 1,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 2,
                    ResumeId = 2,
                    Name = "İngilizce",
                    Proficiency = "Akıcı"
                },
                new Language
                {
                    Id = 3,
                    ResumeId = 3,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 4,
                    ResumeId = 4,
                    Name = "İngilizce",
                    Proficiency = "Orta Düzey"
                },
                new Language
                {
                    Id = 5,
                    ResumeId = 5,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 6,
                    ResumeId = 6,
                    Name = "İngilizce",
                    Proficiency = "Akıcı"
                },
                new Language
                {
                    Id = 7,
                    ResumeId = 7,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 8,
                    ResumeId = 8,
                    Name = "İngilizce",
                    Proficiency = "Orta Düzey"
                },
                new Language
                {
                    Id = 9,
                    ResumeId = 9,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 10,
                    ResumeId = 10,
                    Name = "İngilizce",
                    Proficiency = "Akıcı"
                },
                new Language
                {
                    Id = 11,
                    ResumeId = 11,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                },
                new Language
                {
                    Id = 12,
                    ResumeId = 12,
                    Name = "İngilizce",
                    Proficiency = "Akıcı"
                },
                new Language
                {
                    Id = 13,
                    ResumeId = 13,
                    Name = "İngilizce",
                    Proficiency = "İleri Düzey"
                }
            };

            modelBuilder.Entity<Resume>().HasData(resumes);
            modelBuilder.Entity<Education>().HasData(educations);
            modelBuilder.Entity<WorkExperience>().HasData(workExperiences);
            modelBuilder.Entity<Skill>().HasData(skills);
            modelBuilder.Entity<Certification>().HasData(certifications);
            modelBuilder.Entity<Language>().HasData(languages);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            var adminRole = new IdentityRole
            {
                Id = "0fcba428-3c83-4ca4-b329-32bf4e78ea92",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var managerRole = new IdentityRole
            {
                Id = "704f6694-7020-48f6-bd2c-10e22732c830",
                Name = "Manager",
                NormalizedName = "MANAGER"
            };

            var employeeRole = new IdentityRole
            {
                Id = "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            };

            modelBuilder.Entity<IdentityRole>().HasData(adminRole, managerRole, employeeRole);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = "22e40406-8a9d-2d82-912c-5d6a640ee696" // Admin user ID
                }
            );
        }

        private void SeedShifts(ModelBuilder modelBuilder)
        {
            var shifts = new List<Shift>
            {
                new Shift
                {
                    Id = 1,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee001"),
                    StartTime = new DateTime(2023, 8, 1, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 1, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Sprint planlama toplantısı"
                },
                new Shift
                {
                    Id = 2,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee002"),
                    StartTime = new DateTime(2023, 8, 2, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 2, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Kod gözden geçirme"
                },
                new Shift
                {
                    Id = 3,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee003"),
                    StartTime = new DateTime(2023, 8, 3, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 3, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Mimari tasarım toplantısı"
                },
                new Shift
                {
                    Id = 4,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee004"),
                    StartTime = new DateTime(2023, 8, 4, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 4, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "İşe alım görüşmeleri"
                },
                new Shift
                {
                    Id = 5,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee005"),
                    StartTime = new DateTime(2023, 8, 5, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 5, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Performans değerlendirme toplantıları"
                },
                new Shift
                {
                    Id = 6,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee006"),
                    StartTime = new DateTime(2023, 8, 6, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 6, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Veri analizi projesi"
                },
                new Shift
                {
                    Id = 7,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee007"),
                    StartTime = new DateTime(2023, 8, 7, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 7, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Rapor hazırlama"
                },
                new Shift
                {
                    Id = 8,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee008"),
                    StartTime = new DateTime(2023, 8, 8, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 8, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Müşteri destek çağrıları"
                },
                new Shift
                {
                    Id = 9,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee009"),
                    StartTime = new DateTime(2023, 8, 9, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 9, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Müşteri memnuniyeti analizi"
                },
                new Shift
                {
                    Id = 10,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee010"),
                    StartTime = new DateTime(2023, 8, 10, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 10, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Bulut altyapı optimizasyonu"
                },
                new Shift
                {
                    Id = 11,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee011"),
                    StartTime = new DateTime(2023, 8, 11, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 11, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "CI/CD pipeline güncellemesi"
                },
                new Shift
                {
                    Id = 12,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee012"),
                   StartTime = new DateTime(2023, 8, 12, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 12, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Sprint retrospektif toplantısı"
                },
                new Shift
                {
                    Id = 13,
                    EmployeeId = Guid.Parse("22e40406-8a9d-2d82-912c-5d6a640ee013"),
                    StartTime = new DateTime(2023, 8, 13, 9, 0, 0),
                    EndTime = new DateTime(2023, 8, 13, 18, 0, 0),
                    ShiftType = "Tam Gün",
                    Notes = "Kullanıcı arayüzü tasarım revizyonu"
                }
            };

            modelBuilder.Entity<Shift>().HasData(shifts);
        }
    }
}