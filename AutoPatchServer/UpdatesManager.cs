#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - UpdatesManager.cs
// 
// Description: <Write a description for this file>
// 
// Colaborators who worked in this file:
// Felipe Vieira Vendramini
// 
// Developed by:
// Felipe Vieira Vendramini <service@ftwmasters.com.br>
// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoUpdaterCore.Interfaces;

namespace AutoPatchServer
{
    public static class UpdatesManager
    {
        public const int GAME_UPDATE_MIN = 1000;
        public const int GAME_UPDATE_MAX = 9999;
        public const int UPDATER_VERSION_MIN = 10000;
        public const int UPDATER_VERSION_MAX = 99999;

        private static ConcurrentDictionary<int, PatchStructure> m_patchLibrary =
            new ConcurrentDictionary<int, PatchStructure>();

        public static void AddPatch(PatchStructure patch, bool isLoading)
        {
            if (patch.To >= UPDATER_VERSION_MIN)
                patch.IsGameUpdate = true;

            if (m_patchLibrary.ContainsKey(patch.From))
            {
                if (m_patchLibrary[patch.From].To < patch.To)
                {
                    int addTo = RemovePatch(patch.From, true);
                    m_patchLibrary.TryAdd(patch.From, patch);

                    if (!isLoading)
                    {
                        if (patch.From > 0)
                        {
                            int count = int.Parse(Kernel.MyXml.GetValue("Config", "BundlePatches", "Count"));
                            Kernel.MyXml.AddNewNode("", $"Patch{count}", "Config", "BundlePatches");
                            Kernel.MyXml.AddNewNode(patch.From.ToString(), "From", "Config", "BundlePatches", $"Patch{count}");
                            Kernel.MyXml.AddNewNode(patch.To.ToString(), "To", "Config", "BundlePatches", $"Patch{count}");
                            Kernel.MyXml.AddNewNode(patch.FileName, "FileName", "Config", "BundlePatches", $"Patch{count++}");
                            Kernel.MyXml.ChangeValue(patch.FileName, "Config", "BundlePatches", $"Patch{count}", "FileName");
                        }
                        else
                        {
                            Kernel.MyXml.ChangeValue(patch.To.ToString(), "Config", "AllowedPatches", $"Patch{addTo}");
                        }
                    }
                }
            }
            else
            {
                m_patchLibrary.TryAdd(patch.To, patch);

                if (!isLoading)
                {
                    if (patch.From > 0)
                    {
                        int count = int.Parse(Kernel.MyXml.GetValue("Config", "BundlePatches", "Count"));
                        Kernel.MyXml.AddNewNode("", $"Patch{count}", "Config", "BundlePatches");
                        Kernel.MyXml.AddNewNode(patch.From.ToString(), $"Patch{count}", "Config", "BundlePatches", "From");
                        Kernel.MyXml.AddNewNode(patch.To.ToString(), $"Patch{count}", "Config", "BundlePatches", "To");
                        Kernel.MyXml.AddNewNode(patch.FileName, $"Patch{count}", "Config", "BundlePatches", "FileName");
                        Kernel.MyXml.ChangeValue(patch.FileName, "Config", "BundlePatches", $"Patch{count}", "FileName");
                    }
                    else
                    {
                        int count = int.Parse(Kernel.MyXml.GetValue("Config", "AllowedPatches", "Count"));
                        Kernel.MyXml.AddNewNode(patch.To.ToString(), $"Patch{count++}", "Config", "AllowedPatches");
                        Kernel.MyXml.ChangeValue(count.ToString(), "Config", "AllowedPatches", "Count");
                    }
                }
            }
        }

        /// <summary>
        ///     Removes an patch from the dictionary and from the XML.
        /// </summary>
        /// <param name="version">The version to be removed.</param>
        /// <param name="replace">If it's going to replace the XML with another patch, we wont delete the node.</param>
        /// <returns>-1 if the patch doesn't exist.</returns>
        public static int RemovePatch(int version, bool replace)
        {
            int result = 0;
            if (m_patchLibrary.TryRemove(version, out var removed))
                result = removed.Order;
            else result = -1;

            if (!replace)
            {
                if (removed.From > 0)
                    Kernel.MyXml.DeleteNode("Config", "BundlePatches", $"Patch{removed.Order}");
                else
                    Kernel.MyXml.DeleteNode("Config", "AllowedPatches", $"Patch{removed.Order}");
            }

            return result;
        }

        public static int LatestVersion(bool isUpdate)
        {
            if (isUpdate)
                return m_patchLibrary.Values.Where(x => x.IsGameUpdate == !isUpdate).Max(x => x.To);
            return m_patchLibrary.Values.Where(x => x.IsGameUpdate == isUpdate).Max(x => x.To);
        }

        public static List<PatchStructure> GetDownloadList(int actualVersion)
        {
            List<PatchStructure> result = new List<PatchStructure>();
            bool isUpdater = actualVersion >= UPDATER_VERSION_MIN && actualVersion <= UPDATER_VERSION_MAX;
            var possibleUpdates = m_patchLibrary.Values.Where(x => x.IsGameUpdate == isUpdater && x.To > actualVersion)
                .OrderBy(x => x.To);
            int latestVersion = 0;
            int currently = latestVersion = LatestVersion(isUpdater);

            foreach (PatchStructure patch in possibleUpdates.OrderByDescending(x => x.To))
            {
                if (latestVersion <= actualVersion)
                    break;

                if (patch.To > currently)
                    continue;

                if (patch.From == 0)
                    currently--;
                else
                    currently = patch.From;
            }

            return result.OrderBy(x => x.To).ToList();
        }
    }
}