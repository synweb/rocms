using System.Web.Mvc;

namespace RoCMS.Base.ForWeb.Models
{

    public abstract class PageBase<TModel> : PageBase
    {
        ViewDataDictionary<TModel> _viewData;

        public new AjaxHelper<TModel> Ajax
        {
            get;
            set;
        }

        public new HtmlHelper<TModel> Html
        {
            get;
            set;
        }

        public new TModel Model
        {
            get
            {
                return ViewData.Model;
            }
        }

        public new ViewDataDictionary<TModel> ViewData
        {
            get
            {
                if(_viewData == null)
                {
                    SetViewData(new ViewDataDictionary<TModel>());
                }
                return _viewData;
            }
            set
            {
                SetViewData(value);
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Ajax = new AjaxHelper<TModel>(ViewContext, this);
            Html = new HtmlHelper<TModel>(ViewContext, this);
        }

        protected override void SetViewData(ViewDataDictionary viewData)
        {
            _viewData = new ViewDataDictionary<TModel>(viewData);
            base.SetViewData(_viewData);
        }
    }
}