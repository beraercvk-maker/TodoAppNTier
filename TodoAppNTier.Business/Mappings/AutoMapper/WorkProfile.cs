using AutoMapper;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;

namespace TodoAppNTier.Business.Mappings.AutoMapper
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            // 1. Listeleme işlemi için eşleştirme
            CreateMap<Work, WorkListDto>().ReverseMap();

            // 2. Ekleme işlemi için eşleştirme
            CreateMap<Work, WorkCreateDto>().ReverseMap();

            // 3. Güncelleme işlemi için eşleştirme
            CreateMap<Work, WorkUpdateDto>().ReverseMap();

            CreateMap< WorkListDto,WorkUpdateDto>().ReverseMap();
            
        }
    }
}