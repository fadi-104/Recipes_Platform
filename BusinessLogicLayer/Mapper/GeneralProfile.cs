using AutoMapper;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;


namespace BusinessLogicLayer.Mapper
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<UserRequest, UserApp>().ReverseMap();
            CreateMap<UserApp, UserResponse>().ReverseMap();

            CreateMap<CategoryRequest, Category>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();

            CreateMap<RecipecRequest, Recipec>().ReverseMap();
            CreateMap<Recipec, RecipecResponse>()
                .ForMember(x => x.CategoryName, x => x.MapFrom(x => x.Category.Name))
                .ForMember(x => x.ChefName, x => x.MapFrom(x => $"{x.Chef.FirstName} {x.Chef.LastName}"))
                .ReverseMap();

            CreateMap<RatingRequest, Rating>().ReverseMap();
            CreateMap<Rating, RateResponse>().ReverseMap();

            CreateMap<ImageRequest, Image>().ReverseMap();
            CreateMap<Image, ImageResponse>().ReverseMap();

            CreateMap<MessageRequest, Message>().ReverseMap();
            CreateMap<Message, MessageResponse>()
                .ForMember(x => x.SenderName, x => x.MapFrom(x => $"{x.Sender.FirstName} {x.Sender.LastName}"))
                .ForMember(x => x.ReceiverName, x => x.MapFrom(x => $"{x.Receiver.FirstName} {x.Receiver.LastName}"))
                .ReverseMap();

            CreateMap<FavouriteRequest,Favourite>().ReverseMap();

        }
    }
}
