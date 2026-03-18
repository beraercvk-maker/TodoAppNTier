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
        
        // 1. Yapıcı metot düzeltildi
        public WorkService(IUow uow) 
        {
            _uow = uow;
        }

        public async Task Create(WorkCreateDto Dto)
        {
           await _uow .GetRepository<Work>().Create(new Work
            {
                Definition = Dto.Definition,
                IsCompleted = Dto.IsCompleted
            });
            await _uow.SaveChanges();
        }

        public async Task<List<WorkListDto>> GetAll()
        {
            var list = await _uow.GetRepository<Work>().GetAll();
            var workList = new List<WorkListDto>();

            // 2. Mantık hatası düzeltildi (!= kullanıldı)
            if (list != null && list.Count > 0)
            {
                foreach (var work in list) // Değişken adı küçük harfle yazıldı ki sınıfla karışmasın
                {
                    // 3. Syntax hatası düzeltildi. Doğrudan listeye ekleme yapılıyor.
                    workList.Add(new WorkListDto
                    {
                        Id = work.Id,
                        Definition = work.Definition,
                        IsCompleted = work.IsCompleted
                    });
                }
            }
            
            // 4. Return düzeltildi. Doldurulan liste geri gönderiliyor.
            return workList; 
        }

        public async Task<WorkListDto> GetById(object id) 
{
    var work = await _uow.GetRepository<Work>().GetById(id);
    
    // 3. Null kontrolü yapıldı. Veri yoksa program çökmesin, geriye boş/null dönsün.
    if (work == null)
    {
        return null; 
    }
    
    return new () 
    {
        Id = work.Id, 
        Definition = work.Definition,
        IsCompleted = work.IsCompleted
    }; // 2. Noktalı virgül eklendi
}

        public async Task Remove(object id)
        {
            var deletedWork = await _uow.GetRepository<Work>().GetById(id);
            
           _uow.GetRepository<Work>().Remove(deletedWork);
            await _uow.SaveChanges();
           
        }

        public async Task Update(WorkUpdateDto Dto)
        {
            _uow.GetRepository<Work>().Update(new Work
            {
                Id = Dto.Id,
                Definition = Dto.Definition,
                IsCompleted = Dto.IsCompleted
            });
            await _uow.SaveChanges();
        }
    }
}
