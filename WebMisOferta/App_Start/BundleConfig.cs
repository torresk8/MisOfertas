using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebMisOferta
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
                bundles.Add(new StyleBundle("~/bundles/css")
                .Include(
                "~/Content/assets/css/normalize.css",
                "~/Content/assets/css/font-awesome.min.css",
                "~/Content/assets/css/fontello.css",
                "~/Content/assets/fonts/icon-7-stroke/css/pe-icon-7-stroke.css",
                "~/Content/assets/fonts/icon-7-stroke/css/pe-icon-7-stroke.css",
                "~/Content/assets/fonts/icon-7-stroke/css/pe-icon-7-stroke.css",
                "~/Content/assets/fonts/icon-7-stroke/css/helper.css",
                "~/Content/assets/css/animate.css",
                "~/Content/assets/css/bootstrap-select.min.css",
                "~/Content/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/css/icheck.min_all.css",
                "~/Content/assets/css/price-range.css",
                "~/Content/assets/css/owl.carousel.css",
                "~/Content/assets/css/owl.theme.css",
                "~/Content/assets/css/owl.transitions.css",
                "~/Content/assets/css/style.css",
                "~/Content/assets/css/responsive.css"));

            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                "~/Content/assets/js/modernizr-2.6.2.min.js",
                "~/Content/assets/js/jquery-1.10.2.min.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Content/assets/js/bootstrap-select.min.js",
                "~/Content/assets/js/bootstrap-hover-dropdown.js",
                "~/Content/assets/js/easypiechart.min.js",
                "~/Content/assets/js/jquery.easypiechart.min.js",
                "~/Content/assets/js/owl.carousel.min.js",
                "~/Content/assets/js/wow.js",
                "~/Content/assets/js/icheck.min.js",
                "~/Content/assets/js/price-range.js",
                "~/Content/assets/js/main.js"));
        }
    }
}