﻿using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model.Rates
{
    public class AccommodationRate : ISerializable
    {
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public int HostId { get; set; }
        public int Cleanliness { get; set; }

        public int Correctness { get; set; }

        public string AdditionalComment { get; set; }

        public List<string> Images { get; set; }

        public int RecommendationId { get; set; }

        public AccommodationRate()
        {
            Images = new List<string>();
            RecommendationId = -1;
        }

        public AccommodationRate(int reservationId, int guestId, int hostId, int cleanliness, int correctness, string additionalComment, int recommendationId)
        {
            ReservationId = reservationId;
            GuestId = guestId;
            HostId = hostId;
            Cleanliness = cleanliness;
            Correctness = correctness;
            AdditionalComment = additionalComment;
            Images = new List<string>();
            RecommendationId = recommendationId;
        }

        public void FromCSV(string[] values)
        {
            ReservationId = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            HostId = Convert.ToInt32(values[2]);
            Cleanliness = Convert.ToInt32(values[3]);
            Correctness = Convert.ToInt32(values[4]);
            AdditionalComment = values[5];
            Images = MakeListPictures(values[6]);
            RecommendationId = Convert.ToInt32(values[7]);


        }

        public string[] ToCSV()
        {
            string[] csvValues = { ReservationId.ToString(), GuestId.ToString(), HostId.ToString(), Cleanliness.ToString(), Correctness.ToString(),
            AdditionalComment, MakeStringFromPictures(Images), RecommendationId.ToString()};
            return csvValues;
        }

        private List<string> MakeListPictures(string value)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                list = value.Split(",").ToList();
            }

            return list;
        }

        private string MakeStringFromPictures(List<string> pictures)
        {
            string PictureString = "";
            if (pictures != null)
            {
                PictureString = string.Join(",", Images);
            }
            return PictureString;
        }


    }
}
