﻿using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.DTO;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View
{
    public partial class NewTourWindow
    {
        private readonly TourRepository _tourRepository;
        public TourDTO Tour { get; set; }
        private List<DateTime> selectedDates = new List<DateTime>();

        public NewTourWindow() {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            Tour = new TourDTO();
        }

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            if(Tour.Checkpoints.Count < 2) {
                MessageBox.Show("At least 2 checkpoints are needed in order to make a new tour.");
            }
            else
            {
                foreach(DateTime date in selectedDates)
                {
                    Tour.GroupId = _tourRepository.NextId();
                    Tour.Date = date;
                    _tourRepository.Add(Tour.ToTour());
                }
            
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDates.Clear();

            foreach (var selectedDate in calendar.SelectedDates)
            {
                selectedDates.Add(selectedDate);
            }
        }

        private void AddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            string checkpoint = txtCheckpointTextBox.Text;
            if (!string.IsNullOrEmpty(checkpoint))
            {
                Tour.Checkpoints.Add(checkpoint);
                txtCheckpointTextBox.Text = ""; // Clear the input textbox after adding the checkpoint
                txtCheckpoints.ItemsSource = null;
                txtCheckpoints.ItemsSource = Tour.Checkpoints;
            }
        }


        private void AddPicture_Click(object sender, RoutedEventArgs e)
        {
            string pictureUrl = txtPictureUrlTextBox.Text;
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                Tour.Pictures.Add(pictureUrl);
                txtPictureUrlTextBox.Text = ""; 
                txtPictureUrls.ItemsSource = null;
                txtPictureUrls.ItemsSource = Tour.Pictures;
            }
        }

    }
}
