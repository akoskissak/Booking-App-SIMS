﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model.Rates
{
    public enum RecommendationLevel { LEVEL1, LEVEL2, LEVEL3, LEVEL4, LEVEL5}
    public class RenovationRecommendation : ISerializable
    {
        public int Id;
        public int ReservationId;
        public int AccommodationId;
        public DateTime Date;

        public RecommendationLevel Level;
        public string Comment;
       
        public RenovationRecommendation() {
            Date = DateTime.Now;
        }
        public  RenovationRecommendation(int reservationId, int accommodationId, RecommendationLevel level, string comment)
        {
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            Level = level;
            Comment = comment;

        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            Level = ConvertToLevel(values[3]);
            Comment = values[4];
            Date = Convert.ToDateTime(values[5]);
        }

        private RecommendationLevel ConvertToLevel(string v)
        {
            if(v.Contains("Level 1"))
                    return RecommendationLevel.LEVEL1;
            else if(v.Contains("Level 2"))
                return RecommendationLevel.LEVEL2;
            else if(v.Contains("Level 3"))
                return RecommendationLevel.LEVEL3;
            else if(v.Contains("Level 4"))
                return RecommendationLevel.LEVEL4;
            else
                return RecommendationLevel.LEVEL5;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), AccommodationId.ToString(), ConvertToString(Level), Comment, Date.ToString() };
            return csvValues;
        }

        private string ConvertToString(RecommendationLevel level)
        {
            switch(level)
            {
                case RecommendationLevel.LEVEL1:
                    return "Nivo 1 - bilo bi lepo renovirati neke sitnice, ali sve funkcioniše kako treba i bez toga";
                case RecommendationLevel.LEVEL2:
                    return "Nivo 2 - male zamerke na smeštaj koje kada bi se uklonile bi ga učinile savršenim";
                case RecommendationLevel.LEVEL3:
                    return "Nivo 3 - nekoliko stvari koje su baš zasmetale bi trebalo renovirati";
                case RecommendationLevel.LEVEL4:
                    return "Nivo 4 - ima dosta loših stvari i renoviranje je stvarno neophodno";
                case RecommendationLevel.LEVEL5:
                    return "Nivo 5 - smeštaj je u jako lošem stanju i ne vredi ga uopšte iznajmljivati ukoliko se ne renovira";
                default:
                    return "";


            }

            
        }
    }
}
