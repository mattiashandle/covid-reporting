using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Karolinska.Application.AutoMapper;

namespace Tests.Karolinska.Web
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(() => FixtureHelper.CreateFixture(null)) { }

        public CustomAutoDataAttribute(Type type, string methodName) : base(FixtureHelper.CreateFixtureWithMethod(type, methodName)) { }
    }

    public class InlineCustomAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineCustomAutoDataAttribute(params object[] arguments) : base(() => FixtureHelper.CreateFixture(null), arguments) { }

        public InlineCustomAutoDataAttribute(Type type, string methodName, params object[] arguments) : base(FixtureHelper.CreateFixtureWithMethod(type, methodName), arguments) { }
    }

    public static class FixtureHelper
    {
        public static IFixture CreateFixture(Action<IFixture>? method)
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Register<IMapper>(() =>
            {
                var configuration = new MapperConfiguration(cfg => cfg.AddMaps(new[] { typeof(SupplierProfile) }));

                var mapper = new Mapper(configuration);

                return mapper;
            });

            method?.Invoke(fixture);

            return fixture;
        }

        public static Func<IFixture> CreateFixtureWithMethod(Type type, string methodName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            var method = (Action<IFixture>)Delegate.CreateDelegate(typeof(Action<IFixture>), type, methodName);

            if (method == null)
            {
                throw new ArgumentException($"Method '{methodName}' not found in {type.Name}.", methodName);
            }

            return () => CreateFixture(method);
        }
    }
}
