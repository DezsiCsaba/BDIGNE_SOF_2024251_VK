using System.Reflection;
using Dapper;

namespace bdigne_api.Db;

using Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Define DbSet for each entity/table
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Activity> Activities { get; set; }

    // private List<string> DbSetNames { get; set; } = new List<string>();
    #region Clear DB and Seed data

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Enum to Int conversions
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<int>();
        
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<int>();
        
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Priority)
            .HasConversion<int>();
        #endregion
        
        #region Model relations //TODO - cyclical lekérdezések kezelése
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.AssigneeId);
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Reporter)
            .WithMany(u => u.ReportedTickets)
            .HasForeignKey(t => t.ReporterId);
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectId);
        
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Createdby)
            .WithMany(u => u.ProjectsCreated)
            .HasForeignKey(p => p.CreatedbyId);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Ticket)
            .WithMany(t => t.Activities)
            .HasForeignKey(a => a.TicketId);
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId);
        #endregion
    }
    public void CheckTablesAndRecreateIfNeeded(bool force = false)
    {
        if (force)
        {
            Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Users;");
            Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Tickets;");
            Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Projects;");
            Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Activities;");
        }
        
        #region User Table
        var tableExists = Database.GetDbConnection().QuerySingleOrDefault<bool>(
            "SELECT COUNT(*) > 0 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Users'");
        if (!tableExists)
        {
            Database.ExecuteSqlRaw(@"
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY AUTO_INCREMENT,
                        UserName VARCHAR(256) NOT NULL,
                        Email VARCHAR(256) NOT NULL,
                        Password VARCHAR(256) NOT NULL,
                        Role INT NOT NULL
                    )
                ");
        }
        #endregion
        
        #region Ticket Table
        tableExists = Database.GetDbConnection().QuerySingleOrDefault<bool>(
            "SELECT COUNT(*) > 0 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Tickets'");
        if (!tableExists)
        {
            Database.ExecuteSqlRaw(@"
                    CREATE TABLE Tickets (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description VARCHAR(255) NOT NULL,
                        Status INT NOT NULL,
                        Priority INT NOT NULL,
                        AssigneeId INT NOT NULL,
                        ReporterId INT NOT NULL,
                        ProjectId INT NOT NULL,
                        DueDate DATETIME NULL, -- NULL, mert optional-re akartam állítani NEM HIBA
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                    )
                ");
        }
        #endregion
        
        #region Projects Table
        tableExists = Database.GetDbConnection().QuerySingleOrDefault<bool>(
            "SELECT COUNT(*) > 0 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Projects'");
        if (!tableExists)
        {
            Database.ExecuteSqlRaw(@"
                CREATE TABLE Projects (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Name VARCHAR(255) NOT NULL,
                    ShortName VARCHAR(10) NOT NULL,
                    Description VARCHAR(255) NOT NULL,
                    CreatedById INT NOT NULL,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                )
            ");
        }
        #endregion

        #region Activities Table
        tableExists = Database.GetDbConnection().QuerySingleOrDefault<bool>(
            "SELECT COUNT(*) > 0 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Activities'");
        if (!tableExists)
        {
            Database.ExecuteSqlRaw(@"
                CREATE TABLE Activities (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    UserId INT NOT NULL,
                    TicketId INT,
                    Event VARCHAR(255) NOT NULL,
                    Type VARCHAR(30) NOT NULL,
                    Original VARCHAR(30) NOT NULL,
                    New VARCHAR(30) NOT NULL,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )
            ");
        }
        #endregion
    }
    public void ClearDatabase()
    {
        Database.ExecuteSqlRaw("DELETE FROM Users");
        Database.ExecuteSqlRaw("DELETE FROM Tickets");
        Database.ExecuteSqlRaw("DELETE FROM Projects");
        Database.ExecuteSqlRaw("DELETE FROM Activities");
    }
    
    public void Seed()
    {
        Users.AddRange(new User[]
        {
            new User { Id = 1, UserName = "dezsics", Email = "admin@example.com", Password = "AdminPass123", Role = UserRole.SiteDeveloper },
            new User { Id = 2, UserName = "admin", Email = "admin@example.com", Password = "AdminPass123", Role = UserRole.Admin },
            new User { Id = 3, UserName = "dev1", Email = "dev1@example.com", Password = "DevPass123", Role = UserRole.Dev },
            new User { Id = 4, UserName = "dev2", Email = "dev2@example.com", Password = "DevPass123", Role = UserRole.Dev },
            new User { Id = 5, UserName = "dev3", Email = "dev3@example.com", Password = "DevPass123", Role = UserRole.Dev },
            new User { Id = 6, UserName = "dev4", Email = "dev4@example.com", Password = "DevPass123", Role = UserRole.Dev },
            new User { Id = 7, UserName = "dev5", Email = "dev5@example.com", Password = "DevPass123", Role = UserRole.Dev },
            new User { Id = 8, UserName = "pm1", Email = "pm1@example.com", Password = "PmPass123", Role = UserRole.ProjectManager },
            new User { Id = 9, UserName = "pm2", Email = "pm2@example.com", Password = "PmPass123", Role = UserRole.ProjectManager },
            new User { Id = 10, UserName = "pm3", Email = "pm3@example.com", Password = "PmPass123", Role = UserRole.ProjectManager },
            new User { Id = 11, UserName = "pm4", Email = "pm4@example.com", Password = "PmPass123", Role = UserRole.ProjectManager }
        });
        
        Tickets.AddRange(new Ticket[]
        {
            new Ticket
            {
                Id = 1,
                Title = "MIKROBI User Authentication",
                Description = "Implement user authentication for the MIKROBI project with OAuth2 support.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.High,
                AssigneeId = 1,
                ProjectId = 1,
                ReporterId = 2,
                DueDate = new DateTime(2024, 9, 15, 17, 0, 0),
                CreatedAt = new DateTime(2024, 7, 1, 9, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 15, 12, 0, 0)
            },
            new Ticket
            {
                Id = 2,
                Title = "Client Portal Login Bug",
                Description = "Fix the login issue on the client portal where users are unable to login with valid credentials.",
                Status = TicketStatus.Open,
                Priority = PriorityLevel.Medium,
                AssigneeId = 3,
                ReporterId = 5,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 14, 18, 0, 0),
                CreatedAt = new DateTime(2024, 7, 10, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 20, 14, 0, 0)
            },
            new Ticket
            {
                Id = 3,
                Title = "Inventory System Reporting",
                Description = "Implement real-time reporting for inventory levels across all warehouses.",
                Status = TicketStatus.Done,
                Priority = PriorityLevel.Low,
                AssigneeId = 4,
                ReporterId = 6,
                ProjectId = 3,
                DueDate = new DateTime(2024, 9, 12, 15, 0, 0),
                CreatedAt = new DateTime(2024, 7, 5, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 25, 13, 0, 0)
            },
            new Ticket
            {
                Id = 4,
                Title = "Employee Onboarding API",
                Description = "Develop the API for the Employee Onboarding process, including employee data management.",
                Status = TicketStatus.Closed,
                Priority = PriorityLevel.Medium,
                AssigneeId = 5,
                ReporterId = 7,
                ProjectId = 4,
                DueDate = new DateTime(2024, 9, 8, 12, 0, 0),
                CreatedAt = new DateTime(2024, 7, 18, 9, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 30, 10, 0, 0)
            },
            new Ticket
            {
                Id = 5,
                Title = "MIKROBI Dashboard",
                Description = "Create the dashboard interface for MIKROBI with interactive charts and reports.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.High,
                AssigneeId = 6,
                ReporterId = 8,
                ProjectId = 1,
                DueDate = new DateTime(2024, 9, 10, 16, 0, 0),
                CreatedAt = new DateTime(2024, 7, 23, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 29, 14, 0, 0)
            },
            new Ticket
            {
                Id = 6,
                Title = "Client Portal Notifications",
                Description = "Add notification system to the client portal to alert users about updates and changes.",
                Status = TicketStatus.Open,
                Priority = PriorityLevel.Low,
                AssigneeId = 7,
                ReporterId = 9,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 13, 12, 0, 0),
                CreatedAt = new DateTime(2024, 7, 29, 13, 0, 0),
                UpdatedAt = new DateTime(2024, 9, 1, 14, 0, 0)
            },
            new Ticket
            {
                Id = 7,
                Title = "Inventory System Restock Alerts",
                Description = "Implement restock alerts for low inventory levels in the Inventory System.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.Medium,
                AssigneeId = 8,
                ReporterId = 10,
                ProjectId = 3,
                DueDate = new DateTime(2024, 9, 5, 10, 0, 0),
                CreatedAt = new DateTime(2024, 8, 2, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 9, 1, 9, 0, 0)
            },
            new Ticket
            {
                Id = 8,
                Title = "Employee Onboarding Portal Design",
                Description = "Revamp the user interface of the Employee Onboarding portal to improve user experience.",
                Status = TicketStatus.Done,
                Priority = PriorityLevel.High,
                AssigneeId = 9,
                ReporterId = 11,
                ProjectId = 4,
                DueDate = new DateTime(2024, 9, 2, 17, 0, 0),
                CreatedAt = new DateTime(2024, 7, 12, 14, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 20, 15, 0, 0)
            },
            new Ticket
            {
                Id = 9,
                Title = "MIKROBI Security Enhancements",
                Description = "Apply security enhancements to MIKROBI, including encryption and user role management.",
                Status = TicketStatus.Open,
                Priority = PriorityLevel.High,
                AssigneeId = 2,
                ReporterId = 1,
                ProjectId = 1,
                DueDate = new DateTime(2024, 9, 6, 12, 0, 0),
                CreatedAt = new DateTime(2024, 8, 10, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 25, 12, 0, 0)
            },
            new Ticket
            {
                Id = 10,
                Title = "Client Portal Billing Integration",
                Description = "Integrate billing and invoicing system with the Client Portal for seamless transactions.",
                Status = TicketStatus.Closed,
                Priority = PriorityLevel.Medium,
                AssigneeId = 3,
                ReporterId = 6,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 11, 17, 0, 0),
                CreatedAt = new DateTime(2024, 7, 20, 14, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 30, 15, 0, 0)
            },
            // 10 more tickets follow the same structure
            new Ticket
            {
                Id = 11,
                Title = "MIKROBI Analytics Integration",
                Description = "Integrate analytics dashboard to monitor user engagement and performance in MIKROBI.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.Low,
                AssigneeId = 4,
                ReporterId = 5,
                ProjectId = 1,
                DueDate = new DateTime(2024, 9, 12, 16, 0, 0),
                CreatedAt = new DateTime(2024, 7, 30, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 27, 14, 0, 0)
            },
            new Ticket
            {
                Id = 12,
                Title = "Client Portal Dark Mode",
                Description = "Implement dark mode feature for the client portal to enhance user experience.",
                Status = TicketStatus.Done,
                Priority = PriorityLevel.High,
                AssigneeId = 7,
                ReporterId = 2,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 13, 13, 0, 0),
                CreatedAt = new DateTime(2024, 8, 1, 12, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 29, 16, 0, 0)
            },
            new Ticket
            {
                Id = 13,
                Title = "Inventory System Barcode Scanning",
                Description = "Add barcode scanning capability for faster inventory processing and tracking.",
                Status = TicketStatus.Open,
                Priority = PriorityLevel.Medium,
                AssigneeId = 6,
                ReporterId = 4,
                ProjectId = 3,
                DueDate = new DateTime(2024, 9, 14, 12, 0, 0),
                CreatedAt = new DateTime(2024, 7, 31, 13, 0, 0),
                UpdatedAt = new DateTime(2024, 9, 5, 10, 0, 0)
            },
            new Ticket
            {
                Id = 14,
                Title = "Employee Onboarding Docs Upload",
                Description = "Enable document upload functionality in Employee Onboarding for streamlined document submission.",
                Status = TicketStatus.Closed,
                Priority = PriorityLevel.Low,
                AssigneeId = 1,
                ReporterId = 10,
                ProjectId = 4,
                DueDate = new DateTime(2024, 9, 5, 17, 0, 0),
                CreatedAt = new DateTime(2024, 8, 1, 11, 0, 0),
                UpdatedAt = new DateTime(2024, 9, 1, 12, 0, 0)
            },
            new Ticket
            {
                Id = 15,
                Title = "MIKROBI Performance Improvements",
                Description = "Optimize MIKROBI for better performance, especially under heavy user loads.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.High,
                AssigneeId = 11,
                ReporterId = 9,
                ProjectId = 1,
                DueDate = new DateTime(2024, 9, 13, 18, 0, 0),
                CreatedAt = new DateTime(2024, 7, 15, 14, 0, 0),
                UpdatedAt = new DateTime(2024, 9, 1, 13, 0, 0)
            },
            new Ticket
            {
                Id = 16,
                Title = "Client Portal User Feedback System",
                Description = "Implement user feedback system for clients to submit reviews and suggestions.",
                Status = TicketStatus.Done,
                Priority = PriorityLevel.Medium,
                AssigneeId = 5,
                ReporterId = 8,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 7, 15, 0, 0),
                CreatedAt = new DateTime(2024, 7, 25, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 22, 11, 0, 0)
            },
            new Ticket
            {
                Id = 17,
                Title = "Inventory System UI Overhaul",
                Description = "Overhaul the UI of the Inventory System to improve usability and efficiency.",
                Status = TicketStatus.Closed,
                Priority = PriorityLevel.Low,
                AssigneeId = 2,
                ReporterId = 11,
                ProjectId = 3,
                DueDate = new DateTime(2024, 9, 10, 14, 0, 0),
                CreatedAt = new DateTime(2024, 7, 26, 9, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 28, 12, 0, 0)
            },
            new Ticket
            {
                Id = 18,
                Title = "MIKROBI Admin Panel",
                Description = "Develop an admin panel for MIKROBI to manage users, roles, and system configurations.",
                Status = TicketStatus.InProgress,
                Priority = PriorityLevel.High,
                AssigneeId = 8,
                ReporterId = 1,
                ProjectId = 1,
                DueDate = new DateTime(2024, 9, 15, 17, 0, 0),
                CreatedAt = new DateTime(2024, 8, 5, 13, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 30, 15, 0, 0)
            },
            new Ticket
            {
                Id = 19,
                Title = "Client Portal GDPR Compliance",
                Description = "Ensure GDPR compliance for the Client Portal by adding necessary security and privacy measures.",
                Status = TicketStatus.Done,
                Priority = PriorityLevel.High,
                AssigneeId = 10,
                ReporterId = 3,
                ProjectId = 2,
                DueDate = new DateTime(2024, 9, 9, 16, 0, 0),
                CreatedAt = new DateTime(2024, 7, 20, 14, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 29, 11, 0, 0)
            },
            new Ticket
            {
                Id = 20,
                Title = "Inventory System Auto-Update",
                Description = "Implement an auto-update feature for the Inventory System to push updates without downtime.",
                Status = TicketStatus.Open,
                Priority = PriorityLevel.Medium,
                AssigneeId = 9,
                ReporterId = 7,
                ProjectId = 3,
                DueDate = new DateTime(2024, 9, 12, 17, 0, 0),
                CreatedAt = new DateTime(2024, 7, 30, 15, 0, 0),
                UpdatedAt = new DateTime(2024, 8, 31, 14, 0, 0)
            }
        });
        
        Projects.AddRange(new Project[]
        {
            new Project
            {
                Id = 1,
                Name = "MIKROBI",
                ShortName = "MIKROBI",
                Description = "An internally developed website for managing everything within the company, from HR to Project Management.",
                CreatedbyId = 8,
                CreatedAt = new DateTime(2024, 06, 25, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 08, 20, 10, 0, 0)
            },
            new Project
            {
                Id = 2,
                Name = "Client Portal",
                ShortName = "CLIPORT",
                Description = "A portal for clients to manage their orders and track project progress.",
                CreatedbyId = 9, // Assuming 2 is the ID of another user
                CreatedAt = new DateTime(2024, 6, 27, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 08, 22, 12, 30, 0)
            },
            new Project
            {
                Id = 3,
                Name = "Inventory System",
                ShortName = "INVSYS",
                Description = "A system to manage and track inventory and supplies in real-time.",
                CreatedbyId = 10,
                CreatedAt = new DateTime(2024, 07, 01, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 08, 25, 14, 15, 0)
            },
            new Project
            {
                Id = 4,
                Name = "Employee Onboarding",
                ShortName = "EMPING",
                Description = "An application to streamline the employee onboarding process, from documentation to training.",
                CreatedbyId = 11,
                CreatedAt = new DateTime(2024, 07, 03, 10, 0, 0),
                UpdatedAt = new DateTime(2024, 09, 01, 9, 0, 0)
            }
        });

        Activities.AddRange(new Activity[]
        {
            new Activity
            {
                Id = 1,
                UserId = 2,
                TicketId = 1,
                Event = "<div>%user% made changes - %date%<br>%type% - <b>Original: </b>%original%, <b>New: </b>%new%</div>",
                Type = "Status",
                Original = "Open",
                New = "In Progress",
                CreatedAt = new DateTime(2024, 8, 15, 12, 0, 0)
            },
        });
        SaveChanges();
    }
    #endregion
}
