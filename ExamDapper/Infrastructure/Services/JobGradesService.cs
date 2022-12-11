using System.Net;

using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;


namespace Infrastructure.Services;

public class JobGradesService
{
    private readonly DataContext _context;


    public JobGradesService(DataContext context)
    {
        _context = context;
        
    }

     public async Task<Response<List<GetJobGrades>>> GetJobGrades()
        {       
       var list = await _context.JobGrades.Select(j => new GetJobGrades()
        {  JobGradesId=j.JobGradesId,
            GradeLevel=j.GradeLevel,
            LowestSai=j.LowestSai,
            HighestSai=j.HighestSai
          
        
        }).ToListAsync();

        return new Response<List<GetJobGrades>>(list);
        }

        public async Task <Response<GetJobGrades>> InsertJobGrades(GetJobGrades job)
        {
 var newJob= new JobGrades()
        {     
            GradeLevel=job.GradeLevel,
            LowestSai=job.LowestSai,
            HighestSai=job.HighestSai

        };
        _context.JobGrades.Add(newJob);
        await _context.SaveChangesAsync();
        return new Response<GetJobGrades>(job);
        
}
   public async  Task<Response<GetJobGrades>> UpdateJobGrades(GetJobGrades job)
        {

        var find = await _context.JobGrades.FindAsync(job.JobGradesId);

        if(find == null) return null;
         find.GradeLevel=job.GradeLevel;
            find.LowestSai=job.LowestSai;
            find.HighestSai=job.HighestSai;
        await _context.SaveChangesAsync();
        return new Response<GetJobGrades>(job);
            }
        
         public async Task<Response<string>> DeleteJobGrades(int id)
        {      
        var find = await _context.JobGrades.FindAsync(id);
        _context.JobGrades.Remove(find);
        await _context.SaveChangesAsync();
           if(find != null)  return new Response<string>("JobGrades deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "JobGrades not found");
        }
}
