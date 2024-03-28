using Toolbelt.Blazor;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IHttpInterceptorService : ITransient
    {
        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

        void RegisterEvent();
    }
}