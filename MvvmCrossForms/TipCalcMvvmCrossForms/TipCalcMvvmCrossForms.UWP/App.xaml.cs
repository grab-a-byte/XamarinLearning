using TipCalcMvvmCrossForms.UI;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Forms.Platforms.Uap.Views;

namespace TipCalcMvvmCrossForms.UWP
{
    sealed partial class App

    {
        public App()
        {
            InitializeComponent();
        }
    }

    public abstract class TipCalcApp : MvxWindowsApplication<MvxFormsWindowsSetup<TipCalcMvvmCrossForms.App, FormsApp>, TipCalcMvvmCrossForms.App, FormsApp, MainPage>
    {
    }
}
