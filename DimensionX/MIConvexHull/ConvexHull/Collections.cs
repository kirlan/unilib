﻿namespace MIConvexHull
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used to effectively store vertices beyond.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    sealed class VertexBuffer
    {
        VertexWrap[] items;
        int count;
        int capacity;

        /// <summary>
        /// Number of elements present in the buffer.
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Get the i-th element.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public VertexWrap this[int i]
        {
            get { return items[i]; }
        }

        /// <summary>
        /// Size matters.
        /// </summary>
        void EnsureCapacity()
        {
            if (count + 1 > capacity)
            {
                if (capacity == 0) capacity = 4;
                else capacity = 2 * capacity;
                Array.Resize(ref items, capacity);
            }
        }

        /// <summary>
        /// Adds a vertex to the buffer.
        /// </summary>
        /// <param name="item"></param>
        public void Add(VertexWrap item)
        {
            EnsureCapacity();
            items[count++] = item;
        }

        /// <summary>
        /// Sets the Count to 0, otherwise does nothing.
        /// </summary>
        public void Clear()
        {
            count = 0;
        }
    }
    
    /// <summary>
    /// A priority based linked list.
    /// </summary>
    sealed class FaceList
    {
        ConvexFaceInternal first, last;

        /// <summary>
        /// Get the first element.
        /// </summary>
        public ConvexFaceInternal First { get { return first; } }

        /// <summary>
        /// Adds the element to the beginning.
        /// </summary>
        /// <param name="face"></param>
        void AddFirst(ConvexFaceInternal face)
        {
            face.InList = true;
            this.first.Previous = face;
            face.Next = this.first;
            this.first = face;
        }

        /// <summary>
        /// Adds a face to the list.
        /// </summary>
        /// <param name="face"></param>
        public void Add(ConvexFaceInternal face)
        {
            if (face.InList)
            {
                if (this.first.FurthestDistance < face.FurthestDistance)
                {
                    Remove(face);
                    AddFirst(face);
                }
                return;
            }

            face.InList = true;

            if (first != null && first.FurthestDistance < face.FurthestDistance)
            {
                this.first.Previous = face;
                face.Next = this.first;
                this.first = face;
            }
            else
            {
                if (this.last != null)
                {
                    this.last.Next = face;
                }
                face.Previous = this.last;
                this.last = face;
                if (this.first == null)
                {
                    this.first = face;
                }
            }
        }

        /// <summary>
        /// Removes the element from the list.
        /// </summary>
        /// <param name="face"></param>
        public void Remove(ConvexFaceInternal face)
        {
            if (!face.InList) return;

            face.InList = false;

            if (face.Previous != null)
            {
                face.Previous.Next = face.Next;
            }
            else if (/*first == face*/ face.Previous == null)
            {
                this.first = face.Next;
            }

            if (face.Next != null)
            {
                face.Next.Previous = face.Previous;
            }
            else if (/*last == face*/ face.Next == null)
            {
                this.last = face.Previous;
            }

            face.Next = null;
            face.Previous = null;
        }
    }
}
