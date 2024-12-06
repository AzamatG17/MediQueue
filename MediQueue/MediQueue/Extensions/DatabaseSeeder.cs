using MediQueue.Domain.Entities;
using MediQueue.Infrastructure.Persistence;

namespace MediQueue.Extensions
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<MediQueueDbContext>();

            CreateController(context);
        }

        private static void CreateController(MediQueueDbContext mediQueueDbContext)
        {
            if (mediQueueDbContext.Controllers.Any()) return;

            List<Controller> controllers = new List<Controller>()
            {
                new Controller { Id = 1, ControllerName = "Account", IsActive = true },
                new Controller { Id = 2, ControllerName = "Branch", IsActive = true },
                new Controller { Id = 3, ControllerName = "Category", IsActive = true },
                new Controller { Id = 4, ControllerName = "CategoryLekarstvo", IsActive = true },
                new Controller { Id = 5, ControllerName = "Group", IsActive = true },
                new Controller { Id = 6, ControllerName = "Lekarstvo", IsActive = true },
                new Controller { Id = 7, ControllerName = "PaymentService", IsActive = true },
                new Controller { Id = 8, ControllerName = "Permission", IsActive = true },
                new Controller { Id = 9, ControllerName = "Questionnaire", IsActive = true },
                new Controller { Id = 10, ControllerName = "QuestionnaireHistory", IsActive = true },
                new Controller { Id = 11, ControllerName = "Role", IsActive = true },
                new Controller { Id = 12, ControllerName = "Sclad", IsActive = true },
                new Controller { Id = 13, ControllerName = "Service", IsActive = true },
                new Controller { Id = 14, ControllerName = "Medicine", IsActive = true },
                new Controller { Id = 15, ControllerName = "Conclusion", IsActive = true },
                new Controller { Id = 16, ControllerName = "PaymentLekarstvo", IsActive = true },
                new Controller { Id = 17, ControllerName = "AnalysisResult", IsActive = true },
                new Controller { Id = 18, ControllerName = "ScladLekarstvo", IsActive = true },
                new Controller { Id = 19, ControllerName = "DoctorCabinet", IsActive = true },
                new Controller { Id = 20, ControllerName = "DoctorCabinetLekarstvo", IsActive = true },
                new Controller { Id = 21, ControllerName = "Partiya", IsActive = true },
                new Controller { Id = 22, ControllerName = "Authorization", IsActive = true },
                new Controller { Id = 23, ControllerName = "Sample", IsActive = true },
                new Controller { Id = 24, ControllerName = "Discount", IsActive = true },
                new Controller { Id = 25, ControllerName = "Benefit", IsActive = true },
                new Controller { Id = 26, ControllerName = "ServiceUsage", IsActive = true },
                new Controller { Id = 27, ControllerName = "Nutrition", IsActive = true },
                new Controller { Id = 28, ControllerName = "StationaryStay", IsActive = true },
                new Controller { Id = 29, ControllerName = "Tariff", IsActive = true },
                new Controller { Id = 30, ControllerName = "WardPlace", IsActive = true },
                new Controller { Id = 31, ControllerName = "Ward", IsActive = true }
            };

            mediQueueDbContext.Controllers.AddRange(controllers);
            mediQueueDbContext.SaveChanges();
        }
    }
}
