﻿using BookingApp.Application.Services.FeatureServices;
using BookingApp.Application.Services.ReservationServices;
using BookingApp.Domain.Model.Features;
using BookingApp.Domain.Model.Reservations;
using BookingApp.Domain.RepositoryInterfaces.Features;
using BookingApp.Domain.RepositoryInterfaces.Rates;
using BookingApp.Domain.RepositoryInterfaces.Reservations;
using BookingApp.Observer;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.HostGuestViewModel.GuestViewModels
{
    public class NotificationViewModel : IObserver
    {
        public ObservableCollection<string> Notifications { get; set; }
        public DelayRequestService DelayRequestService { get; set; }
        public HostService HostService { get; set; }

        public AccommodationReservationService AccommodationReservationService { get; set; }
        public User User { get; set; }


        public NotificationViewModel(User user)
        {
            User = user;
            Notifications = new ObservableCollection<string>();
            DelayRequestService = new DelayRequestService(Injector.Injector.CreateInstance<IDelayRequestRepository>());
            HostService = new HostService(Injector.Injector.CreateInstance<IHostRepository>(), Injector.Injector.CreateInstance<IAccommodationRateRepository>());
            AccommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(), Injector.Injector.CreateInstance<IDelayRequestRepository>());
        }
        public void Update()
        {
            Notifications.Clear();
            List<DelayRequest> delayRequests = DelayRequestService.GetAll().OrderByDescending(d => d.RepliedDate).ToList();
            foreach (DelayRequest request in delayRequests)
            {

                if (request.Status == RequestStatus.APPROVED && request.GuestId == User.Id)
                {
                    Notifications.Add(CreateApprovedNotification(request));

                }
                if (request.Status == RequestStatus.REJECTED && request.GuestId == User.Id)
                {
                    Notifications.Add(CreateRejectedNotification(request));
                }

            }
        }

        private string CreateRejectedNotification(DelayRequest request)
        {
            string hostUsername = HostService.GetById(request.HostId).Username;
            string message = hostUsername + " has rejected your request.\n";
            AccommodationReservation reservation = AccommodationReservationService.GetById(request.ReservationId);
            string dateRange = reservation.StartDate.ToString("MM/dd/yyyy") + " -> " + reservation.EndDate.ToString("MM/dd/yyyy");
            message += "Reservation" + reservation.Id.ToString() + ": " + dateRange + "\n";
            message += "Time: " + request.RepliedDate.ToString();
            return message;
        }

        private string CreateApprovedNotification(DelayRequest request)
        {
            string hostUsername = HostService.GetById(request.HostId).Username;
            string message = hostUsername + " has approved your request.\n";
            AccommodationReservation reservation = AccommodationReservationService.GetById(request.ReservationId);
            string dateRange = reservation.StartDate.ToString("MM/dd/yyyy") + " -> " + reservation.EndDate.ToString("MM/dd/yyyy");
            message += "Reservation" + reservation.Id.ToString() + ": " + dateRange + "\n";
            message += "Time: " + request.RepliedDate.ToString();
            return message;
        }

        public DelayRequestViewModel GetRequest(string? notification)
        {
            foreach(DelayRequest request in DelayRequestService.GetAll())
            {
                if(notification.Contains(request.RepliedDate.ToString()))
                {
                   
                    return new DelayRequestViewModel(request);
                }
                    
               
            }
            return null;
        }

       
    }
}
