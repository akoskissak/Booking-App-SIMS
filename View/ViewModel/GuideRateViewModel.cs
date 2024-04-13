﻿using BookingApp.Model;
using BookingApp.Services;
using BookingApp.View.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookingApp.ViewModel
{
    public class GuideRateViewModel : INotifyPropertyChanged
    {
        private readonly GuideRateService _guideRateService;
        public ObservableCollection<BitmapImage> imagePreviews {  get; set; }
        public ICommand SelectKnowledgeRatingCommand { get; set; }
        public ICommand SelectLanguageRatingCommand { get; set; }
        public ICommand SelectInterestRatingCommand { get; set; }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private int _touristId;
        public int TouristId
        {
            get
            {
                return _touristId;
            }
            set
            {
                if (_touristId != value)
                {
                    _touristId = value;
                    OnPropertyChanged(nameof(TouristId));
                }
            }
        }

        private int _tourId;
        public int TourId
        {
            get
            {
                return _tourId;
            }
            set
            {
                if( _tourId != value)
                {
                    _tourId = value;
                    OnPropertyChanged(nameof(TourId));
                }
            }
        }

        private int _guideId;
        public int GuideId
        {
            get
            {
                return _guideId;
            }
            set
            {
                if( _guideId != value)
                {
                    _guideId = value;
                    OnPropertyChanged(nameof(GuideId));
                }
            }
        }

        private int _knowledge;
        public int Knowledge
        {
            get
            {
                return _knowledge;
            }
            set
            {
                _knowledge = value;
                OnPropertyChanged(nameof(Knowledge));
            }
        }

        private int _language;
        public int Language
        {
            get
            {
                return _language;
            }
            set
            {
                if( _language != value)
                {
                    _language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }

        private int _tourInterest;
        public int TourInterest
        {
            get
            {
                return _tourInterest;
            }
            set
            {
                if(_tourInterest != value)
                {
                    _tourInterest = value;
                    OnPropertyChanged(nameof(TourInterest));
                }
            }
        }

        private string _additionalComment;
        public string AdditionalComment
        {
            get
            {
                return _additionalComment;
            }
            set
            {
                if(_additionalComment != value)
                {
                    _additionalComment = value;
                    OnPropertyChanged(nameof(AdditionalComment));
                }
            }
        }

        private List<string> _pictures;
        public List<string> Pictures
        {
            get
            {
                return _pictures;
            }
            set
            {
                if( _pictures != value)
                {
                    _pictures = value;
                    OnPropertyChanged(nameof(Pictures));
                }
            }
        }

        private bool _isAddImageButtonEnabled;
        public bool IsAddImageButtonEnabled
        {
            get
            {
                return _isAddImageButtonEnabled;
            }
            set
            {
                if( value != _isAddImageButtonEnabled )
                {
                    _isAddImageButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddImageButtonEnabled));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddImage()
        {
            if(imagePreviews.Count >= 4)
            {
                IsAddImageButtonEnabled = false;
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(fileName));
                    imagePreviews.Add(bitmap);
                }
            }
        }

        public void Submit(GuideRateViewModel rate)
        {
            Pictures = imagePreviews.Select(image => image.UriSource.AbsoluteUri).ToList();
            _guideRateService.SaveRate(rate);
        }

        public bool initializeGuideRate(int tourId, int guideId, int userId)
        {
            IsAddImageButtonEnabled = true;
            TouristId = userId;
            TourId = tourId;
            GuideId = guideId;
            Knowledge = 1;
            Language = 1;
            TourInterest = 1;
            return false;
        }

        private void ExecuteSelectKnowledgeRating(object parameter)
        {
            int selectedIndex = int.Parse(parameter.ToString());

            Knowledge = selectedIndex + 1;
        }

        private void ExecuteSelectLanguageRating(object parameter)
        {
            int selectedIndex = int.Parse(parameter.ToString());

            Language = selectedIndex + 1;
        }
        private void ExecuteSelectInterestRating(object parameter)
        {
            int selectedIndex = int.Parse(parameter.ToString());

            TourInterest = selectedIndex + 1;
        }

        public GuideRateViewModel() {
            _guideRateService = new GuideRateService();

            imagePreviews = new ObservableCollection<BitmapImage>();
            SelectKnowledgeRatingCommand = new RelayCommand(ExecuteSelectKnowledgeRating);
            SelectLanguageRatingCommand = new RelayCommand(ExecuteSelectLanguageRating);
            SelectInterestRatingCommand = new RelayCommand(ExecuteSelectInterestRating);
        }
        public GuideRateViewModel(GuideRate guideRate)
        {
            Id = guideRate.Id;
            TouristId = guideRate.TouristId;
            TourId = guideRate.TourId;
            GuideId = guideRate.GuideId;
            Knowledge = guideRate.Knowledge;
            Language = guideRate.Language;
            TourInterest = guideRate.TourInterest;
            AdditionalComment = guideRate.AdditionalComment;
            Pictures = guideRate.Pictures;
        }

        public GuideRate toGuideRate()
        {
            GuideRate guideRate = new GuideRate(_id, _touristId, _tourId, _guideId, _knowledge, _language, _tourInterest, _additionalComment, _pictures);
            return guideRate;
        }
    }
}