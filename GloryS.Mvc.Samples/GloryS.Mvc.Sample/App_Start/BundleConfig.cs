using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using GloryS.Common.Kendo.Metadata;
using GloryS.Common.Web.Metadata;

namespace GloryS.Mvc.Sample
{
    public class BundleConfig
    {
        public const string DefaultScripts = "~/Scripts/Default";

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var kendoResources = new KendoResources();

            var commonResources = new CommonResources();

            string[] defaultScripts = commonResources.GetCommonScriptResources().Resources
                .Concat(kendoResources.GetScriptsResources().Resources)
                .Concat(kendoResources.GetResourceScriptsResources(new Culture[]{Culture.Default, Culture.DE, Culture.ES, Culture.RU}).Resources)
                .Concat(kendoResources.GetCommonScriptResources().Resources)
                .ToArray();
            bundles.Add(new ScriptBundle(DefaultScripts).Include(defaultScripts));

            bundles.Add(new ScriptBundle("~/Content/BaseStyles.css")
                .Include("~/Content/Site.css"));
        }
    }
}