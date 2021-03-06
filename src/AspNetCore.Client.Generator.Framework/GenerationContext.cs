﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AspNetCore.Client.Generator.Framework
{
	/// <summary>
	/// Context used to keep track of generation details
	/// </summary>
	public class GenerationContext
	{

		/// <summary>
		/// Clients that will be generated
		/// </summary>
		public IList<Controller> Clients { get; set; } = new List<Controller>();

		/// <summary>
		/// All of the endpoints inside the clients
		/// </summary>
		public IEnumerable<Endpoint> Endpoints => Clients.SelectMany(x => x.Endpoints);

		/// <summary>
		/// Merge this and another context into a new one
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public GenerationContext Merge(GenerationContext other)
		{
			return new GenerationContext
			{
				Clients = this.Clients.Union(other.Clients).ToList()
			};
		}

		/// <summary>
		/// Maps the related information together, like base controllers
		/// </summary>
		public void MapRelatedInfo()
		{
			foreach (var client in Clients)
			{
				if (!string.IsNullOrEmpty(client.BaseClass))
				{
					client.BaseController = Clients.SingleOrDefault(x => x.Name == client.BaseClass);
				}
			}
		}
	}
}
