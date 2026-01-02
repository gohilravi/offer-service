using AutoMapper;
using OfferService.Domain.Entities;
using OfferService.Application.DTOs;
using OfferService.Application.Events;

namespace OfferService.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Seller mappings
        CreateMap<Seller, SellerDto>();
        
        // Offer mappings
        CreateMap<CreateOfferDto, Offer>()
            .ForMember(dest => dest.OfferId, opt => opt.Ignore()) // Let database generate ID
            .ForMember(dest => dest.SellerNetworkId, opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.SellerName, opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.Status, opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.NoSQLIndexId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore()); // Set in service
            
        CreateMap<Offer, OfferDto>();
        
        CreateMap<UpdateOfferDto, Offer>()
            .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Event mappings
        CreateMap<Offer, OfferCreated>();
        CreateMap<Offer, OfferAssigned>();
        CreateMap<Offer, OfferUpdated>();
        CreateMap<Offer, OfferCanceled>();
    }
}