﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApp.Model;

namespace BookingApp.DTO
{
    public class DelayRequestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }

        private int hostId;
        public int HostId
        {
            get { return hostId; }
            set
            {
                if (hostId != value)
                {
                    hostId = value;
                    OnPropertyChanged("HostId");
                }
            }
        }

        private int reservationId;
        public int ReservationId
        {
            get { return reservationId; }
            set
            {
                if (reservationId != value)
                {
                    reservationId = value;
                    OnPropertyChanged("ReservationId");
                }
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {

                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {

                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {

                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private RequestStatus status;
        public RequestStatus Status
        {
            get { return status; }
            set
            {
                if( status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DelayRequestViewModel()
        {

        }

        public DelayRequestViewModel(DelayRequest dr)
        {
            id = dr.Id;
            guestId = dr.GuestId;
            hostId = dr.HostId;
            reservationId = dr.ReservationId;
            startDate = dr.StartDate;
            endDate = dr.EndDate;
            comment = dr.Comment;
            status = dr.Status;
        }

        public DelayRequest ToDelayRequest()
        {
            DelayRequest request = new DelayRequest(guestId, hostId, reservationId, startDate, endDate, status, comment);
            request.Id = id;
            return request;
        }

    }
}