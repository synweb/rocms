using System.IO;
using System.Web.UI.WebControls;
using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace RoCMS
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {


            bundles.Add<ScriptBundle>("Content/base/vendor/jquery/core");

            bundles.Add<ScriptBundle>("Content/base/vendor/jquery/ui-short", bundle => bundle.AddReference("~/Content/base/vendor/jquery/core"));
            bundles.Add<ScriptBundle>("Content/base/vendor/jquery/ui", bundle =>
            {
                bundle.AddReference("~/Content/base/vendor/jquery/core");
                bundle.AddReference("~/Content/base/vendor/jquery/ui-short");
            });

            bundles.Add<ScriptBundle>("Content/base/vendor/knockout", bundle => bundle.AddReference("~/Content/base/vendor/jquery/core"));
            bundles.Add<ScriptBundle>("Content/base/ro/js", bundle => bundle.AddReference("~/Content/base/vendor/jquery/core"));

            bundles.Add<ScriptBundle>("Content/base/vendor/moment");

            bundles.Add<ScriptBundle>("Content/base/vendor/ckeditor", new[]
            {
                "ro.ckeditor.js",
                "ckeditor.js",

                "config.js",
                "styles.js",
                "lang/ru.js"
            }, bundle =>
            {
                bundle.AddReference("~/Content/base/vendor/jquery/core");

            });

            bundles.Add<ScriptBundle>("Content/admin/vendor/ace", new[]
            {

                "ace.js",
                "ro.ace.js",
                //"mode-razor.js",
                //"theme-sqlserver.js"
            });

            bundles.Add<ScriptBundle>("Content/theme", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.js"
            }, bundle =>
            {
                bundle.AddReference("~/Content/base/vendor/jquery/core");
            });

            bundles.AddPerIndividualFile<ScriptBundle>("Content/client/ro/js");

            bundles.Add<ScriptBundle>("Content/client/vendor/bootstrap-image-gallery/js");

            bundles.Add<ScriptBundle>("Content/admin/vendor/FU/short");
            bundles.Add<ScriptBundle>("Content/admin/vendor/FU/ui", bundle => bundle.AddReference("~/Content/admin/vendor/FU/short"));
            bundles.Add<ScriptBundle>("Content/admin/vendor/FU/cors");

            bundles.Add<ScriptBundle>("Content/client/vendor/flipclock2", new FileSearch
            {
                Pattern = "*.js"
            });




            bundles.Add<ScriptBundle>("Content/adminTemplate", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.js"
            });

            bundles.Add<ScriptBundle>("Content/admin/vendor", new[]
            {
                "bootstrap/js/bootstrap.min.js",
                "dateformat/dateformat.js",
                "bootstrap-switch/js/bootstrap-switch.min.js",
                "bootstrap-timepicker/js/bootstrap-timepicker.js",
                "tagsinput/jquery.tagsinput.min.js",
                "clipboard/clipboard.min.js"
            }, bundle =>
            {
                bundle.AddReference("~/Content/base/vendor/jquery/core");
                bundle.AddReference("~/Content/base/vendor/jquery/ui");
                
            });

            bundles.AddPerIndividualFile<ScriptBundle>("Content/admin/ro", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.js"
            }, bundle =>
            {
                bundle.AddReference("~/Content/base/vendor/jquery/core");
                bundle.AddReference("~/Content/base/ro/js/rocms.app.js");
            });

            bundles.Add<ScriptBundle>("Content/admin/vendor/flot", new[]
            {

                "jquery.flot.js",
                "jquery.flot.selection.js",
                "jquery.flot.resize.js",
                "jquery.flot.time.js",
                "jquery.flot.categories.js",
                "jquery.flot.pie.js",
                "curvedLines.js",
                "jquery.flot.barnumbers.js"
            });



            bundles.AddPerSubDirectory<ScriptBundle>("bin/Content/admin", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.js"
            }, bundle =>
            {

                bundle.AddReference("~/Content/admin/vendor");
                bundle.AddReference("~/Content/base/ro/js");
                bundle.AddReference("~/Content/base/vendor/knockout");
                bundle.AddReference("~/Content/admin/vendor/ace");
                bundle.AddReference("~/Content/base/vendor/ckeditor");
                bundle.AddReference("~/Content/admin/ro/wysiwyg/rocms.wysiwyg.js");
                bundle.AddReference("~/Content/admin/ro/js/admin-ajax.js");
                bundle.AddReference("~/Content/admin/ro/js/admin.dialogs.js");
                bundle.AddReference("~/Content/admin/ro/js/rocms.admin.main.js");
            });


            bundles.AddPerSubDirectory<ScriptBundle>("bin/Content/client", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.js"
            }, bundle =>
            {

                bundle.AddReference("~/Content/base/vendor/jquery/core");
                bundle.AddReference("~/Content/base/ro/js");

            });

            bundles.Add<StylesheetBundle>("Content/base/vendor/Font-Awesome");



            bundles.Add<StylesheetBundle>("Content/base/ro/css");

            bundles.Add<StylesheetBundle>("Content/adminTemplate/css");

            bundles.Add<StylesheetBundle>("Content/admin/vendor/bootstrap/css/bootstrap.css");

            bundles.Add<StylesheetBundle>("Content/login/login.css", bundle => bundle.AddReference("~/Content/admin/vendor/bootstrap/css/bootstrap.css"));

            bundles.Add<StylesheetBundle>("Content/admin/vendor", new[]
            {

                "bootstrap-switch/css/bootstrap-switch.css",
                "bootstrap-timepicker/css/bootstrap-timepicker.css",
                "tagsinput/jquery.tagsinput.css"
            }, bundle => bundle.AddReference("~/Content/admin/vendor/bootstrap/css/bootstrap.css"));

            bundles.Add<StylesheetBundle>("Content/admin/ro/css");

            bundles.Add<StylesheetBundle>("Content/admin/vendor/FU/jquery.fileupload-ui.css");

            bundles.Add<StylesheetBundle>("Content/theme/vendor/bootstrap/css");

            bundles.Add<StylesheetBundle>("Content/theme/css", bundle => bundle.AddReference("~/Content/theme/vendor/bootstrap/css"));

            bundles.Add<StylesheetBundle>("Content/client/ro/css");
            bundles.Add<StylesheetBundle>("Content/client/vendor/flipclock2", new FileSearch
            {
                Pattern = "*.css"
            });
            bundles.Add<StylesheetBundle>("Content/client/vendor/bootstrap-image-gallery/css", new[]
            {
                "blueimp-gallery.css",
                "bootstrap-image-gallery.css"
            });

            bundles.AddPerSubDirectory<StylesheetBundle>("bin/Content/client", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.css"
            });

            bundles.AddPerSubDirectory<StylesheetBundle>("bin/Content/admin", new FileSearch
            {
                SearchOption = SearchOption.AllDirectories,
                Pattern = "*.css"
            });

        }
    }
}