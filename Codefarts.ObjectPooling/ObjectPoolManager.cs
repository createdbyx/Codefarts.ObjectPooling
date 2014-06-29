// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.ObjectPooling
{
    using System;

    /// <summary>
    /// Provides a generic object pooling manager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPoolManager<T> where T : class
    {
        /// <summary>
        /// Holds a reference to a singleton instance.
        /// </summary>
        private static ObjectPoolManager<T> instance;

        /// <summary>
        /// Used to track the number of items that have been pushed to the pool.
        /// </summary>
        private int count;

        /// <summary>
        /// Holds the pooled item references.
        /// </summary>
        private T[] cachedItems = new T[10000];


        /// <summary>
        /// Gets or sets the creation callback.
        /// </summary>
        public Func<T> CreationCallback { get; set; }

        /// <summary>
        /// Gets the number of items in the pool.
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Pops a item from the pool.
        /// </summary>
        /// <returns>A pooled object reference.</returns>
        /// <exception cref="System.NullReferenceException">'CreationCallback' property must be set if you try to pop a item and there are no items available.</exception>
        public T Pop()
        {
            // lock here to prevent treading conflicts with array manipulation
            lock (this.cachedItems)
            {
                // check if there are any pooled objects
                if (this.count < 1)
                {
                    // check if creation callback is null
                    if (this.CreationCallback == null)
                    {
                        throw new NullReferenceException("'CreationCallback' property must be set if you try to pop a item and there are no items available.");
                    }

                    // there are no available objects so create a new one.
                    return this.CreationCallback();
                }

                // reduce the count
                this.count--;

                // retrieve the item and return it
                return this.cachedItems[this.count];
            }
        }

        /// <summary>
        /// Pushes the specified value.
        /// </summary>
        /// <param name="value">The value to push into the pool.</param>
        public void Push(T value)
        {
            // lock here to prevent treading conflicts with array manipulation
            lock (this.cachedItems)
            {
                // update the count
                this.count++;

                // if we need more room for storage increase the size of the cache array
                if (this.count > this.cachedItems.Length)
                {
                    Array.Resize(ref this.cachedItems, this.cachedItems.Length * 2);
                }

                // store the value 
                this.cachedItems[this.count - 1] = value;
            }
        }

        /// <summary>
        /// Gets the singleton instance of the class.
        /// </summary>
        public static ObjectPoolManager<T> Instance
        {
            get
            {
                return instance ?? (instance = new ObjectPoolManager<T>());
            }
        }
    }
}
