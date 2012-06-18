﻿using System;
using System.Collections.Generic;
using System.Security;
using Guidelines.Core.Properties;
using Guidelines.Core.Specifications;
using Guidelines.Core.Validation;

namespace Guidelines.Core.Commands
{
	public class CreateCommandHandler<TCreateCommand, TDomain> : IQueryHandler<TCreateCommand, TDomain>, IRegisterMappings
		where TCreateCommand : ICreateCommand<TDomain>
		where TDomain : class
	{
		private readonly IRepository<TDomain> _repository;
		private readonly IValidationEngine _validationEngine;
		private readonly IEnumerable<IPermision<TDomain>> _permisionSet;
		private readonly IEnumerable<ICommandPermision<TCreateCommand, TDomain>> _commandPermisions;
		private readonly ICreateCommandHandler<TCreateCommand, TDomain> _creator;

		public CreateCommandHandler(IRepository<TDomain> repository, IValidationEngine validationEngine, IEnumerable<IPermision<TDomain>> permisionSet, ICreateHandlerFactory<TCreateCommand, TDomain> creator, IEnumerable<ICommandPermision<TCreateCommand, TDomain>> commandPermisions)
		{
			_repository = repository;
			_commandPermisions = commandPermisions;
			_creator = creator.BuildCreator();
			_permisionSet = permisionSet;
			_validationEngine = validationEngine;
		}

		public TDomain Execute(TCreateCommand commandMessage)
		{
			TDomain entity = _creator.Create(commandMessage);

			bool wasCreatedSuccsfully = new Exists<TDomain>()
				.And(new IsAccessibleWith<TDomain>(_permisionSet))
				.And(new CanRunWithCommand<TCreateCommand, TDomain>(commandMessage, _commandPermisions))
				.IsSatisfiedBy(entity);

			if (!wasCreatedSuccsfully)
			{
				throw new SecurityException(Resources.Error_AccessDenied);
			}

			_validationEngine.Validate(entity);
			_repository.Insert(entity);

			return entity;
		}

		public Type SourceType
		{
			get { return typeof (TCreateCommand); }
		}

		public Type DestinationType
		{
			get { return typeof (TDomain); }
		}

		public KeyGenerationMethod KeyGenerationMethod
		{
			get { return KeyGenerationMethod.Default; }
		}
	}
}