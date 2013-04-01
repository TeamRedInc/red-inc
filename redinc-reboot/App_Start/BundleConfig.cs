using System.Web;
using System.Web.Optimization;

namespace redinc_reboot
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.markitup").Include(
                        "~/Libs/markitup/jquery.markitup.js",
                        "~/Libs/markitup/sets/bbcode/set.js"));

            bundles.Add(new ScriptBundle("~/bundles/edit_area").Include(
                        "~/Libs/edit_area/edit_area_full.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /*
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

             * */

            bundles.Add(new ScriptBundle("~/Content/js/js").Include(
                        "~/Scripts/bootstrap"
                        ));
            /**
             * add "~/Scripts/bootstrap.min" if necessary
             */

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css"));
            // also add "~/Content/bootstrap-responsive.css",
            // and      "~/Content/bootstrap-responsive.min.css",

            bundles.Add(new StyleBundle("~/Libs/markitup/css").Include(
                        "~/Libs/markitup/skins/simple/style.css",
                        "~/Libs/markitup/sets/bbcode/style.css"));

        }
    }
}