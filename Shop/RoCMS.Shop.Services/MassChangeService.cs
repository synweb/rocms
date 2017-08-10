//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Transactions;
//using RoCMS.Base;
//using RoCMS.Shop.Contract.Models;
//using RoCMS.Shop.Contract.Services;
//using RoCMS.Shop.Data;
//using Category = RoCMS.Shop.Data.Category;
//using MassPriceChangeTask = RoCMS.Shop.Contract.Models.MassPriceChangeTask;

//namespace RoCMS.Shop.Services
//{
//    public class MassChangeService : ShopContextService, IMassChangeService
//    {
//        private IShopService _shopService;

//        public MassChangeService(IShopService shopService)
//        {
//            _shopService = shopService;
//        }

//        public MassPriceChangeTask StartChangePriceTask(MassPriceChange change)
//        {
//            if (change.Increase == false && change.Value >= 100)
//                throw new Exception("Нельзя уменьшить стоимость более чем на 99 процентов");
//            if (change.Value <= 0)
//            {
//                throw new Exception("Процент должен быть положительным числом больше 0");
//            }

//            int taskId;
//            var task = new MassPriceChangeTask()
//            {
//                Comment = change.Comment,
//                CreationDate = DateTime.UtcNow,
//                Description = change.GetDescription(),
//                State = MassPriceChangeTaskState.Processed
//            };
//            using (var db = Context)
//            {

//                var dataTask = db.MassPriceChangeTask.Add(_mapper.Map<Data.MassPriceChangeTask>(task));
//                db.SaveChanges();
//                taskId = task.TaskId = dataTask.TaskId;
//            }

//            Task t = new Task(() => ChangePrice(change, taskId));
//            t.Start();

//            return task;
        
//        }

//        private void ChangePrice(MassPriceChange change, int taskId)
//        {

//            using (var scope = new TransactionScope())
//            {
//                var filter = change.Filter;
//                using (var db = Context)
//                {
//                    try
//                    {
//                        IQueryable<Goods> goods;
//                        if (filter.CategoryIds != null && filter.CategoryIds.Any())
//                        {
//                            IEnumerable<Goods> resGoods = new List<Goods>();
//                            foreach (var categoryId in filter.CategoryIds)
//                            {
//                                var category = db.CategorySet.Find(categoryId);

//                                List<Goods> tempgoods = category.Goods.Select(x => x.GoodsSet).ToList();
//                                FillGoodsFromChildCategories(category, tempgoods);
//                                resGoods = resGoods.Concat(tempgoods);
//                            }

//                            goods = resGoods.AsQueryable();

//                        }
//                        else
//                        {
//                            goods = db.GoodsSet;
//                        }

//                        if (filter.ManufacturerIds != null && filter.ManufacturerIds.Any())
//                        {
//                            IEnumerable<Goods> resGoods = new List<Goods>();

//                            foreach (var manufacturerId in filter.ManufacturerIds)
//                            {
//                                resGoods = resGoods.Concat(goods.Where(g => g.ManufacturerId == manufacturerId || g.SupplierId == manufacturerId));
//                            }

//                            goods = resGoods.AsQueryable();
//                        }

//                        goods = goods.Distinct().Where(x => !x.Deleted);

//                        foreach (var goodsItem in goods)
//                        {
//                            if (!goodsItem.Price.HasValue) continue;

//                            decimal dt = goodsItem.Price.Value*change.Value/100m;
//                            decimal val = change.Increase ? goodsItem.Price.Value + dt : goodsItem.Price.Value - dt;
//                            goodsItem.Price = Math.Round(val, 0, MidpointRounding.AwayFromZero);

//                            foreach (var pack in goodsItem.Packs)
//                            {
//                                if (!pack.Price.HasValue) continue;
//                                dt = pack.Price.Value*change.Value/100m;
//                                val = change.Increase ? pack.Price.Value + dt : pack.Price.Value - dt;
//                                pack.Price = Math.Round(val, 0, MidpointRounding.AwayFromZero);
//                            }

//                        }



//                        var task = db.MassPriceChangeTask.Find(taskId);
//                        task.State = MassPriceChangeTaskState.Completed.ToString();


//                        db.SaveChanges();
//                        scope.Complete();
//                    }
//                    catch
//                    {
//                        var task = db.MassPriceChangeTask.Find(taskId);
//                        task.State = MassPriceChangeTaskState.Completed.ToString();
//                    }
//                }
//            }

//        }

//        private void FillGoodsFromChildCategories(Category childCat, List<Goods> goods)
//        {
//            foreach (var cat in childCat.ChildrenCategories)
//            {
//                if (cat.Hidden) continue;
//                cat.Goods.Each(x =>
//                {
//                    goods.Add(x.GoodsSet);
//                });


//                FillGoodsFromChildCategories(cat, goods);
//            }
//        }

//        public IEnumerable<MassPriceChangeTask> GetChangePriceTasks()
//        {
//            using (var db = Context)
//            {
//                return _mapper.Map<IEnumerable<MassPriceChangeTask>>(db.MassPriceChangeTask.OrderByDescending(x => x.CreationDate));
//            }
//        }

//        protected override int CacheExpirationInMinutes
//        {
//            get { return 30; }
//        }
//    }
//}
