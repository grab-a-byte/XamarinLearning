using MvvmCross;
using MvvmCross.ViewModels;
using TipCalcMvvmCrossForms.Services;
using TipCalcMvvmCrossForms.ViewModels;

namespace TipCalcMvvmCrossForms
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ICalculationService, CalculationService>();
            RegisterAppStart<TipViewModel>();
        }
    }
}
