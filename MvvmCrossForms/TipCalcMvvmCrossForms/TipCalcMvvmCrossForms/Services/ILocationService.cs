using System.Threading.Tasks;
using TipCalcMvvmCrossForms.ViewModels;

namespace TipCalcMvvmCrossForms.Services
{
    public interface ILocationService
    {
        Task<Location> GetLocation();
    }
}