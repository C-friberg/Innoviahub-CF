using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models;

namespace api.Interfaces
{
    public interface IResourceRepository
    {
        Task<List<Resource>> GetAllAsync();  
        Task<Resource?> GetResourceAsync(int id);   
        Task<List<Resource>> GetByTypeAsync(ResourceType type);
    }
}