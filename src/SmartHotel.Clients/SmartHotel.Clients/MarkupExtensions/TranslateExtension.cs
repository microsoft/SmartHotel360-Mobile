using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartHotel.Clients.Core.MarkupExtensions
{
	[ContentProperty( "Text" )]
	public class TranslateExtension : IMarkupExtension
	{
		public string Text { get; set; }
		public string StringFormat { get; set; }

		public object ProvideValue( IServiceProvider serviceProvider )
		{
			if ( Text == null )
				return null;

			var assembly = typeof( TranslateExtension ).GetTypeInfo().Assembly;
			var assemblyName = assembly.GetName();
			ResourceManager resourceManager = new ResourceManager( $"{assemblyName.Name}.Resources", assembly );

			string result = resourceManager.GetString( Text, CultureInfo.CurrentCulture );
			if ( !string.IsNullOrWhiteSpace( StringFormat ) )
			{
				result = string.Format( StringFormat, result );
			}

			return result;

		}
	}
}