using Microsoft.AspNetCore.Components;

namespace GoogleSearchScrape.Shared
{
    public class NavMenuBase : ComponentBase
    {
        private bool _collapseNavMenu = true;

        protected string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        protected void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
