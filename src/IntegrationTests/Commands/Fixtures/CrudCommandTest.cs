﻿using Guidelines.Core;
using Guidelines.Ioc.StructureMap.Bootstrap;
using Guidelines.Logging.Log4Net;
using NUnit.Framework;
using StructureMap;

namespace Guidelines.IntegrationTests.Commands.Fixtures
{
	[TestFixture]
	public abstract class CrudCommandTest
	{
		protected Bootstrap Bootstrapper;
		protected IContainer Container;
		protected MemoryRepository<TestEntity> Repository;

		[TestFixtureSetUp]
		public void Setup()
		{
			Container = new Container();
			Bootstrapper = new Bootstrap()
				.WithContainer(Container)
				.WithRegistrar(new StructuremapRegistrar())
				.WithLogger(new Logger<CrudCommandTest>())
				.Start();
			Repository = Container.GetInstance<MemoryRepository<TestEntity>>();

			SetupTestContext();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			Repository.Clear();
			Bootstrapper.Dispose();
		}

		public class TestEntity : EntityBase<TestEntity>
		{
			public string Name { get; set; }
		}

		public abstract void SetupTestContext();
	}
}