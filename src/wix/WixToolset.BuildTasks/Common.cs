// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolset.BuildTasks
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Common WixTasks utility methods and types.
    /// </summary>
    public static class ToolsCommon
    {
        /// <summary>Metadata key name to turn off harvesting of project references.</summary>
        public const string DoNotHarvest = "DoNotHarvest";

        private static readonly Regex AddPrefix = new Regex(@"^[^a-zA-Z_]");
        private static readonly Regex IllegalIdentifierCharacters = new Regex(@"[^A-Za-z0-9_\.]|\.{2,}"); // non 'words' and assorted valid characters

        /// <summary>
        /// Return an identifier based on passed file/directory name
        /// </summary>
        /// <param name="name">File/directory name to generate identifer from</param>
        /// <returns>A version of the name that is a legal identifier.</returns>
        /// <remarks>This is duplicated from WiX's Common class.</remarks>
        public static string GetIdentifierFromName(string name)
        {
            var result = IllegalIdentifierCharacters.Replace(name, "_"); // replace illegal characters with "_".

            // MSI identifiers must begin with an alphabetic character or an
            // underscore. Prefix all other values with an underscore.
            if (AddPrefix.IsMatch(name))
            {
                result = String.Concat("_", result);
            }

            return result;
        }

        public static string GetMetadataOrDefault(ITaskItem item, string metadataName, string defaultValue)
        {
            var value = item.GetMetadata(metadataName);
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}
