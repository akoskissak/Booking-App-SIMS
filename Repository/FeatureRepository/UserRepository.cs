﻿using BookingApp.Domain.Model.Features;
using BookingApp.Domain.RepositoryInterfaces.Features;
using BookingApp.Observer;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;
        public Subject UserSubject { get; set; }

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
            UserSubject = new Subject();
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public User GetById(int id)
        {
            foreach (User user in _users)
            {
                if (id == user.Id) return user;
            }

            return null;
        }

        public User Update(User user)
        {
            _users = _serializer.FromCSV(FilePath);
            User current = _users.Find(a => a.Id == user.Id);
            int index = _users.IndexOf(current);
            _users.Remove(current);
            _users.Insert(index, user);
            _serializer.ToCSV(FilePath, _users);
            UserSubject.NotifyObservers();
            return user;
        }

    }
}
