
using BootstrapBlazor.Components;
using BootstrapBS.Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBS.Server.Components.Layout
{
    public abstract class WebSiteModuleComponentBase : BootstrapModuleComponentBase
    {
        [Inject]
        [NotNull]
        private IOptions<WebsiteOptions>? WebsiteOption { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnLoadJSModule()
        {
            base.OnLoadJSModule();

            if (!string.IsNullOrEmpty(ModulePath))
            {
                ModulePath = $"{WebsiteOption.Value.JSModuleRootPath}{ModulePath}";
            }
        }
    }
}
