﻿using BookingApp.WPF.ViewModel;
using BookingApp.Domain.Model;
using BookingApp.Application.Services;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model.Features;
using BookingApp.Serializer;
using System.Windows;
using BookingApp.Application.Services.ReservationServices;
using BookingApp.Domain.Model.Reservations;
using BookingApp.Repository.FeatureRepository;
using BookingApp.Repository.ReservationRepository;
using BookingApp.Domain.RepositoryInterfaces.Features;
using BookingApp.Domain.RepositoryInterfaces.Reservations;

namespace BookingApp.Application.Services.FeatureServices
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;

        private readonly TourParticipantService _tourParticipantService;
        private readonly VoucherService _voucherService;
        private readonly TourReservationService _tourReservationService;

        private TouristService _touristService;
        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
            _voucherService = new VoucherService(Injector.Injector.CreateInstance<IVoucherRepository>());
            _touristService = new TouristService(Injector.Injector.CreateInstance<ITouristRepository>());
            _tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>());
            _tourParticipantService = new TourParticipantService(Injector.Injector.CreateInstance<ITourParticipantRepository>());
        }

        public List<Tour> GetAllTours()
        {
            return _tourRepository.GetAll();
        }

        public int FindMaxNumberOfParticipants()
        {
            return _tourRepository.FindMaxNumberOfParticipants();
        }

        public int FindMaxNumberOfParticipants(List<Tour> tours)
        {
            return _tourRepository.FindMaxNumberOfParticipants(tours);
        }

        public List<Tour>? SearchTours(Tour searchCriteria)
        {
            return _tourRepository.SearchTours(searchCriteria);
        }

        public int ToursCount()
        {
            return _tourRepository.ToursCount();
        }

        public Tour UpdateAvailablePlaces(TourViewModel tour, int reducer)
        {
            return _tourRepository.UpdateAvailablePlaces(tour.ToTour(), reducer);
        }

        public List<Tour> FindMyTours(int id)
        {
            Tourist tourist = _touristService.GetTouristById(id);
            return _tourReservationService.FindMyTours(id, tourist.Name, tourist.LastName);
        }

        public List<Tour> FindMyEndedTours(int id)
        {
            Tourist tourist = _touristService.GetTouristById(id);
            return _tourReservationService.FindMyEndedTours(id, tourist.Name, tourist.LastName);
        }


        public List<string> GetParticipantsThatJoinedNow(TouristNotification notification)
        {
            // moram naci rezervacije
            List<TourReservation> reservations = _tourReservationService.FindReservationsByUserIdAndTourId(notification.TourId, notification.TouristId);

            List<string> nowJoinedParticipantNames = new List<string>();
            foreach (var reservation in reservations)
            {
                List<TourParticipant> joinedTourParticipants = _tourParticipantService.GetAllJoinedParticipantsByReservation(reservation.Id);
                foreach(var participant in joinedTourParticipants)
                {
                    // ako se sad prikljucio onda cemo prikazati u notifitikaciji
                    if(participant.JoinedCheckpointIndex == notification.CurrentCheckpoint)
                    {
                        nowJoinedParticipantNames.Add(participant.LastName + " " + participant.Name);
                    }
                }
            }
            return nowJoinedParticipantNames;
        }

        public List<Tour> GetTourByCityWithAvailablePlaces(string city)
        {
            return _tourRepository.GetTourByCityWithAvailablePlaces(city);
        }

        public List<string> GetCheckpointsByTour(int tourId)
        {
            return _tourRepository.GetCheckpointsByTour(tourId);
        }

        public List<Tour> findToursToCancel(int guideId) //izmeniti referencu!
        {
            List<Tour> tours = _tourRepository.findToursByGuideId(guideId);
            foreach (Tour tour in tours.ToList())
            {
                TimeSpan difference = tour.Date - DateTime.Now;
                if (difference.TotalHours <= 48 || tour.Status != TourStatus.inPreparation || tour.Date < DateTime.Now)
                {
                    tours.Remove(tour);
                }
            }
            return tours;
        }

        public void grantVoucher(string reason, DateOnly expireDate, int tour_id, int guide_id)
        {
            List<TourReservation> Reservations = _tourReservationService.GetReservationsByTour(tour_id);
            foreach (TourReservation reservation in Reservations)
            {
                Voucher voucher = new Voucher(0, reservation.TouristId, guide_id, false, reason, expireDate);
                _voucherService.Add(voucher);
            }
        }

        public void cancelTour(int tour_id, int guide_id)
        {
            Tour tour = _tourRepository.GetTourById(tour_id);
            if (tour == null) return;
            tour.Status = TourStatus.Canceled;

            string reason = "Tour " + tour.Name + " has been canceled";
            DateOnly expireDate = new DateOnly(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
            grantVoucher(reason, expireDate, tour_id, guide_id);
            MessageBox.Show(reason);
            _tourRepository.save(tour);
        }

        public List<int> GetAgeStatistic(int id)
        {
            int below18 = 0;
            int above18 = 0;
            int above50 = 0;
            List<TourParticipant> participants = _tourReservationService.GetJoinedParticipantsByTour(id);

            foreach (TourParticipant participant in participants)
            {
                if (participant.Years < 18)
                {
                    below18++;
                }
                else if (participant.Years < 50)
                {
                    above18++;
                }
                else
                {
                    above50++;
                }
            }
            return new List<int> { below18, above18, above50 };

        }

        public Tour GetMostPopularTourForGuide(int guide_id)
        {
            List<Tour> tours = _tourRepository.findToursByGuideId(guide_id);
            Tour mostPopularTour = null;
            int maxParticipants = 0;

            foreach (Tour tour in tours)
            {
                int numberOfParticipants = _tourReservationService.GetNumberOfJoinedParticipants(tour.Id);

                if (numberOfParticipants > maxParticipants)
                {
                    maxParticipants = numberOfParticipants;
                    mostPopularTour = tour;
                }
            }

            return mostPopularTour;
        }

        public Tour GetMostPopularTourForGuideInYear(int guide_id, int year)
        {
            List<Tour> tours = _tourRepository.findToursByGuideId(guide_id);

            Tour mostPopularTourInYear = null;
            int maxParticipantsInYear = 0;

            foreach (Tour tour in tours)
            {
                if (tour.Date.Year == year)
                {
                    int numberOfParticipants = _tourReservationService.GetNumberOfJoinedParticipants(tour.Id);

                    if (numberOfParticipants > maxParticipantsInYear)
                    {
                        maxParticipantsInYear = numberOfParticipants;
                        mostPopularTourInYear = tour;
                    }
                }
            }

            return mostPopularTourInYear;
        }

        public List<Tour> findFinnishedToursByGuide(int guide_id)
        {
            return _tourRepository.findFinnishedToursByGuide(guide_id);
        }

        public bool isTourFinished(int tourId)
        {
            return _tourRepository.isTourFinished(tourId);
        }

    }
}