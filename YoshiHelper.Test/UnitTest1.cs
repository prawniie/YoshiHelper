using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;

namespace YoshiHelper.Test
{
    [TestClass]
    public class UnitTest1
    {

        List<BusStation> x = new List<BusStation>
            {
                new BusStation
                {
                    StationName="Lilla bommen",
                    DepartureTimes = new List<TimeSpan>
                    {
                        new TimeSpan(8, 10,0),
                        new TimeSpan(9, 00,0),
                        new TimeSpan(13, 12,0),

                    }
                },

            };

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    // Arrange
        //    var now = new TimeSpan(8, 58, 0);
            
        //    // Act
        //    TimeSpan result = Program.FindNextBus(x,"Lilla bommen",now); 
           
        //    // Assert
        //    Assert.AreEqual(new TimeSpan(9, 0, 0), result);
        //}

        //[TestMethod]
        //public void TestMethod2()
        //{
        //    var now = new TimeSpan(13, 0, 0);
        //    TimeSpan result = Program.FindNextBus(x, "Lilla bommen", now); 
        //    Assert.AreEqual(new TimeSpan(13, 12, 0), result);
        //}

    }
}
