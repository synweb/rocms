using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Options.Contract.Models;

namespace RoCMS.Options.Contract.Services
{
    public interface IOptionService
    {
        IList<OptionKey> GetOptions ();
        OptionKey GetOption(int id);
        void RemoveOption(int id);
        int CreateOption(OptionKey option);
        void UpdateOption(OptionKey option);
        IList<OptionKey> GetOptionsForValues(IList<int> optionValueIds);
    }
}
