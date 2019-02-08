using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinForms.ViewModels;

namespace XamarinForms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TipView : ContentPage
	{
		public TipView ()
		{
            BindingContext = new TipViewModel();
			InitializeComponent ();
		}
	}
}