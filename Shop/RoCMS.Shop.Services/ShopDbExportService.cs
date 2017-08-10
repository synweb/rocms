//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using RoCMS.Shop.Contract.Models;
//using RoCMS.Shop.Contract.Services;
//using RoCMS.Shop.Data;
//using RoCMS.Web.Contract.Services;

//namespace RoCMS.Shop.Services
//{
//    public class ShopDbExportService: ShopContextService, IShopDbExportService
//    {
//        private readonly ILogService _logService;

//        public ShopDbExportService(ILogService logService)
//        {
//            _logService = logService;
//        }

//        public DbExportTask StartDbExportTask()
//        {
//            ShopDbExportTask newTask = new ShopDbExportTask()
//            {
//                StartDate = DateTime.UtcNow,
//                Status = DbExportStatus.Processing.ToString(),
//            };
//            int taskId;
//            using (var db = Context)
//            {
//                var dataTask = db.ShopDbExportTask.Add(newTask);
//                db.SaveChanges();
//                taskId = dataTask.TaskId;
//            }
//            Task t = new Task(() => SyncShopData(taskId));
//            t.Start();

//            return _mapper.Map<DbExportTask>(newTask);
//        }

//        private void SyncShopData(int taskId)
//        {
//            try
//            {
//                DbExporter.SyncShopDb();
//                using (var db = Context)
//                {
//                    var dataTask = db.ShopDbExportTask.Find(taskId);
//                    dataTask.EndDate = DateTime.UtcNow;
//                    dataTask.Status = DbExportStatus.Success.ToString();
//                    db.SaveChanges();
//                }
//            }
//            catch (Exception e)
//            {
//                using (var db = Context)
//                {
//                    var dataTask = db.ShopDbExportTask.Find(taskId);
//                    dataTask.EndDate = DateTime.UtcNow;
//                    dataTask.Status = DbExportStatus.Error.ToString();
//                    var errCode = _logService.LogError(e);
//                    dataTask.ErrorCode = errCode.ToString();
//                    db.SaveChanges();
//                }
//            }

//        }

//        public List<DbExportTask> GetDbExportTasks(int count)
//        {
//            using (var db = Context)
//            {
//                var tasks = db.ShopDbExportTask.Take(count);
//                return _mapper.Map<List<DbExportTask>>(tasks);
//            }
//        }

//        protected override int CacheExpirationInMinutes
//        {
//            get { return 60; }
//        }



//    }
//}
