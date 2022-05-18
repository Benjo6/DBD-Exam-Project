using ConsultationService.Entities;
using lib.DTO;

namespace ConsultationService.Util
{
    public static class ConsultationMapper
    {
        public static ConsultationDto ToDto(ConsultationEntity entity){
            var dto = new ConsultationDto(){
                Id = entity.Id.ToString(),
                DoctorId = entity.DoctorId,
                PatientId = entity.DoctorId,
                Regarding = entity.Regarding

            };
            return dto;
        }

        public static ConsultationEntity FromDto(ConsultationDto dto){
            var entity = new ConsultationEntity(){

            };
            return entity;
        }
    }
}