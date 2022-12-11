using System.Net;
using Dapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;



namespace Infrastructure.Services;

public class RegionService
{
    private readonly DataContext _context;

    public RegionService(DataContext context)
    {
        _context = context;
      
    }

     public async Task<Response<List<GetRegion>>> GetRegion()
        {
            
       var list = await _context.Regions.Select(r => new GetRegion()
        {
            RegionId=r.RegionId,
            RegionName  = r.RegionName
            

        }).ToListAsync();

        return new Response<List<GetRegion>>(list);
            
        }

        public async Task <Response<GetRegion>> InsertRegion(GetRegion region)
        {            
        var newRegion= new Region()
        {
            
            RegionName = region.RegionName,

        };
        _context.Regions.Add(newRegion);
        await _context.SaveChangesAsync();
        return new Response<GetRegion>(region);

        }
        

   public async  Task<Response<GetRegion>>UpdateRegion(GetRegion region)
        {
        {
            var find = await _context.Regions.FindAsync(region.RegionId);

            if(find == null)
             return null;
            find.RegionName = region.RegionName;
     
            await _context.SaveChangesAsync();
             return new Response<GetRegion>(region);
        }
        }
         public async Task<Response<string>> DeleteRegion(int id)
        {
            
        var find = await _context.Regions.FindAsync(id);
        _context.Regions.Remove(find);
        await _context.SaveChangesAsync();
           if(find != null)  return new Response<string>("Region deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "Region not found");
            
        }
}