using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;

namespace TodoAppNTier.Business.Interfaces
{
    public interface IWorkService
    {
      Task <List<WorkListDto>> GetAll();


      Task Create (WorkCreateDto Dto);

      Task <IDto> GetById<IDto>(int id);

      Task Remove(int id);


      Task Update(WorkUpdateDto Dto);
     
    }

    
}