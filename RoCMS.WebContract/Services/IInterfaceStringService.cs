using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IInterfaceStringService
    {
        ICollection<InterfaceString> GetStrings();
        InterfaceString GetString(string key);
        void Upsert(InterfaceString interfaceString);
        void Delete(string key);
        void SaveMany(ICollection<InterfaceString> strings);
    }
}
