using System;
using Xunit;
using ABuro.Controllers;
using ABuro.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestABuros
{
    public class UnitTest1 : IDisposable
    {
        private readonly ABuroContext _context;
        public UnitTest1() 
        {
            var options = new DbContextOptionsBuilder<ABuroContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ABuroContext(options);
            _context.Database.EnsureCreated();
            var buros = new[]
            {
                new ArchBuro{Id = 1, Name = "Aecom", Address = "abc", ManagerId = 0},
                new ArchBuro{Id = 2, Name = "Gensler", Address = "xyz", ManagerId = 0},
                new ArchBuro{Id = 3, Name = "Jacobs", Address = "iop", ManagerId = 0}
            };
            _context.ArchBuros.AddRange(buros);
            _context.SaveChanges();
        }

        [Fact]
        public async Task ArchBurosGetAll()
        {
            //Arrange
            var burosController = new ArchBurosController(_context);
            //Act
            var result = await burosController.GetArchBuros();
            //Assert
            Assert.IsAssignableFrom<IEnumerable<CompactBuro>>(result.Value);
        }

        [Fact]
        public async Task ArchBurosUpdate()
        {
            //Arrange
            var burosController = new ArchBurosController(_context);
            //Act
            var result = await burosController.PutArchBuro(1, new
                ArchBuro { Id = 100, Name = "Aecom", Address = "rty", ManagerId = 0 });
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task ArchBurosPost()
        {
            //Arrange
            var burosController = new ArchBurosController(_context);
            //Act
            var result = await burosController.PostArchBuro(new
                ArchBuro
            {Name = "Aecom", Address = "abc", ManagerId = 0 });
            //Assert
            Assert.IsType<ActionResult<ArchBuro>>(result);
        }
        [Fact]
        public async Task ArchBurosDelete()
        {   
            //Arrange
            var burosController = new ArchBurosController(_context);
            //Act
            var result = await burosController.DeleteArchBuro(1);
            //Assert
            Assert.IsAssignableFrom<ArchBuro>(result.Value);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
