using AutoMapper;
using RoCMS.Options.Data;

namespace RoCMS.Options.Services
{
    public abstract class OptionsContextService
    {

        protected internal OptionsContainer Context
        {
            get { return new OptionsContainer(); }
        }

        static OptionsContextService()
        {
            ConfigureMapper();
        }
        
        private static void ConfigureMapper()
        {


            Mapper.CreateMap<Options.Data.OptionValue, Options.Contract.Models.OptionValue>();
            Mapper.CreateMap<Options.Contract.Models.OptionValue, Options.Data.OptionValue>()
                .ForMember(x => x.OptionKey, y => y.Ignore());

            Mapper.CreateMap<Options.Data.OptionKey, Options.Contract.Models.OptionKey>();
            Mapper.CreateMap<Options.Contract.Models.OptionKey, Options.Data.OptionKey>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
