using System.Linq;
using AutoFixture;
using Domain;
using FluentAssertions;
using Infrastructure;
using MongoRepositoryDAL;
using Xunit;

namespace MongoRepositoryTests.Integration
{
    public class MongoRepositoryTests
    {
        private IFixture fixture;

        public MongoRepositoryTests()
        {
            GenericContainer.Instance.Register<IGenericMongoRepository<AnyEntity>, MongoRepositoryFake>();
            this.fixture = new Fixture();

            var sut = GenericContainer.Instance.Resolve<IGenericMongoRepository<AnyEntity>>();
            sut.DropCollection();
        }

        [Fact]
        public void GivenAnyEntity_WhenSave_ThenCanGetSameEntity()
        {
            // Arrange
            var anyEntity = this.fixture.Create<AnyEntity>();
            var sut = GenericContainer.Instance.Resolve<IGenericMongoRepository<AnyEntity>>();

            // Act
            sut.Save(anyEntity);

            // Assert
            var result = sut.Get(x => x.Id == anyEntity.Id).FirstOrDefault();
            result.Should().NotBeNull();
            result.Should().Match<AnyEntity>(x =>
                x.Id == anyEntity.Id &&
                x.Prop1 == anyEntity.Prop1 &&
                x.Prop2 == anyEntity.Prop2);
        }

        [Fact]
        public void GivenAnyEntitySaved_WhenUpdate_ThenGetUpdatedEntity()
        {
            // Arrange
            var anyEntity = this.fixture.Create<AnyEntity>();
            var sut = GenericContainer.Instance.Resolve<IGenericMongoRepository<AnyEntity>>();
            sut.Save(anyEntity);
            var prop1Update = this.fixture.Create<string>();

            // Act
            sut.Update(x => x.Id == anyEntity.Id, y => y.Prop1, prop1Update);

            // Assert
            var result = sut.Get(x => x.Id == anyEntity.Id).FirstOrDefault();
            result.Should().NotBeNull();
            result.Should().Match<AnyEntity>(x =>
                x.Id == anyEntity.Id &&
                x.Prop1 == prop1Update &&
                x.Prop2 == anyEntity.Prop2);
        }

        [Fact]
        public void GivenAnyEntitySaved_WhenDelete_ThenEntityIsMissing()
        {
            // Arrange
            var anyEntity = this.fixture.Create<AnyEntity>();
            var sut = GenericContainer.Instance.Resolve<IGenericMongoRepository<AnyEntity>>();
            sut.Save(anyEntity);

            // Act
            sut.Delete(x => x.Id == anyEntity.Id);

            // Assert
            var result = sut.Get(x => x.Id == anyEntity.Id).FirstOrDefault();
            result.Should().BeNull();
        }
    }
}
