﻿namespace SharpArchCookbook.Web.Mvc.CastleWindsor
{
    using Castle.Core;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Contracts.Repositories;
    using SharpArch.Web.Mvc.Castle;

    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container) 
        {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddQueriesTo(container);
            AddApplicationServicesTo(container);
        }

        private static void AddApplicationServicesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes
                    .FromAssemblyNamed("SharpArchCookbook.Tasks")
                    .Pick()
                    .WithService.FirstInterface());
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container) 
        {
            container.Register(
                AllTypes
                    .FromAssemblyNamed("SharpArchCookbook.Infrastructure")
                    .Pick()
                    .WithService.FirstNonGenericCoreInterface("SharpArchCookbook.Domain"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof(IQuery<>))
                    .ImplementedBy(typeof(NHibernateQuery<>))
                    .Named("NHibernateQuery"));

            container.Register(
                Component.For(typeof(IEntityDuplicateChecker))
                    .ImplementedBy(typeof(EntityDuplicateChecker))
                    .Named("entityDuplicateChecker"));

            container.Register(
                Component.For(typeof(INHibernateRepository<>))
                    .ImplementedBy(typeof(NHibernateRepository<>))
                    .Named("nhibernateRepositoryType"));

            container.Register(
                Component.For(typeof(INHibernateRepositoryWithTypedId<,>))
                    .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
                    .Named("nhibernateRepositoryWithTypedId"));

            container.Register(
                    Component.For(typeof(ISessionFactoryKeyProvider))
                        .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
                        .Named("sessionFactoryKeyProvider"));
        }

        private static void AddQueriesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.FromAssemblyNamed("SharpArchCookbook.Web.Mvc")
                    .Pick()
                    .WithService.FirstInterface());
        }
    }
}