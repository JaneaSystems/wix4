// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolset.Core.WindowsInstaller
{
    using System.Collections.Generic;
    using WixToolset.Core.WindowsInstaller.Bind;
    using WixToolset.Data.WindowsInstaller;
    using WixToolset.Extensibility;
    using WixToolset.Extensibility.Data;
    using WixToolset.Extensibility.Services;

    internal class MspBackend : IBackend
    {
        public IBindResult Bind(IBindContext context)
        {
            var messaging = context.ServiceProvider.GetService<IMessaging>();

            var backendHelper = context.ServiceProvider.GetService<IBackendHelper>();

            var pathResolver = context.ServiceProvider.GetService<IPathResolver>();

            var fileResolver = context.ServiceProvider.GetService<IFileResolver>();

            var extensionManager = context.ServiceProvider.GetService<IExtensionManager>();

            var resolveExtensions = extensionManager.GetServices<IResolverExtension>();

            var backendExtensions = extensionManager.GetServices<IWindowsInstallerBackendBinderExtension>();

            foreach (var extension in backendExtensions)
            {
                extension.PreBackendBind(context);
            }

            // Create transforms named in patch transforms.
            IEnumerable<PatchTransform> patchTransforms;
            {
                var command = new CreatePatchTransformsCommand(messaging, backendHelper, pathResolver, fileResolver, resolveExtensions, context.IntermediateRepresentation, context.IntermediateFolder, context.BindPaths);
                patchTransforms = command.Execute();
            }

            // Enhance the intermediate by attaching the created patch transforms.
            IEnumerable<SubStorage> subStorages;
            {
                var command = new AttachPatchTransformsCommand(messaging, backendHelper, context.IntermediateRepresentation, patchTransforms);
                subStorages = command.Execute();
            }

            // Create WindowsInstallerData with patch metdata and transforms as sub-storages
            // and create MSP from that WindowsInstallerData.
            IBindResult result = null;
            try
            {
                var command = new BindDatabaseCommand(context, backendExtensions, subStorages);
                result = command.Execute();

                foreach (var extension in backendExtensions)
                {
                    extension.PostBackendBind(result);
                }

                return result;
            }
            catch
            {
                result?.Dispose();
                throw;
            }
        }
    }
}
