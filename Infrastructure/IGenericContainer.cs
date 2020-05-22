// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>

namespace Infrastructure
{
    public interface IGenericContainer
    {
        T Resolve<T>();

        void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface;
    }
}