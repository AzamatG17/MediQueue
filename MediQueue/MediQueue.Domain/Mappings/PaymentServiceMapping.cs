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
            CreateMap<PaymentService, PaymentServiceDto>()
                .ForMember(dest => dest.ServiceName, cfg => cfg.MapFrom(e => e.Service.Name))
                .ForMember(dest => dest.AccountName, cfg => cfg.MapFrom(e => $"{e.Account.LastName ?? ""} {e.Account.FirstName ?? ""} {e.Account.SurName ?? ""}"));
            CreateMap<PaymentServiceForCreateDto, PaymentService>();
            CreateMap<PaymentServiceForUpdateDto, PaymentService>();
        }
    }
}
