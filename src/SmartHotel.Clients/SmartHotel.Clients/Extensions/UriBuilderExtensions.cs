using System;
using System.IO;

namespace SmartHotel.Clients.Core.Extensions
{
    internal static class UriBuilderExtensions
    {
        internal static void AppendToPath(this UriBuilder builder, string pathToAdd)
        {
            var completePath = Path.Combine(builder.Path, pathToAdd);
            builder.Path = completePath;
        } 
    }
}
