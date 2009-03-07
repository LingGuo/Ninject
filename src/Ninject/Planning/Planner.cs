﻿#region License
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Strategies;
#endregion

namespace Ninject.Planning
{
	/// <summary>
	/// Generates plans for how to activate instances.
	/// </summary>
	public class Planner : NinjectComponent, IPlanner
	{
		private readonly Dictionary<Type, IPlan> _plans = new Dictionary<Type, IPlan>();

		/// <summary>
		/// Gets the strategies that contribute to the planning process.
		/// </summary>
		public IList<IPlanningStrategy> Strategies { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Planner"/> class.
		/// </summary>
		/// <param name="strategies">The strategies to execute during planning.</param>
		public Planner(IEnumerable<IPlanningStrategy> strategies)
		{
			Ensure.ArgumentNotNull(strategies, "strategies");
			Strategies = strategies.ToList();
		}

		/// <summary>
		/// Gets or creates an activation plan for the specified type.
		/// </summary>
		/// <param name="type">The type for which a plan should be created.</param>
		/// <returns>The type's activation plan.</returns>
		public IPlan GetPlan(Type type)
		{
			Ensure.ArgumentNotNull(type, "type");

			if (_plans.ContainsKey(type))
				return _plans[type];

			var plan = CreateEmptyPlan(type);
			_plans.Add(type, plan);

			Strategies.Map(s => s.Execute(plan));

			return plan;
		}

		/// <summary>
		/// Creates an empty plan for the specified type.
		/// </summary>
		/// <param name="type">The type for which a plan should be created.</param>
		/// <returns>The created plan.</returns>
		protected virtual IPlan CreateEmptyPlan(Type type)
		{
			Ensure.ArgumentNotNull(type, "type");
			return new Plan(type);
		}
	}
}