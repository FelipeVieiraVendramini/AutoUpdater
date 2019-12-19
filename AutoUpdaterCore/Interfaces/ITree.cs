#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - ITree.cs
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

namespace AutoUpdaterCore.Interfaces
{
    /// <summary>
    ///     This interface encapsulates the necessary methods and structures required to complete a tree
    ///     data structure. It allows for easy conversion of tree types and comparing branches.
    /// </summary>
    public interface ITree<K, V>
        where K : IComparable
    {
        /// <summary>
        ///     This method appends a key and value to the tree. It does this by starting at
        ///     the root, then following child branches until a position is found in the tree. The tree is
        ///     then resorted for the most optimized key-value pair lookups. If the key already has a value
        ///     associated with it, this method will update that value.
        /// </summary>
        void AppendOrUpdate(K key, V value);

        /// <summary>
        ///     This method appends a key and value to the tree. It does this by starting at the
        ///     root, then following child branches until a position is found in the tree. The tree is then
        ///     resorted for the most optimized key-value pair lookups. If the key already has a value
        ///     associated with it, this method will return false; else, it will return true (if the key and
        ///     value were both added to the tree).
        /// </summary>
        bool TryAppend(K key, V value);

        /// <summary>
        ///     This method returns true if the tree contains the key specified; else, it returns
        ///     false. It finds the key by starting at the root; then, if the search key is less than the
        ///     current key, it goes left - else, right.
        /// </summary>
        bool Contains(K key);

        /// <summary>
        ///     This method returns the value at a specified key if the tree contains the key specified; else, it ..
        ///     returns null. It finds the key by starting at the root; then, if the search key is less than the
        ///     current key, it goes left - else, right.
        /// </summary>
        V TryGetValue(K key);

        /// <summary>
        ///     This method returns the value at the specified key if the tree was able to remove it from the
        ///     tree structure successfully; else, it returns null. After removal, the tree is restructured.
        /// </summary>
        V TryRemove(K key);
    }

    /// <summary>
    ///     This interface encapsulates the necessary methods and data elements required to complete a
    ///     tree node. It allows for easy comparisons and conversions between nodes of different trees.
    /// </summary>
    public interface ITreeNode<K, V>
        where K : IComparable
    {
        K Key { get; set; }
        V Value { get; set; }
    }
}