using MvvmCross.Forms.Views;
using TipCalcMvvmCrossForms.ViewModels;
using Xamarin.Forms.Xaml;

namespace TipCalcMvvmCrossForms.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TipView : MvxContentPage<TipViewModel>
	{
		public TipView ()
		{
			InitializeComponent ();
		}
	}
}