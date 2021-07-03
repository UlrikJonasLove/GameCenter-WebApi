using GameCenter.Controllers;
using GameCenter.DTOs;
using GameCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCenter.Tests.UnitTests
{
    [TestClass]
    public class GenresControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetAllGenres()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.Genres.Add(new Genre() { Name = "Genre 2" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            //Testing
            var controller = new GenresController(context2, mapper);
            var response = await controller.Get();

            //Verification
            var genres = response.Value;
            Assert.AreEqual(2, genres.Count);
        }

        [TestMethod]
        public async Task GetGenreByIdDoesNotExist()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //Testing
            var controller = new GenresController(context, mapper);
            var response = await controller.Get(1);
            var result = response.Result as StatusCodeResult;

            //Verification
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task GetGenreByIdExist()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.Genres.Add(new Genre() { Name = "Genre 2" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            //Testing
            var controller = new GenresController(context2, mapper);
            var id = 1;
            var response = await controller.Get(id);
            var result = response.Value;

            //Verification
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task CreateGenre()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var newGenre = new GenreCreationDTO() { Name = "new Genre" };

            //Testing
            var controller = new GenresController(context, mapper);
            var response = await controller.Post(newGenre);
            var result = response as CreatedAtRouteResult;

            //Verification
            Assert.AreEqual(201, result.StatusCode);
            var context2 = BuildContext(databaseName);
            var count = await context2.Genres.CountAsync();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task UpdateGenre()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.SaveChanges();

            //Testing
            var context2 = BuildContext(databaseName);
            var controller = new GenresController(context2, mapper);
            var genreCreation = new GenreCreationDTO() { Name = "New Name" };

            var id = 1;
            var response = await controller.Put(id, genreCreation);
            var result = response as StatusCodeResult;

            //Verification
            Assert.AreEqual(200, result.StatusCode);
            var context3 = BuildContext(databaseName);
            var exist = await context3.Genres.AnyAsync(x => x.Name == "New Name");
            Assert.IsTrue(exist);
        }

        [TestMethod]
        public async Task DeleteGenreNotFound()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var controller = new GenresController(context, mapper);
            var response = await controller.Delete(1);
            var result = response as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteGenre()
        {
            //Preperation
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var controller = new GenresController(context2, mapper);

            var response = await controller.Delete(1);
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context3 = BuildContext(databaseName);
            var exist = await context3.Genres.AnyAsync();
            Assert.IsFalse(exist);
        }
    }
}
