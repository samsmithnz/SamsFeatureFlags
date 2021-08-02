using System.Threading.Tasks;

namespace FeatureFlagsDemo.Web.Controllers
{
    public interface IFeatureFlagsServiceApiClient
    {
        Task<bool> CheckFeatureFlag(string name, string environment);
    }
}
