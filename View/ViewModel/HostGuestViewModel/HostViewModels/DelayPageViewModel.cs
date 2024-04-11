﻿using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using BookingApp.View.ViewModel.HostGuestViewModel.HostViewModels.Commands;
using BookingApp.View.HostPages;

namespace BookingApp.View.ViewModel.HostGuestViewModel.HostViewModels
{
    public class DelayPageViewModel : INotifyPropertyChanged, IObserver
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<DelayRequestViewModel> Delays { get; set; }

        public DelayRequestService DelayRequestService { get; set; }

        public UserService UserService { get; set; }
        public AccommodationReservationService AccommodationReservationService { get; set; }
        public AccommodationService AccommodationService { get; set; }

        public DelayRequestViewModel SelectedDelay { get; set; }

        public DelayRequestViewModel Delay { get; set; }

       // public ICommand ApproveCommand { get; set; }
      //  public ICommand RejectCommand { get; set; }

        public DelayPageViewModel() {
            Delays = new ObservableCollection<DelayRequestViewModel>();
            DelayRequestService = new DelayRequestService();
            UserService = new UserService();
            AccommodationReservationService = new AccommodationReservationService();
            Delay = new DelayRequestViewModel();
            AccommodationService = new AccommodationService();
           // ApproveCommand = new ApproveDelayCommand(SelectedDelay, Delay, AccommodationReservationService, AccommodationService,
            // UserService, DelayRequestService);
            Update();

        }

        public void Update()
        {
            Delays.Clear();
            foreach (DelayRequest delay in DelayRequestService.GetAll())
            {
                DelayRequestViewModel delayRequestViewModel = new DelayRequestViewModel(delay);
                User user = UserService.GetById(delay.GuestId);
                delayRequestViewModel.GuestUsername = user.Username;
                AccommodationReservation reservation = AccommodationReservationService.GetById(delay.ReservationId);
                Accommodation accommodation = AccommodationService.GetById(AccommodationReservationService.GetById(delay.ReservationId).AccommodationId);
                delayRequestViewModel.ReservationName = reservation.Name;
                delayRequestViewModel.StartLastDate = reservation.StartDate;
                delayRequestViewModel.EndLastDate = reservation.EndDate;
                delayRequestViewModel.Reserved = AccommodationReservationService.IsReserved(delay.StartDate, delay.EndDate, accommodation.Id);
                if (delayRequestViewModel.Status == RequestStatus.PENDING)
                {
                    Delays.Add(delayRequestViewModel);
                }

            }
        }

        public void Approve(object sender, RoutedEventArgs e)
        {
            if (SelectedDelay != null)
            {
                Delay = SelectedDelay;
                Delay.Status = RequestStatus.APPROVED;
                AccommodationReservationService.UpdateReservation(Delay.ReservationId, Delay.StartDate, Delay.EndDate);
                DelayRequestService.Update(Delay.ToDelayRequest());
                Accommodation accommodation = AccommodationService.GetById(AccommodationReservationService.GetById(Delay.ReservationId).AccommodationId);
                ReplaceDates(accommodation, Delay);
                AccommodationService.Update(accommodation);
                Update();
            }
        }

        private void ReplaceDates(Accommodation accommodation, DelayRequestViewModel delay)
        {
            foreach (CalendarDateRange dateRange in accommodation.UnavailableDates)
                if (dateRange.Start == delay.StartLastDate && dateRange.End == delay.EndLastDate)
                {
                    dateRange.Start = delay.StartDate;
                    dateRange.End = delay.EndDate;
                }
        }




        public void Reject(object sender, RoutedEventArgs e)
        {
            if (SelectedDelay != null)
            {
                Delay.Id = SelectedDelay.Id;
                Delay.StartDate = SelectedDelay.StartDate;
                Delay.EndDate = SelectedDelay.EndDate;
                Delay.GuestId  = SelectedDelay.GuestId;
                Delay.ReservationId = SelectedDelay.ReservationId;
                Delay.HostId = SelectedDelay.HostId;
                Delay.Status = RequestStatus.REJECTED;
               // Delay.Comment = DelayPage.ExplanationTextBox.Text;
                DelayRequestService.Update(Delay.ToDelayRequest());
                Update();
            }

        }
       
    }
}
