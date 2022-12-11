
using System.Net;
namespace Infrastructure.Services;
using Domain.Entities;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class DepartmentService
{
    private DataContext _context;

    public DepartmentService(DataContext context)
    {
        _context = context;
    }
    

    public async Task<Response<List<GetDepartment>>> GetDepartment()
    {
        var list = await _context.Departments.Select(d => new GetDepartment()
        {
                             DepartmentId = d.DepartmentId,
                           DepartmentName = d.DepartmentName,
                              LocationId=d.LocationId
        }).ToListAsync();

        return new Response<List<GetDepartment>>(list);
            
    }
    
    public async Task<Response<AddDepartment>> InsertDepartment(AddDepartment department)
    {
        var newDepartment = new Department()
        {
                                  DepartmentName = department.DepartmentName,
                                LocationId = department.LocationId
        };
         _context.Departments.Add(newDepartment);

         await _context.SaveChangesAsync();

         return new Response<AddDepartment>(department);
    }
        public async Task<Response<AddDepartment>> UpdateDepartment(AddDepartment department)
        {
                    var find = await _context.Departments.FindAsync(department.DepartmentId);
                                       if(find != null) return null;
                                find.DepartmentName = department.DepartmentName;
                                  find.LocationId = department.LocationId;

                              await _context.SaveChangesAsync();

                          return new Response<AddDepartment>(department);
        }
      public async Task<Response<string>> DeleteDepartment(int id)
        {      
                         var find = await _context.Departments.FindAsync(id);
                         _context.Departments.Remove(find);
                        await _context.SaveChangesAsync();
        if(find.DepartmentId > 0 )  return new Response<string>("Department deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "Department not found");
        }
}