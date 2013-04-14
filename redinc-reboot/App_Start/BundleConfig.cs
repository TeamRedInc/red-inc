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

            bundles.Add(new ScriptBundle("~/Libs/markitup/js").Include(
                        "~/Libs/markitup/jquery.markitup.js",
                        "~/Libs/markitup/sets/bbcode/set.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Content/js/js").Include(
                        "~/Scripts/bootstrap*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css", 
                        "~/Content/bootstrap-responsive.css"));

            bundles.Add(new StyleBundle("~/Libs/markitup/css").Include(
                        "~/Libs/markitup/skins/simple/style.css",
                        "~/Libs/markitup/sets/bbcode/style.css"));

            bundles.Add(new StyleBundle("~/Libs/markitup/skins/simple/css").Include(
                        "~/Libs/markitup/skins/simple/style.css"));

            bundles.Add(new StyleBundle("~/Libs/markitup/sets/bbcode/css").Include(
                        "~/Libs/markitup/sets/bbcode/style.css"));

        }
    }
}