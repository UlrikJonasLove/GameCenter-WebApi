using GameCenter.Controllers;
using GameCenter.DTOs;
using GameCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCenter.Tests.UnitTests
{
    [TestClass]
    public class GamesControllerTests : BaseTests
    {
        private string CreateTestData()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var genre = new Genre() { Name = "genre 1" };

            var movies = new List<Game>()
            {
                new Game(){Title = "Game 1", ReleaseDate = new DateTime(2010, 1,1), NewlyRelease = false},
                new Game(){Title = "Future Game", ReleaseDate = DateTime.Today.AddDays(1), NewlyRelease = false},
                new Game(){Title = "Newly Released Game", ReleaseDate = DateTime.Today.AddDays(-1), NewlyRelease = true}
            };

            var gameWithGenre = new Game()
            {
                Title = "Game With Genre",
                ReleaseDate = new DateTime(2010, 1, 1),
                NewlyRelease = false
            };
            movies.Add(gameWithGenre);

            context.Add(genre);
            context.AddRange(movies);
            context.SaveChanges();

            var movieGenre = new GamesGenres() { GenreId = genre.Id, GameId = gameWithGenre.Id };
            context.Add(movieGenre);
            context.SaveChanges();

            return databaseName;
        }

        [TestMethod]
        public async Task FilterByTitle()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new GameController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                Title = "Game 1",
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;
            Assert.AreEqual(1, games.Count);
            Assert.AreEqual("Game 1", games[0].Title);
        }

        [TestMethod]
        public async Task FilterByNewlyReleases()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new GameController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                NewlyReleases = true,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;
            Assert.AreEqual(1, games.Count);
            Assert.AreEqual("Newly Released Game", games[0].Title);
        }

        [TestMethod]
        public async Task FilterByUpcomingReleases()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new GameController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                UpcomingReleases = true,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;
            Assert.AreEqual(1, games.Count);
            Assert.AreEqual("Future Game", games[0].Title);
        }

        [TestMethod]
        public async Task FilterByGenre()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var genreId = context.Genres.Select(x => x.Id).First();

            var context2 = BuildContext(databaseName);

            var controller = new GameController(context2, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                GenreId = genreId,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;
            Assert.AreEqual(1, games.Count);
            Assert.AreEqual("Game With Genre", games[0].Title);
        }

        [TestMethod]
        public async Task FilterOrderByTitleAscending()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new GameController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                OrderingField = "title",
                AscendingOrder = true,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;

            var context2 = BuildContext(databaseName);
            var gamesFromDb = context2.Game.OrderBy(x => x.Title).ToList();

            Assert.AreEqual(gamesFromDb.Count, games.Count);
            for (int i = 0; i < gamesFromDb.Count; i++)
            {
                var gameFromController = games[i];
                var gameFromDb = gamesFromDb[i];

                Assert.AreEqual(gameFromDb.Id, gameFromController.Id);
            }
        }

        [TestMethod]
        public async Task FilterOrderByTitleDescending()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new GameController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                OrderingField = "title",
                AscendingOrder = false,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;

            var context2 = BuildContext(databaseName);
            var gamesFromDb = context2.Game.OrderByDescending(x => x.Title).ToList();

            Assert.AreEqual(gamesFromDb.Count, games.Count);
            for (int i = 0; i < gamesFromDb.Count; i++)
            {
                var gameFromController = games[i];
                var gameFromDb = gamesFromDb[i];

                Assert.AreEqual(gameFromDb.Id, gameFromController.Id);
            }
        }

        [TestMethod]
        public async Task FilterOrderByWrongFieldStillReturnsMovies()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var mock = new Mock<ILogger<GameController>>();
            var controller = new GameController(context, mapper, null, mock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new GameFilterDTO()
            {
                OrderingField = "abcd",
                AscendingOrder = false,
                ItemsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var games = response.Value;

            var context2 = BuildContext(databaseName);
            var moviesFromDb = context2.Game.ToList();
            Assert.AreEqual(moviesFromDb.Count, games.Count);
            Assert.AreEqual(1, mock.Invocations.Count);

        }
    }
}
