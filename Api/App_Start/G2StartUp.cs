using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFL = G2.Frameworks.Logging;
using DFC = G2.Frameworks.Core;
using DTBS = G2.DB.BusinessServices;

namespace G2.DB.Api
{
	public class G2StartUp
	{
		public static void Configuration()
		{
			/*Logging default settings...*/
			DFL.DefaultLogManagerFactory.LogManager.Configure("DefaultLogger");

			/*Set Encryption key...*/
			DFC.Encryption.EncryptionKey = "DiaBroker";
			DFL.DefaultLogManagerFactory.LogManager.Debug("EncryptionKey key has been set...");

			/*Register AutoMapper settings...*/
			DFL.DefaultLogManagerFactory.LogManager.Debug("AutoMapper settings started...");
			AutoMapper.Mapper.Initialize(cfg =>
			{
				Infrastructure.AutoMapperConfiguration.Configure(cfg);
				//DTBS.AutoMapperConfiguration.Configure(cfg);
			});
			DFL.DefaultLogManagerFactory.LogManager.Debug("AutoMapper settings initialized...");
			AutoMapper.Mapper.AssertConfigurationIsValid();
			DFL.DefaultLogManagerFactory.LogManager.Debug("AutoMapper settings validated...");
		}
	}
}