using MediQueue.Domain.Entities;
using MediQueue.Infrastructure.Persistence;

namespace MediQueue.Helpers
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
                new Controller { ControllerName = "Account", IsActive = true },
                new Controller { ControllerName = "Branch", IsActive = true },
                new Controller { ControllerName = "Category", IsActive = true },
                new Controller { ControllerName = "CategoryLekarstvo", IsActive = true },
                new Controller { ControllerName = "Group", IsActive = true },
                new Controller { ControllerName = "Lekarstvo", IsActive = true },
                new Controller { ControllerName = "PaymentService", IsActive = true },
                new Controller { ControllerName = "Permission", IsActive = true },
                new Controller { ControllerName = "Questionnaire", IsActive = true },
                new Controller { ControllerName = "QuestionnaireHistory", IsActive = true },
                new Controller { ControllerName = "Role", IsActive = true },
                new Controller { ControllerName = "Sclad", IsActive = true },
                new Controller { ControllerName = "Service", IsActive = true },
                new Controller { ControllerName = "Medicine", IsActive = true },
                new Controller { ControllerName = "Conclusion", IsActive = true },
                new Controller { ControllerName = "PaymentLekarstvo", IsActive = true },
                new Controller { ControllerName = "AnalysisResult", IsActive = true },
                new Controller { ControllerName = "ScladLekarstvo", IsActive = true },
                new Controller { ControllerName = "DoctorCabinet", IsActive = true },
                new Controller { ControllerName = "DoctorCabinetLekarstvo", IsActive = true },
                new Controller { ControllerName = "Partiya", IsActive = true },
                new Controller { ControllerName = "Authorization", IsActive = true },
                new Controller { ControllerName = "Sample", IsActive = true },
                new Controller { ControllerName = "Discount", IsActive = true },
                new Controller { ControllerName = "Benefit", IsActive = true },
                new Controller { ControllerName = "ServiceUsage", IsActive = true },
                new Controller { ControllerName = "Nutrition", IsActive = true },
                new Controller { ControllerName = "StationaryStay", IsActive = true },
                new Controller { ControllerName = "Tariff", IsActive = true },
                new Controller { ControllerName = "WardPlace", IsActive = true },
                new Controller { ControllerName = "Ward", IsActive = true },
                new Controller { ControllerName = "AuditLog", IsActive = true },
                new Controller { ControllerName = "ProcedureBooking", IsActive = true },
                new Controller { ControllerName = "ProcedureCategory", IsActive = true},
                new Controller { ControllerName = "Procedure", IsActive = true },
            };

            mediQueueDbContext.Controllers.AddRange(controllers);
            mediQueueDbContext.SaveChanges();
        }
    }
}
