using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Serilog;
using Syncfusion.Blazor.Notifications;
using System;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Shared
{
    public class MainLayoutBase : LayoutComponentBase, IAsyncDisposable
    {
        protected SfToast SfToastObjRef { get; set; }
        private DotNetObjectReference<MainLayout> _objectRef;

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference _pwaModule;
        private Task<IJSObjectReference> PwaModule => _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/pwa.js").AsTask();


        protected async Task InstallClicked()
        {
            SfToastObjRef?.Hide();
            try
            {
                await _pwaModule.InvokeVoidAsync("installPWA");
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, nameof(InstallClicked));
            }
        }

        [JSInvokable]
        public Task ShowInstallPrompt()
        {
            return SfToastObjRef.Show();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _pwaModule = await PwaModule;
                _objectRef = DotNetObjectReference.Create((MainLayout)this);
                await _pwaModule.InvokeVoidAsync("setLayoutRef", _objectRef);
            }
        }

        public async ValueTask DisposeAsync()
        {

            _objectRef?.Dispose();
            if(_pwaModule != null)
            {
                await _pwaModule.DisposeAsync();
            }
        }
    }
}
