using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using BookingApp.WPF.ViewModel;
using BookingApp.Domain.Model.Features;
using BookingApp.Repository.FeatureRepository;

namespace BookingApp.View
{
    public partial class NewTourWindow
    {
        private readonly TourRepository _tourRepository;
        public TourViewModel Tour { get; set; }
        private List<DateTime> selectedDates = new List<DateTime>();
        private User Guide;

        public NewTourWindow(User guide) {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            Tour = new TourViewModel();
            Guide = guide;
        }

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            if(Tour.Checkpoints.Count < 2) {
                MessageBox.Show("At least 2 checkpoints are needed in order to make a new tour.");
            }
            else
            {
                int groupId = _tourRepository.NextId();
                foreach (DateTime date in selectedDates)
                {
                    Tour.GuideId = Guide.Id;
                    Tour.GroupId = groupId;
                    Tour.Date = date;
                    Tour.Id = _tourRepository.NextPersonalId();
                    Tour.AvailablePlaces = Tour.MaxTourists;
                    _tourRepository.Add(Tour.ToTour());
                }
            
            }
            MessageBox.Show("Tour added");
            Close();
        }

        private void AddDate_Click(object sender, RoutedEventArgs e)
        {
            //selectedDates.Clear();
            DateTime newDate = dateTimePicker.Value ?? DateTime.MinValue;
            selectedDates.Add(newDate);
            txtDates.ItemsSource = null;
            txtDates.ItemsSource = selectedDates;

        }

        private void AddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            string checkpoint = txtCheckpointTextBox.Text;
            if (!string.IsNullOrEmpty(checkpoint))
            {
                Tour.Checkpoints.Add(checkpoint);
                txtCheckpointTextBox.Text = ""; 
                txtCheckpoints.ItemsSource = null;
                txtCheckpoints.ItemsSource = Tour.Checkpoints;
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(selectedFileName));

                    string imageUrl = selectedFileName;

                    imageUrl = convertToRelativePath(imageUrl);
                    AddPicture(imageUrl);

                    //imgPhoto.Source = bitmapImage; //kasnije verzije gde se prikazuje
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
        }


        public string convertToRelativePath(string input)
        {
            int index = input.IndexOf("Resources");
            if (index != -1)
            {
                return input.Substring(index);
            }
            else
            {
                MessageBox.Show("Please select an image from the resources privided within this app!");
                return input;
            }
        }


        private void AddPicture(string pictureUrl)
        {
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                Tour.Pictures.Add(pictureUrl);
                txtPictureUrlTextBox.Text = ""; 
                txtPictureUrls.ItemsSource = null;
                txtPictureUrls.ItemsSource = Tour.Pictures;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}