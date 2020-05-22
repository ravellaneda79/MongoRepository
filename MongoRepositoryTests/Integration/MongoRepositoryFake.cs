// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>

using Domain;
using MongoRepositoryDAL;

namespace MongoRepositoryTests.Integration
{
    public class MongoRepositoryFake : GenericMongoRepository<AnyEntity>
    {
        private const string DataBaseTests = "DataBaseTests";
        private const string UsersCollection = "CollectionTests";

        public MongoRepositoryFake() 
            : base(DataBaseTests, UsersCollection)
        {
        }
    }
}