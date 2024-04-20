﻿using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestRateService
    {
        private readonly GuestRateRepository GuestRateRepository;

        public GuestRateService()
        {
            GuestRateRepository = new GuestRateRepository();
        }

        public List<GuestRate> GetAll()
        {
            return GuestRateRepository.GetAll();
        }  

       
    }
}
