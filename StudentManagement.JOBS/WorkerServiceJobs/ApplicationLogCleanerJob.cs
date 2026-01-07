using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StudentManagement.BLL.Signature;
using StudentManagement.DAL;
using StudentManagement.ENTITY;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;
using System.Reflection;
using static StudentManagement.ENTITY.Logger;

namespace StudentManagement.JOBS.WorkerServiceJobs
{
    public class InsertTStudentStageDataIntoTStudent : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public InsertTStudentStageDataIntoTStudent(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var _Logger = scope.ServiceProvider.GetRequiredService<IApplicationLogger>();
                var _BLL = scope.ServiceProvider.GetRequiredService<IBLLCommon>();
                var _Db = scope.ServiceProvider.GetRequiredService<StudentManagementDbContext>();

                try
                {
                    //var cutoffTime = DateTime.Now.AddMinutes(-10);

                    bool isStage = await _Db.TStudentStage
                        .AnyAsync(x => x.IsStage == (int)IsStaged.False, stoppingToken);

                    if (isStage)
                    {
                        await _BLL.RunAsync(stoppingToken);
                    }

                }
                catch (Exception ex)
                {
                    _Logger.ManageException(
                        ex,
                        MethodBase.GetCurrentMethod()!,
                        LogAssemblyEnum.JOBS
                    );
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}