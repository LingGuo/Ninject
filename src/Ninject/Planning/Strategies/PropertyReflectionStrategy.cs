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
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Planning.Directives;
using Ninject.Selection;
#endregion

namespace Ninject.Planning.Strategies
{
	/// <summary>
	/// Adds directives to plans indicating which properties should be injected during activation.
	/// </summary>
	public class PropertyReflectionStrategy : NinjectComponent, IPlanningStrategy
	{
		/// <summary>
		/// Gets the selector component.
		/// </summary>
		public ISelector Selector { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyReflectionStrategy"/> class.
		/// </summary>
		/// <param name="selector">The selector component.</param>
		public PropertyReflectionStrategy(ISelector selector)
		{
			Ensure.ArgumentNotNull(selector, "selector");
			Selector = selector;
		}

		/// <summary>
		/// Adds a <see cref="PropertyInjectionDirective"/> to the plan for each property
		/// that should be injected.
		/// </summary>
		/// <param name="plan">The plan that is being generated.</param>
		public void Execute(IPlan plan)
		{
			Ensure.ArgumentNotNull(plan, "plan");

			foreach (PropertyInfo property in Selector.SelectPropertiesForInjection(plan.Type))
				plan.Add(new PropertyInjectionDirective(property));
		}
	}
}