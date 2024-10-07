using AutoMapper;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class PaymentServiceMapping : Profile
    {
        public PaymentServiceMapping()
        {
            CreateMap<PaymentServiceDto, PaymentService>();
            CreateMap<PaymentService, PaymentServiceDto>();
            CreateMap<PaymentServiceForCreateDto, PaymentService>();
            CreateMap<PaymentServiceForUpdateDto, PaymentService>();
        }
    }
}
