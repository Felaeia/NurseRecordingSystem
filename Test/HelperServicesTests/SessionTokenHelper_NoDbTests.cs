using System;
using Microsoft.Extensions.Configuration;
using Xunit;
using NurseRecordingSystem.Class.Services.HelperServices;

namespace NurseRecordingSystem.Test.ServiceTests.HelperServicesTests
{
    public class SessionTokenHelper_NoDbTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenConnectionStringMissing()
        {
            // Arrange: empty configuration
            var config = new ConfigurationBuilder().Build();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => new sessionTokenHelper(config));
            Assert.Contains("Default Connection", ex.Message);
        }

        [Fact]
        public void ValidateToken_Throws_WhenConnectionStringInvalid()
        {
            
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnectionString", "Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;" }
            };

            // var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var config = new ConfigurationBuilder().AddInMemoryCollection((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string?>>)inMemorySettings).Build();
            var helper = new sessionTokenHelper(config);
            var token = new byte[] { 1, 2, 3 };

            
            Assert.ThrowsAny<Exception>(() => helper.ValidateToken(token));
        }
    }
}
