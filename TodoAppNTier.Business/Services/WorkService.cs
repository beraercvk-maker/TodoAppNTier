using AutoMapper;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.DataAccess.UnitofWork;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete; // Work sınıfı için bunu eklemen gerekebilir

namespace TodoAppNTier.Services.WorkService
{
    public class WorkService : IWorkService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        
        // 1. Yapıcı metot düzeltildi
        public WorkService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task Create(WorkCreateDto Dto)
        {
           await _uow .GetRepository<Work>().Create(_mapper.Map<Work>(Dto));
            await _uow.SaveChanges();
        }

        public async Task<List<WorkListDto>> GetAll()
        {
            // var list = await _uow.GetRepository<Work>().GetAll();
            // var workList = new List<WorkListDto>();

            // // 2. Mantık hatası düzeltildi (!= kullanıldı)
            // if (list != null && list.Count > 0)
            // {
            //     foreach (var work in list) // Değişken adı küçük harfle yazıldı ki sınıfla karışmasın
            //     {
            //         // 3. Syntax hatası düzeltildi. Doğrudan listeye ekleme yapılıyor.
            //         workList.Add(new WorkListDto
            //         {
            //             Id = work.Id,
            //             Definition = work.Definition,
            //             IsCompleted = work.IsCompleted
            //         });
            //     }
            // }
            
            // // 4. Return düzeltildi. Doldurulan liste geri gönderiliyor.
            // return workList; 

            return _mapper.Map<List<WorkListDto>>(await _uow.GetRepository<Work>().GetAll());
        }

        public async Task<IDto> GetById<IDto>(int id)
{
   return _mapper.Map<IDto>(await _uow.GetRepository<Work>().GetByFilter(x => x.Id == id));
    

    
}


      

        public async Task Remove(int id)
        {
           
            
           _uow.GetRepository<Work>().Remove(id);
            await _uow.SaveChanges();
           
        }

        public async Task Update(WorkUpdateDto Dto)
        {
            _uow.GetRepository<Work>().Update(_mapper.Map<Work>(Dto));
            await _uow.SaveChanges();
        }
    }
}
