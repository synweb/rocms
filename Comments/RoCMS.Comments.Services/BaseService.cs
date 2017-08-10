using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Comments.Contract.ViewModels;
using RoCMS.Comments.Data.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Comments.Services
{
    public abstract class BaseService: BaseCacheService
    {

        protected readonly IMapperService _mapper;

        static BaseService()
        {
            Mapper.CreateMap<Comment, Contract.Models.Comment>();
            Mapper.CreateMap<Contract.Models.Comment, Comment>();

            Mapper.CreateMap<CommentTopic, Contract.Models.CommentTopic>();
            Mapper.CreateMap<Contract.Models.CommentTopic, CommentTopic>();

            Mapper.CreateMap<Contract.Models.Comment, CommentVM>()
                .ForMember(x => x.Replies, x => x.Ignore())
                .ForMember(x => x.Author, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.CommentTopic, CommentTopicVM>()
                .ForMember(x => x.CommentCount, x => x.Ignore());

            Mapper.AssertConfigurationIsValid();
        }

        protected BaseService(IMapperService mapper)
        {
            _mapper = mapper;
        }
    }
}
