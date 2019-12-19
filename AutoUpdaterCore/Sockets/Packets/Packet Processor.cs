#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - Packet Processor.cs
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

using System;
using System.Reflection;
using AutoUpdaterCore.DataStructures;

namespace AutoUpdaterCore.Sockets.Packets
{
    /// <summary>
    ///     This class encapsulates a packet processor. It stores packet handling methods in a red-black tree
    ///     for faster switching between large amounts of handlers. This technique can replace a large switch
    ///     statement for better efficiency.
    /// </summary>
    /// <typeparam name="THandlerType">The type of attribute defining handlers to be added.</typeparam>
    /// <typeparam name="TIdentity">The data type of the identity key for the handlers.</typeparam>
    /// <typeparam name="TCallbackSignature">The type of method to be stored as handlers.</typeparam>
    public sealed class PacketProcessor<THandlerType, TIdentity, TCallbackSignature>
        where THandlerType : IPacketAttribute
        where TIdentity : IComparable
        where TCallbackSignature : class
    {
        // Local-Scope Variable Declarations:
        private RedBlackTree<TIdentity, TCallbackSignature> _tree;

        /// <summary>
        ///     This class encapsulates a packet processor. It stores packet handling methods in a red-black tree
        ///     for faster switching between large amounts of handlers. This technique can replace a large switch
        ///     statement for better efficiency.
        /// </summary>
        /// <param name="handlers">The class encapsulating the packet handlers to be added to the tree.</param>
        public PacketProcessor(object handlers)
        {
            // Error check the callback type (should be void or delegate):
            if (!typeof(TCallbackSignature).IsSubclassOf(typeof(Delegate))) return;
            _tree = new RedBlackTree<TIdentity, TCallbackSignature>();
            var root = handlers.GetType();

            // Populate the processing tree: For each method in the class, check for custom attributes and 
            // the IPacketAttribute interface.
            foreach (MethodInfo method in root.GetMethods())
            foreach (var attr in method.GetCustomAttributes(true))
            {
                // If the attribute isn't null, add the method as a callback for the handler type.
                IPacketAttribute attribute = attr as IPacketAttribute;
                if (attribute != null)
                {
                    // Get the key and value for the entry in the tree:
                    TIdentity key = (TIdentity) attribute.Type;
                    TCallbackSignature value = Delegate.CreateDelegate(typeof(TCallbackSignature), null, method)
                        as TCallbackSignature;

                    // Add the callback definition:
                    if (!_tree.TryAppend(key, value))
                    {
                        // The method already exists. Combine.
                        TCallbackSignature source = _tree.TryGetValue(key);
                        source = Delegate.Combine(source as Delegate, value as Delegate) as TCallbackSignature;
                        _tree.AppendOrUpdate(key, source);
                    }
                }
            }
        }

        /// <summary>
        ///     This indexer attempts to get the callback associated with the specified key passed in the arguments. If
        ///     the tree contains the value, it will return that callback value, else it will return null.
        /// </summary>
        /// <param name="index">The packet type or parameter being checked for.</param>
        public TCallbackSignature this[TIdentity index] => _tree.TryGetValue(index);
    }
}