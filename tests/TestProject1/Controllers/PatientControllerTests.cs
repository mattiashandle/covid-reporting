//using AutoFixture;
//using AutoFixture.Idioms;
//using AutoMapper;
//using Karolinska.Infrastructure.Contexts;
//using Karolinska.Web.Controllers;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging.Abstractions;
//using NUnit.Framework;

//namespace Tests.Karolinska.Web.Controllers
//{
//    [TestFixture]
//    public class PatientControllerTests
//    {
//        [Test, CustomAutoData]
//        public void Constructor_is_guarded(GuardClauseAssertion assertion)
//        {
//            assertion.Verify(typeof(ReportsController).GetConstructors());
//        }

//        [Test, CustomAutoData]
//        public void TestMethod1(IMapper mapper, Patient patient, IFixture fixture)
//        {
//            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
//                  .UseInMemoryDatabase(databaseName: fixture.Create<string>())
//                  .Options;

//            using var context = new KarolinskaContext(dbContextOptions);

//            context.CapacityReports.AddAsync(patient);

//            context.SaveChangesAsync();

//            var sut = new ReportsController(NullLogger<ReportsController>.Instance);

//            var actionResult = sut.Get(new GetPatientByIdQueryHandler(mapper), patient.Id);
//        }
//    }
//}