using AutoMapper;
using FluentValidation;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Business.Mappings.AutoMapper;
using TodoAppNTier.DataAccess.UnitofWork;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete;

namespace TodoAppNTier.Services.WorkService
{
    public class WorkService : IWorkService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;

        private readonly IValidator<WorkCreateDto> _createValidator;
        private readonly IValidator<WorkUpdateDto> _updateValidator;

        public WorkService(IUow uow, IMapper mapper, IValidator<WorkCreateDto> createValidator, IValidator<WorkUpdateDto> updateValidator)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;

        }

        public async Task Create(WorkCreateDto Dto)
        {
            var validationResult = _createValidator.Validate(Dto);
            
            if (validationResult.IsValid)
            {
                  await _uow.GetRepository<Work>().Create(_mapper.Map<Work>(Dto));
            await _uow.SaveChanges();
            }
            
        }

        public async Task<List<WorkListDto>> GetAll()
        {
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
            var result = _updateValidator.Validate(Dto);
            
            if (result.IsValid) 
            {
                // 1. Veritabanından "orijinal" (unchanged) veriyi bulup getiriyoruz.
                // Artık bu görevi Service üstlendi, Repository veritabanını boş yere yormayacak!
                var unchanged = await _uow.GetRepository<Work>().GetById(Dto.Id);

                if (unchanged != null) // Ufak bir güvenlik: Ya böyle bir görev yoksa?
                {
                    // 2. Formdan gelen güncel DTO'yu, AutoMapper ile "yeni" veriye (entity) çeviriyoruz.
                    var entity = _mapper.Map<Work>(Dto);

                    // 3. İki paketi birden Repository'deki o iki parametreli, performanslı metodumuza gönderiyoruz!
                    _uow.GetRepository<Work>().Update(entity, unchanged);
                    await _uow.SaveChanges();
                }
            }
        }
    }
}
