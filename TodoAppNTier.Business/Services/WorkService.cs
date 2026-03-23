using AutoMapper;
using FluentValidation;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Common.ResponseObjects;
using TodoAppNTier.DataAccess.UnitofWork;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.Business.Extensions;


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

        public async Task<IResponse> Create(WorkCreateDto dto)
        {
            var validationResult = _createValidator.Validate(dto);
            
            if (validationResult.IsValid) // Eğer doğrulama başarılı ise
            {
                await _uow.GetRepository<Work>().Create(_mapper.Map<Work>(dto)); // DTO'yu Entity'e çevirip veritabanına ekliyoruz
                await _uow.SaveChanges();
                return new Response(ResponseType.Success); 
            }

            var errors = validationResult.GetErrorMessages();
            return new Response(ResponseType.ValidationError, errors);
        }

        public async Task<IResponse<List<WorkListDto>>> GetAll()
        {
            var data = _mapper.Map<List<WorkListDto>>(await _uow.GetRepository<Work>().GetAll());
            return new Response<List<WorkListDto>>(ResponseType.Success, data);
        }

        public async Task<IResponse<IDto>> GetById<IDto>(int id)
        {
            var data = _mapper.Map<IDto>(await _uow.GetRepository<Work>().GetByFilter(x => x.Id == id));
            if (data == null)
            {
                return new Response<IDto>(ResponseType.NotFound, "İlgili veri bulunamadı.");
            }
            return new Response<IDto>(ResponseType.Success, data);
        }

        public async Task<IResponse> Remove(int id)
        {
            var removedEntity = await _uow.GetRepository<Work>().GetByFilter(x => x.Id == id);
            if (removedEntity != null)
            {
                _uow.GetRepository<Work>().Remove(removedEntity);
                await _uow.SaveChanges();
                return new Response(ResponseType.Success);
            }
            return new Response(ResponseType.NotFound, "İlgili kayıt bulunamadı.");
        }

        public async Task<IResponse> Update(WorkUpdateDto dto)
        {
            var result = _updateValidator.Validate(dto); // DTO'nun doğrulamasını yapıyoruz
            
            if (result.IsValid) // Eğer doğrulama başarılı ise
            {
                var unchanged = await _uow.GetRepository<Work>().GetById(dto.Id);

                if (unchanged != null) 
                {
                    var entity = _mapper.Map<Work>(dto);
                    _uow.GetRepository<Work>().Update(entity, unchanged);
                    await _uow.SaveChanges();
                    return new Response(ResponseType.Success);
                }
                return new Response(ResponseType.NotFound, "Güncellenecek kayıt bulunamadı.");
            }

            var errors = result.GetErrorMessages();
            return new Response(ResponseType.ValidationError, errors);
        }
    }
}
