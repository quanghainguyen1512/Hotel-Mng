using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using HotelMng.SubWindows;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DTO.Annotations;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RentersPage.xaml
    /// </summary>
    public partial class RentersPage : INotifyPropertyChanged
    {
        private CollectionView _view;
        private IEnumerable<Renter> _renters;

        public IEnumerable<Renter> Renters
        {
            get => _renters;
            set
            {
                _renters = value; 
                OnPropertyChanged(nameof(Renters));
            }
        }

        public RentersPage()
        {
            InitializeComponent();
            LoadData();

            _view.Filter = RenterFilter;
        }

        private bool RenterFilter(object obj)
        {
            var name = TxbName.Text;
            var idnum = TxbIdNum.Text;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(idnum))
                return true;
            var renter = obj as Renter;
            return renter != null && renter.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0 &&
                   renter.IdentityNum.IndexOf(idnum, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        void LoadData()
        {
            Renters = RenterDAO.Instance.GetAllRenters();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Renters);
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var id = ((Renter)btn.DataContext).RenterId;
            var rowBeingEdited = Renters.FirstOrDefault(s => s.RenterId == id);

            var editor = new EditRenterDialog
            {
                UpdateRenterAction = renter =>
                {
                    if (RenterDAO.Instance.UpdateRenter(rowBeingEdited))
                        LoadData();
                },
                PassParameterToDialogAction = () => rowBeingEdited
            };
            editor.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Txb_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _view.Refresh();
        }
    }
}
