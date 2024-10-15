using AutoMapper;
using MediQueue.Domain.DTOs.PaymentLekarstvo;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class PaymentLekarstvoMapping : Profile
    {
        public PaymentLekarstvoMapping()
        {
            CreateMap<PaymentLekarstvoDto, PaymentLekarstvo>();
            CreateMap<PaymentLekarstvo, PaymentLekarstvoDto>()
                .ForCtorParam(nameof(PaymentLekarstvoDto.LekarstvoName), cfg => cfg.MapFrom(e => e.Lekarstvo.Name))
                .ForCtorParam(nameof(PaymentLekarstvoDto.AccountName), cfg => cfg.MapFrom(e => $"{e.Account.LastName ?? ""} {e.Account.FirstName ?? ""} {e.Account.SurName ?? ""}"));
            CreateMap<PaymentLekarstvoForCreateDto, PaymentLekarstvo>();
            CreateMap<PaymentLekarstvoForUpdateDto, PaymentLekarstvo>();
        }
    }
}
