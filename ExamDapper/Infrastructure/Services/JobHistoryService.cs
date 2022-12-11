namespace Infrastructure.Services;
using Domain.Entities;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;
public class JobHistoryService
{
    private DataContext _context;

    public JobHistoryService(DataContext context)
    {
        _context = context;
    }
    

    public async Task<Response<List<GetJobHistory>>> GetJobHistory()
    {
        var list = await _context.JobHistories.Select(j => new GetJobHistory()
        {
           JobId=j.job.JobId,
           EmployeId=j.employee.EmployeeId,
           DepartmentId=j.department.DepartmentId,
            StartDate = j.StartDate,
            EndDate = j.EndDate
        }).ToListAsync();

        return new Response<List<GetJobHistory>>(list);
            
    }
    public async Task<Response<AddJobHistory>> InsertJobHistory(AddJobHistory job)
    {
        var newJobHistory = new JobHistory()
        {
             EmployeeId = job.EmployeeId,
            StartDate = job.StartDate,
            EndDate = job.EndDate,
            DepartmentId = job.DepartmentId,
                JobId = job.JobId
        };
         _context.JobHistories.Add(newJobHistory);

         await _context.SaveChangesAsync();

         return new Response<AddJobHistory>(job);
    }
        public async Task<Response<AddJobHistory>> UpdateJobHistory(AddJobHistory job)
        {
            var find = await _context.JobHistories.FindAsync(job.EmployeeId);
            if(find != null) return null;
             find.EmployeeId = job.EmployeeId;
            find.StartDate = job.StartDate;
            find.EndDate = job.EndDate;
            find.JobId = job.JobId;
            find.DepartmentId = job.DepartmentId;

            await _context.SaveChangesAsync();

            return new Response<AddJobHistory>(job);
        }
      public async Task<Response<string>> DeleteJobHistory(int id)
        {      
        var find = await _context.JobHistories.FindAsync(id);
        _context.JobHistories.Remove(find);
        await _context.SaveChangesAsync();
           if(find.EmployeeId > 0 )  return new Response<string>("JobHistory deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "JobHistory not found");
        }
}