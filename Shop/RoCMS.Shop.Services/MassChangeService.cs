using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Services;
using MassPriceChangeTask = RoCMS.Shop.Contract.Models.MassPriceChangeTask;

namespace RoCMS.Shop.Services
{
    public class MassChangeService : BaseShopService, IMassChangeService
    {
        private IShopService _shopService;
        private ILogService _logService;
        private readonly MassPriceChangeTaskGateway _massPriceChangeTaskGateway = new MassPriceChangeTaskGateway();

        public MassChangeService(IShopService shopService, ILogService logService)
        {
            _shopService = shopService;
            _logService = logService;
        }

        public MassPriceChangeTask StartChangePriceTask(MassPriceChange change)
        {
            if (change.Increase == false && change.Value >= 100)
                throw new Exception("Нельзя уменьшить стоимость более чем на 99 процентов");
            if (change.Value <= 0)
            {
                throw new Exception("Процент должен быть положительным числом больше 0");
            }

            int taskId;
            var task = new MassPriceChangeTask()
            {
                Comment = change.Comment,
                CreationDate = DateTime.UtcNow,
                Description = change.GetDescription(),
                State = MassPriceChangeTaskState.Processed
            };

            var rec = Mapper.Map<Data.Models.MassPriceChangeTask>(task);
            taskId = task.TaskId = _massPriceChangeTaskGateway.Insert(rec);

            Task t = new Task(() => ChangePrice(change, taskId));
            t.Start();

            return task;
        }

        private void ChangePrice(MassPriceChange change, int taskId)
        {

            using (var scope = new TransactionScope())
            {
                var filter = change.Filter;
                {
                    try
                    {
                        IList<GoodsItem> goods;
                        int total;
                        FilterCollections filterCollections;
                        goods = _shopService.GetGoodsSet(filter, 1, int.MaxValue, out total, out filterCollections, false);
                        foreach (var goodsItem in goods)
                        {
                            decimal dt = goodsItem.Price * change.Value / 100m;
                            decimal val = change.Increase ? goodsItem.Price + dt : goodsItem.Price - dt;
                            goodsItem.Price = Math.Round(val, 0, MidpointRounding.AwayFromZero);

                            foreach (var pack in goodsItem.Packs)
                            {
                                if (!pack.Price.HasValue)
                                    continue;
                                dt = pack.Price.Value * change.Value / 100m;
                                val = change.Increase ? pack.Price.Value + dt : pack.Price.Value - dt;
                                pack.Price = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                            }
                            _shopService.UpdateGoods(goodsItem);
                        }
                        var task = _massPriceChangeTaskGateway.SelectOne(taskId);
                        task.State = MassPriceChangeTaskState.Completed.ToString();
                        _massPriceChangeTaskGateway.Update(task);
                        scope.Complete();
                    }
                    catch(Exception e)
                    {
                        var task = _massPriceChangeTaskGateway.SelectOne(taskId);
                        _logService.LogError(e);
                        task.State = MassPriceChangeTaskState.Error.ToString();
                    }
                }
            }

        }


        public IEnumerable<MassPriceChangeTask> GetChangePriceTasks()
        {
            var dataRes = _massPriceChangeTaskGateway.Select();
            var res = Mapper.Map<IEnumerable<MassPriceChangeTask>>(dataRes);
            return res;
        }

        protected override int CacheExpirationInMinutes
        {
            get { return 30; }
        }
    }
}
