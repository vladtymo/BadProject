using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_MVVM
{
    public class ArgbColorModel : INotifyPropertyChanged
    {
        private byte alpha;
        private byte red;
        private byte green;
        private byte blue;

        private bool isAlphaChecked = false;
        private bool isRedChecked = false;
        private bool isGreenChecked = false;
        private bool isBlueChecked = false;

        public bool IsAlphaChecked { get { return isAlphaChecked; } set { Alpha = 0; isAlphaChecked = value; NotifyChanged(); } }
        public bool IsRedChecked { get { return isRedChecked; } set { Red = 0; isRedChecked = value; NotifyChanged(); } }
        public bool IsGreenChecked { get { return isGreenChecked; } set { Green = 0; isGreenChecked = value; NotifyChanged(); } }
        public bool IsBlueChecked { get { return isBlueChecked; } set { Blue = 0; isBlueChecked = value; NotifyChanged(); } }


        private ICommand addCommand;
        private ICommand removeCommand;
        private SolidColorBrush selectedBrush;

        public SolidColorBrush SelectedBrush { get { return selectedBrush; } set { selectedBrush = value; NotifyChanged(); } }

        public ObservableCollection<SolidColorBrush> Brushes { get; } = new ObservableCollection<SolidColorBrush>();
        public byte Alpha { get { return alpha; } set { if (!isAlphaChecked) { alpha = value; Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(alpha, red, green, blue)); NotifyChanged(); } } }
        public byte Red { get { return red; } set { if (!isRedChecked) { red = value; Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(alpha, red, green, blue)); NotifyChanged(); } } }
        public byte Green { get { return green; } set { if (!isGreenChecked) { green = value; Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(alpha, red, green, blue)); NotifyChanged(); } } }
        public byte Blue { get { return blue; } set { if (!isBlueChecked) { blue = value; Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(alpha, red, green, blue)); NotifyChanged(); } } }

        SolidColorBrush brush;
        public SolidColorBrush Color { get { return brush; } set { brush = value; NotifyChanged(); } }

        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(
                       param => AddBrush(),
                        param => IsAddEnabled()
                    );
                }
                return addCommand;
            }

        }
        private bool IsAddEnabled()
        {
            if (Brushes.Contains(brush))
                return false;
            return true;
        }
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(
                       param => RemoveBrush(),
                        param => true
                    );
                }
                return removeCommand;
            }
        }
        private void AddBrush() => Brushes.Add(brush);
        private void RemoveBrush() => Brushes.Remove(SelectedBrush);

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            _execute(parameters);
        }
    }
}
