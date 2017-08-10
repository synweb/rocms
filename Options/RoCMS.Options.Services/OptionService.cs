using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Options.Contract.Models;
using RoCMS.Options.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Options.Services
{
    public class OptionService : OptionsContextService, IOptionService
    {
        private IMapperService _mapperService;
        public OptionService(IMapperService mapperService)
        {
            _mapperService = mapperService;
        }

        public IList<OptionKey> GetOptions()
        {
            using (var db = Context)
            {
                var result = _mapperService.Map<IList<OptionKey>>(db.OptionKey);
                return result;
            }
        }

        public OptionKey GetOption(int id)
        {
            using (var db = Context)
            {
                var data = db.OptionKey.Find(id);
                if (data != null)
                {
                    var result = _mapperService.Map<OptionKey>(data);
                    return result;
                }
                return null;
            }
        }

        public void RemoveOption(int id)
        {
            using (var db = Context)
            {
                var data = db.OptionKey.Find(id);
                if (data != null)
                {
                    db.OptionKey.Remove(data);
                    db.SaveChanges();
                }
            }
        }

        public int CreateOption(OptionKey option)
        {
            using (var db = Context)
            {
                var dataOption = _mapperService.Map<Data.OptionKey>(option);
                dataOption = db.OptionKey.Add(dataOption);
                dataOption.CreationDate = DateTime.UtcNow;
                foreach (var val in dataOption.OptionValues)
                {
                    val.CreationDate = DateTime.UtcNow;
                    db.OptionValue.Add(val);
                }
                db.SaveChanges();
                return dataOption.Id;
            }
        }

        public void UpdateOption(OptionKey option)
        {
            using (var db = Context)
            {
                var dataOption = _mapperService.Map<Data.OptionKey>(option);
                dataOption = db.OptionKey.Attach(dataOption);
                db.Entry(dataOption).State = EntityState.Modified;

                foreach (var val in dataOption.OptionValues)
                {
                    val.OptionKeyId = dataOption.Id;
                }

                //D
                var optValues = db.OptionValue.Where(x => x.OptionKeyId == option.Id);
                foreach (var optValue in optValues)
                {
                    if (!option.OptionValues.Any(x => x.Id == optValue.Id))
                    {
                        db.OptionValue.Remove(optValue);
                    }
                }

                foreach (var optValue in option.OptionValues)
                {

                    if (optValue.Id == 0)
                    {
                        //C
                        var dataOptValue = _mapperService.Map<Data.OptionValue>(optValue);
                        dataOptValue.CreationDate = DateTime.UtcNow;
                        db.OptionValue.Add(dataOptValue);
                    }
                    else
                    {
                        //U
                        var dataOptValue = dataOption.OptionValues.Single(x => x.Id == optValue.Id);
                        db.Entry(dataOptValue).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
        }

        public IList<OptionKey> GetOptionsForValues(IList<int> optionValueIds)
        {
            List<OptionKey> result = new List<OptionKey>();
            if (optionValueIds.Any())
            {
                
                using (var db = Context)
                {
                    IEnumerable<Data.OptionKey> keys;

                    var res = db.OptionKey
                        .Where(p => p.OptionValues.Any(c => optionValueIds.Contains(c.Id)))
                        .Select(p => new
                        {
                            Key = p,
                            Values = p.OptionValues.Where(value => optionValueIds.Contains(value.Id))
                        });

                    //keys = res.AsEnumerable().Select(a => a.Key).ToList();
                    //result.AddRange(_mapperService.Map<IList<OptionKey>>(keys));        
                    
                    //TODO: не оптимально. отрефакторить, разобраться с бредом, почему-то ToList() не вызывает выполнение запроса. Две строки выше должны были сделать то, что нужно
                    foreach (var key in res)
                    {
                        OptionKey resKey = _mapperService.Map<OptionKey>(key.Key);
                        resKey.OptionValues.Clear();
                        foreach (var val in key.Values)
                        {
                            resKey.OptionValues.Add(_mapperService.Map<OptionValue>(val));
                        }
                        result.Add(resKey);
                    }

                }
                
            }

            return result;
        }
    }
}
