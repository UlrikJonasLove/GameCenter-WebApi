using GameCenter.Controllers;
using GameCenter.DTOs;
using GameCenter.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCenter.Tests.UnitTests
{
    [TestClass]
    public class GameCentersControllerTests
    {
        [TestMethod]
        public async Task GetGameCenters5KmsOrCloser()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            using (var context = LocalDbDatabaseinitializer.GetDbContextLocalDb())
            {
                var centers = new List<GameCenters>
                    {
                        new GameCenters{Name = "Agora", Location = geometryFactory.CreatePoint(new Coordinate(-69.9388777, 18.4839233))},
                        new GameCenters{Name = "Sambil", Location = geometryFactory.CreatePoint(new Coordinate(-69.9118804, 18.4826214))},
                        new GameCenters{Name = "Megacentro", Location = geometryFactory.CreatePoint(new Coordinate(-69.856427, 18.506934))},
                        new GameCenters{Name = "Village East Center", Location = geometryFactory.CreatePoint(new Coordinate(-73.986227, 40.730898))}
                    };

                context.AddRange(centers);
                context.SaveChanges();
            }

            var gameCenterFilterDTO = new GameCenterFilterDTO()
            {
                DistanceInKms = 5,
                Lat = 18.481139,
                Long = -69.938950
            };

            using (var context = LocalDbDatabaseinitializer.GetDbContextLocalDb())
            {
                var controller = new GameCentersController(context);
                var response = await controller.Get(gameCenterFilterDTO);
                var centersFromController = response.Value;
                Assert.AreEqual(0, centersFromController.Count);
            }
        }
    }
}
