using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using DTO;
using DAO;
using DTO.Annotations;
using System.ComponentModel;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditRenter.xaml
    /// </summary>
    public partial class EditRenter : INotifyPropertyChanged
    {
        public Action<Renter> UpdateRenterAction;
        public Func<Renter> PassParameterToDialogAction;

        private Renter _renterBeingUpdated;

        public Renter RenterBeingUpdated
        {
            get => _renterBeingUpdated;
            set
            {
                _renterBeingUpdated = value;
                OnPropertyChanged(nameof(RenterBeingUpdated));
            }
        }

        public EditRenter()
        {
            InitializeComponent();
        }

        private void EditRenter_OnLoaded(object sender, RoutedEventArgs e)
        {
            RenterBeingUpdated = PassParameterToDialogAction();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateRenterAction(RenterBeingUpdated);
            this.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
