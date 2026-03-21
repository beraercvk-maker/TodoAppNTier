using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;

namespace TodoAppNTier.Business.Interfaces
{
    public interface IWorkService
    {
      Task <List<WorkListDto>> GetAll();


      Task Create (WorkCreateDto Dto);

      Task <WorkListDto> GetById(int id);

      Task Remove(object id);


      Task Update(WorkUpdateDto Dto);
     
    }

    
}