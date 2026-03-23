using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Common.ResponseObjects;

namespace TodoAppNTier.Business.Interfaces
{
    public interface IWorkService
    {
        // 1. Veri taşıyan kargo kutularımız (IResponse<T>)
        Task<IResponse<List<WorkListDto>>> GetAll();
        
        Task<IResponse<IDto>> GetById<IDto>(int id);

        
        Task<IResponse> Create(WorkCreateDto dto);
        
        Task<IResponse> Remove(int id);
        
        Task<IResponse> Update(WorkUpdateDto dto);
    }
}