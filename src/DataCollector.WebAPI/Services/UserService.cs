﻿using DataCollector.WebAPI.Models.Api;
using DataCollector.WebAPI.Models.Dto;
using DataCollector.WebAPI.Models.Entities;
using DataCollector.WebAPI.Models.Interfaces;
using LinqKit;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollector.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContext _db;
        public UserService(IDbContext db)
        {
            _db = db;
        }

        public Task<User> GetByIdAsync(string userId)
        {
            //todo: create mapper

            var objectId = new ObjectId(userId);
            var user = _db.Users.AsQueryable()
                                .AsExpandable()
                                .SingleOrDefaultAsync(u => u.Id == objectId);

            return user;
        }

        public Task<List<UserDto>> GetAsync(UserFilterModel filterModel)
        {
            var filter = PredicateBuilder.New<User>(u => true);
            #region expressions
            if (!string.IsNullOrEmpty(filterModel.CommonInfo.FirstName))
            {
                filter = filter.And(u => u.CommonInfo.FirstName.Contains(filterModel.CommonInfo.FirstName));
            }

            if (!string.IsNullOrEmpty(filterModel.CommonInfo.LastName))
            {
                filter = filter.And(u => u.CommonInfo.LastName.Contains(filterModel.CommonInfo.LastName));
            }

            if (filterModel.CommonInfo.Gender != null)
            {
                filter = filter.And(u => u.CommonInfo.Gender == filterModel.CommonInfo.Gender);
            }

            if (filterModel.CommonInfo.FromAge != null)
            {
                filter = filter.And(u => u.CommonInfo.Age >= filterModel.CommonInfo.FromAge);
            }

            if (filterModel.CommonInfo.ToAge != null)
            {
                filter = filter.And(u => u.CommonInfo.Age <= filterModel.CommonInfo.ToAge);
            }

            if (!string.IsNullOrEmpty(filterModel.CommonInfo.Country))
            {
                filter = filter.And(u => u.CommonInfo.Country.Contains(filterModel.CommonInfo.Country));
            }

            if (!string.IsNullOrEmpty(filterModel.CommonInfo.City))
            {
                filter = filter.And(u => u.CommonInfo.City.Contains(filterModel.CommonInfo.City));
            }

            if (!string.IsNullOrEmpty(filterModel.Contacts.MobilePhone))
            {
                filter = filter.And(u => u.Contacts.MobilePhone.Contains(filterModel.Contacts.MobilePhone));
            }

            if (!string.IsNullOrEmpty(filterModel.Education.Country))
            {
                filter = filter.And(u => u.Education.Any(ev => ev.Country.Contains(filterModel.Education.Country)));
            }

            if (!string.IsNullOrEmpty(filterModel.Education.City))
            {
                filter = filter.And(u => u.Education.Any(ev => ev.City.Contains(filterModel.Education.City)));
            }

            if (!string.IsNullOrEmpty(filterModel.Education.EducationalInstitution))
            {
                filter = filter.And(u => u.Education.Any(ev => ev.EducationalInstitution.Contains(filterModel.Education.EducationalInstitution)));
            }

            if (!string.IsNullOrEmpty(filterModel.Education.Speciality))
            {
                filter = filter.And(u => u.Education.Any(ev => ev.Speciality.Contains(filterModel.Education.Speciality)));
            }

            //if (!string.IsNullOrEmpty(filterModel.Сareer.Country))
            //{
            //    filter = filter.And(u => u.Career.Any(ev => ev.Country.Contains(filterModel.Сareer.Country)));
            //}

            //if (!string.IsNullOrEmpty(filterModel.Сareer.City))
            //{
            //    filter = filter.And(u => u.Career.Any(ev => ev.City.Contains(filterModel.Сareer.City)));
            //}

            //if (!string.IsNullOrEmpty(filterModel.Сareer.Position))
            //{
            //    filter = filter.And(u => u.Career.Any(ev => ev.Position.Contains(filterModel.Сareer.Position)));
            //}

            //if (!string.IsNullOrEmpty(filterModel.Сareer.PlaceOfWork))
            //{
            //    filter = filter.And(u => u.Career.Any(ev => ev.Position.Contains(filterModel.Сareer.PlaceOfWork)));
            //}

            if (!string.IsNullOrEmpty(filterModel.LifePositions.WorldView))
            {
                filter = filter.And(u => u.LifePositions.WorldView == filterModel.LifePositions.WorldView);
            }

            if (!string.IsNullOrEmpty(filterModel.LifePositions.MainInLife))
            {
                filter = filter.And(u => u.LifePositions.MainInLife == filterModel.LifePositions.MainInLife);
            }

            if (!string.IsNullOrEmpty(filterModel.LifePositions.MainInPeople))
            {
                filter = filter.And(u => u.LifePositions.MainInPeople == filterModel.LifePositions.MainInPeople);
            }

            if (!string.IsNullOrEmpty(filterModel.LifePositions.PositionToSigarets))
            {
                filter = filter.And(u => u.LifePositions.PositionToSigarets == filterModel.LifePositions.PositionToSigarets);
            }

            if (!string.IsNullOrEmpty(filterModel.LifePositions.PositionToAlhocol))
            {
                filter = filter.And(u => u.LifePositions.PositionToAlhocol == filterModel.LifePositions.PositionToAlhocol);
            }

            if (!string.IsNullOrEmpty(filterModel.Activity.Books))
            {
                var books = filterModel.Activity.Books.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(x => x.Trim());

                filter = filter.And(u => u.Activities.Books.Any(b => books.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Activity.Films))
            {
                var films = filterModel.Activity.Films.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(x => x.Trim());

                filter = filter.And(u => u.Activities.Films.Any(f => films.Contains(f)));
            }

            if (!string.IsNullOrEmpty(filterModel.Activity.Games))
            {
                var games = filterModel.Activity.Games.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(x => x.Trim());

                filter = filter.And(u => u.Activities.Games.Any(b => games.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Activity.Musics))
            {
                var musics = filterModel.Activity.Musics.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(x => x.Trim());

                filter = filter.And(u => u.Activities.Musics.Any(b => musics.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Activity.Hoobies))
            {
                var hoobies = filterModel.Activity.Hoobies.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(x => x.Trim());

                filter = filter.And(u => u.Activities.Hoobies.Any(b => hoobies.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Interest.TypesOfBooks))
            {
                var typesOfBooks = filterModel.Interest.TypesOfBooks.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                               .Select(x => x.Trim());

                filter = filter.And(u => u.Interests.TypesOfBooks.Any(b => typesOfBooks.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Interest.TypesOfFilms))
            {
                var typesOfFilms = filterModel.Interest.TypesOfFilms.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                               .Select(x => x.Trim());

                filter = filter.And(u => u.Interests.TypesOfFilms.Any(b => typesOfFilms.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Interest.TypesOfGames))
            {
                var typesOfGames = filterModel.Interest.TypesOfGames.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                               .Select(x => x.Trim());

                filter = filter.And(u => u.Interests.TypesOfGames.Any(b => typesOfGames.Contains(b)));
            }

            if (!string.IsNullOrEmpty(filterModel.Interest.TypesOfMusic))
            {
                var typesOfMusic = filterModel.Interest.TypesOfMusic.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                               .Select(x => x.Trim());

                filter = filter.And(u => u.Interests.TypesOfMusic.Any(b => typesOfMusic.Contains(b)));
            }

            #endregion

            //var filterDefinitionBuilder = new FilterDefinitionBuilder<User>();
            //var filterDefinition = filterDefinitionBuilder.Where(filter);

            //using var userCursor = await _db.Users.FindAsync(filterDefinition);
            //await userCursor.MoveNextAsync();

            var query = _db.Users.AsQueryable()
                                 .AsExpandable()
                                 .Where(filter)
                                 .Select(u => new UserDto()
                                 {
                                     Id = u.Id,
                                     FirstName = u.CommonInfo.FirstName,
                                     LastName = u.CommonInfo.LastName,                                  
                                     City = u.CommonInfo.City,
                                     MobilePhone = u.Contacts.MobilePhone,
                                     Email = u.Contacts.Email,
                                     Vk = u.Contacts.Vk
                                 });

            return query.ToListAsync();
        }    
    }
}
