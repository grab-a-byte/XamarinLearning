namespace XamarinForms.ViewModels
{
    public class TipViewModel : BaseViewModel
    {
        public TipViewModel()
        {
            Generosity = 50;
        }

        private double _amount;
        private int _generosity;
        private double _tip;

        public double Amount { get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
                Recalculate();
            }
        }

        private void Recalculate()
        {
            Tip = Amount * ((double)Generosity / 100);
        }

        public int Generosity
        {
            get => _generosity;
            set
            {
                _generosity = value;
                OnPropertyChanged();
                Recalculate();
            }
        }


        public double Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                OnPropertyChanged();
            }
        }
    }
}
