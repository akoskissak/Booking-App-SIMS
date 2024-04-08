﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.GuestPages
{
    /// <summary>
    /// Interaction logic for ProfileInfo.xaml
    /// </summary>
    public partial class ProfileInfo : Page, IObserver
    {
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        public User User { get; set; }
        public AccommodationReservationService AccommodationReservationService { get; set; }

        public Frame Frame {  get; set; }   
        public ProfileInfo(AccommodationReservationService accommodationReservationService, User user, Frame frame)
        {
            InitializeComponent();
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            this.User = user;
            this.Frame = frame;
            this.AccommodationReservationService = accommodationReservationService;
            DataContext = this;
            Update();
        }

        public void Update()
        {
            Reservations.Clear();

            foreach (AccommodationReservation reservation in AccommodationReservationService.GetAll())
            {
                if (reservation.GuestId == User.Id)
                {
                    Reservations.Add(new AccommodationReservationDTO(reservation));
                }
            }
        }
    }
}
