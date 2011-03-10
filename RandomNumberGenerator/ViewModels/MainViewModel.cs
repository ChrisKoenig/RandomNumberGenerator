using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace RandomNumberGenerator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        [XmlIgnore]
        public RelayCommand ResetCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand NextCommand { get; private set; }

        public MainViewModel()
        {
            ResetCommand = new RelayCommand(new System.Action(ResetMethod));
            NextCommand = new RelayCommand(
                () => NextMethod(), () => CanGenerateNumber);
        }

        private Random _random = new Random();

        private void ResetMethod()
        {
            GeneratedNumbersList.Clear();
            _random = new Random();
            RaisePropertyChanged("CanGenerateNumber");
        }

        private void AddItemToList(int i)
        {
            GeneratedNumbersList.Add(i);
            RaisePropertyChanged("CanGenerateNumber");
        }

        private void NextMethod()
        {
            if (MaximumValue - GeneratedNumbersList.Count <= 1)
            {
                for (int i = 1; i <= MaximumValue; i++)
                {
                    if (!GeneratedNumbersList.Contains(i))
                    {
                        AddItemToList(i);
                        return;
                    }
                }
            }
            bool stillLooking = true;
            int nextNumber = 0;
            while (stillLooking)
            {
                nextNumber = _random.Next(MaximumValue + 1);
                stillLooking = nextNumber == 0 || GeneratedNumbersList.Contains(nextNumber);
            }
            AddItemToList(nextNumber);
        }

        public bool CanGenerateNumber
        {
            get
            {
                return MaximumValue > GeneratedNumbersList.Count;
            }
        }

        #region MaximumValue property

        public const string MaximumValuePropertyName = "MaximumValue";

        private int _max = 100;

        public int MaximumValue
        {
            get
            {
                return _max;
            }

            set
            {
                if (_max == value)
                {
                    return;
                }

                var oldValue = _max;
                _max = value;
                RaisePropertyChanged(MaximumValuePropertyName);
                ResetMethod();
            }
        }

        #endregion MaximumValue property

        #region GeneratedNumbersList property

        public const string GeneratedNumbersListPropertyName = "GeneratedNumbersList";

        private ObservableCollection<int> _numbers = new ObservableCollection<int>();

        public ObservableCollection<int> GeneratedNumbersList
        {
            get
            {
                return _numbers;
            }

            set
            {
                if (_numbers == value)
                {
                    return;
                }

                var oldValue = _numbers;
                _numbers = value;

                RaisePropertyChanged(GeneratedNumbersListPropertyName);
            }
        }

        #endregion GeneratedNumbersList property
    }
}