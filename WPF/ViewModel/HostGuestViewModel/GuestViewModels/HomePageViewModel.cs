﻿using BookingApp.Application.Services.FeatureServices;
using BookingApp.Domain.Model.Features;
using BookingApp.Domain.RepositoryInterfaces.Features;
using BookingApp.Domain.RepositoryInterfaces.Reservations;
using BookingApp.Repository;
using BookingApp.View;
using BookingApp.View.GuestPages;
using BookingApp.WPF.View.Guest.GuestPages;
using BookingApp.WPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.HostGuestViewModel.GuestViewModels
{
    public class HomePageViewModel
    {
        public User User { get; set; }
        public Frame Frame { get; set; }
        public Guest Guest { get; set; }
        public GuestService GuestService { get; set; }
        public GuestICommand ReserveCommand { get; set; }
        public GuestICommand AboutCommand { get; set; }
        public GuestICommand HelpCommand { get; set; }
        public GuestWindow GuestWindow { get; set; }
        public HomePageViewModel(User user, Frame frame, GuestWindow guestWindow) { 
            
            User = user;
            Frame = frame;
            GuestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>(), Injector.Injector.CreateInstance<IAccommodationReservationRepository>(), Injector.Injector.CreateInstance<IDelayRequestRepository>());
            Guest = GuestService.GetById(User.Id);
            GuestService.CalculateGuestStats(Guest);
            ReserveCommand = new GuestICommand(OnReserve);
            AboutCommand = new GuestICommand(OnAbout);
            HelpCommand = new GuestICommand(OnHelp);
            GuestWindow = guestWindow;
          

        }

        private void OnAbout()
        {
            Frame.Content = new AboutPage(Frame);
            GuestWindow.backButton.Visibility = Visibility.Visible;
            GuestWindow.aboutMenu.IsChecked = true;
        }

        private void OnHelp()
        {
            Frame.Content = new HelpPage(Frame);
            GuestWindow.backButton.Visibility = Visibility.Visible;
            GuestWindow.helpMenu.IsChecked = true;
        }

        private void OnReserve()
        {

            Frame.Content = new AccommodationsPage(User, Frame);
            GuestWindow.backButton.Visibility = Visibility.Visible;
            GuestWindow.accommodationsMenu.IsChecked = true;
        }
    }
}
