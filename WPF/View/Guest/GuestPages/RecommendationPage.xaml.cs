﻿using BookingApp.Domain.Model.Features;
using BookingApp.WPF.ViewModel.HostGuestViewModel;
using BookingApp.WPF.ViewModel.HostGuestViewModel.GuestViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guest.GuestPages
{
    /// <summary>
    /// Interaction logic for RecommendationPage.xaml
    /// </summary>
    public partial class RecommendationPage : Page
    {
        public User User;
        public Frame Frame;
        public AccommodationReservationViewModel SelectedReservation;
        public AccommodationViewModel SelectedAccommodation;
        public AccommodationRateViewModel AccommodationRate;
        public RecommendationPageViewModel ViewModel { get; set; }
      

        public RecommendationPage(User user, Frame frame, AccommodationReservationViewModel selectedReservation, AccommodationViewModel selectedAccommodation, AccommodationRateViewModel accommodationRate)
        {
            InitializeComponent();
            User = user;
            Frame = frame;
            SelectedReservation = selectedReservation;
            SelectedAccommodation = selectedAccommodation;
            AccommodationRate = accommodationRate;
            ViewModel = new RecommendationPageViewModel(User, Frame, SelectedReservation, SelectedAccommodation, AccommodationRate);
            DataContext = ViewModel;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save_Click(sender, e);
        }
    }
}