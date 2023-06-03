using System.Web;
using System.Web.Optimization;

namespace TravellersChoice
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/JavaScripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.barrating.min.js",
                      "~/Scripts/jquery.dotdotdot.js",
                      "~/Scripts/main.js"
                     ));

  

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-stars.css",
                      "~/Content/site.css"
                      ));


           
            bundles.Add(new ScriptBundle("~/Scripts/jquery-ui-1.11.4/jqueryuiscript").Include(
                       "~/Scripts/jquery-ui-1.11.4/jquery-ui.js"));

            bundles.Add(new StyleBundle("~/Scripts/jquery-ui-1.11.4/jqueryuicss").Include(
                        "~/Scripts/jquery-ui-1.11.4/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/Scripts/slick/slickscript").Include(
                      "~/Scripts/slick/slick.js"));

            bundles.Add(new StyleBundle("~/Scripts/slick/slickcss").Include(
                      "~/Scripts/slick/slick.css",
                      "~/Scripts/slick/slick-theme.css"));

        }
    }
}
