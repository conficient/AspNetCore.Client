﻿using AspNetCore.Client.Serializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Client
{
	public static class ProtobufInstaller
	{

		public static void UseProtobufSerlaizer(this ClientConfiguration config)
		{
			config.SerializeType = typeof(ProtobufSerializer);
		}
	}
}