using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.ViewModels;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Comment = RoCMS.Comments.Data.Models.Comment;

namespace RoCMS.Comments.Services
{
    public abstract class BaseService: BaseCacheService
    {

        protected readonly IMapperService _mapper;

        static BaseService()
        {
            Mapper.CreateMap<Comment, Contract.Models.Comment>();
            Mapper.CreateMap<Contract.Models.Comment, Comment>();

            Mapper.CreateMap<Contract.Models.Comment, CommentVM>()
                .ForMember(x => x.Replies, x => x.Ignore())
                .ForMember(x => x.Author, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.CommentTopic, CommentTopicVM>()
                .ForMember(x => x.CommentCount, x => x.Ignore());


            Mapper.CreateMap<Heart, CommentTopic>()
                .ForMember(x => x.TargetCanonicalUrl, y => y.MapFrom(z => z.CanonicalUrl))
                .ForMember(x => x.TargetTitle, y => y.MapFrom(z => z.Title));



            Mapper.AssertConfigurationIsValid();
        }

        protected BaseService(IMapperService mapper)
        {
            _mapper = mapper;
        }
    }
}
