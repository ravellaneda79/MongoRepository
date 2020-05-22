using System;
using Unity;

namespace Infrastructure
{
    public class GenericContainer : IGenericContainer
    {
        private readonly Lazy<IUnityContainer> containerLazy = new Lazy<IUnityContainer>(() => new UnityContainer());
        private static readonly Lazy<IGenericContainer> ContainerWrapper = new Lazy<IGenericContainer>(() => new GenericContainer());

        public static IGenericContainer Instance => ContainerWrapper.Value;

        public T Resolve<T>()
        {
            if (this.containerLazy.Value.IsRegistered(typeof(T)))
            {
                return this.containerLazy.Value.Resolve<T>();
            }

            throw new NotImplementedException($"There is no type {typeof(T).Name} registered.");
        }

        public void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            if (!this.containerLazy.Value.IsRegistered(typeof(TInterface)))
            {
                this.containerLazy.Value.RegisterType<TInterface, TImplementation>();
            }
        }
    }
}