using ConsultationService.Entities;
using lib.DTO;

namespace ConsultationService.Util
{
    public static class ConsultationMetadataMapper
    {
        public static ConsultationMetadataDto ToDto(ConsultationMetadataEntity entity){
            if (entity == null)
                return null;

            
            var dto = new ConsultationMetadataDto() {
                DayOfConsultationsAdded = entity.DayOfConsultationsAdded,
                CreatedCount = entity.ConsultationsCount,
                CreatedUtc = entity.CreatedUtc                
            };
            return dto;
        }

        public static ConsultationMetadataEntity FromDto(ConsultationMetadataDto dto){
            var entity = new ConsultationMetadataEntity(){
                CreatedUtc = dto.CreatedUtc,
                DayOfConsultationsAdded = dto.DayOfConsultationsAdded,
                ConsultationsCount = dto.CreatedCount
            };
            return entity;
        }
    }
}