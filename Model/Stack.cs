//-----------------------------------------------------------------------
// <copyright file="Stack.cs" company="Perspektyva">
//     Copyright (c) Perspektyva All rights reserved.  
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.  
// </copyright>
//-----------------------------------------------------------------------
namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a quantity of a certain type of goods.
    /// </summary>
    public class Stack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stack" /> class.
        /// </summary>
        /// <param name="good">The good that this stack will contain.</param>
        /// <param name="quantity">The quantity of good that this stack will contain.</param>
        public Stack(Good good, double quantity)
        {
            if (quantity < 0.0)
            {
                throw new ArgumentException(string.Format(
                    "Cannot create a stack of negative quantity: '{0}'",
                    quantity));
            }

            if (good == null)
            {
                throw new ArgumentNullException(
                    "good",
                    "Cannot create a stack without specifying a good.");
            }

            this.Good = good;
            this.Quantity = quantity;
        }

        /// <summary>
        /// Gets the good contained within this stack.
        /// </summary>
        public Good Good { get; private set; }

        /// <summary>
        /// Gets the amount of goods in this stack.
        /// </summary>
        public double Quantity { get; private set; }

        /// <summary>
        /// Splits the stack in two.
        /// </summary>
        /// <param name="stack">The stack that is to be split.</param>
        /// <param name="quantityOfItem1">The quantity of the first stack.</param>
        /// <returns>A tuple, where the firs stack has quantity of 'quantityOfItem1' and the second stack has the remainder.</returns>
        public static Tuple<Stack, Stack> SplitStack(
            Stack stack, double quantityOfItem1)
        {
            if (stack == null)
            {
                return new Tuple<Stack, Stack>(null, null);
            }

            if (quantityOfItem1 < 0.0)
            {
                throw new ArgumentException(string.Format(
                    "Requested quantity cannot be negative: '{0}'",
                    quantityOfItem1));
            }

            if (quantityOfItem1 > stack.Quantity)
            {
                throw new ArgumentException(string.Format(
                    "Requested quantity cannot exceed stack size: '{0}' " +
                    "cannot be extracted from '{1}'",
                    quantityOfItem1,
                    stack.Quantity));
            }

            var normalizedQuantityOfItem1 =
                Math.Min(stack.Quantity, quantityOfItem1);
            var quantityOfItem2 =
                stack.Quantity - normalizedQuantityOfItem1;
            return Tuple.Create<Stack, Stack>(
                new Stack(stack.Good, normalizedQuantityOfItem1),
                new Stack(stack.Good, quantityOfItem2));
        }

        /// <summary>
        /// Merges two stacks together. Two null stacks produce a null stack.
        /// A null stack and a non-null stack produce the non-null stack.
        /// </summary>
        /// <param name="stack1">The first stack that needs to be merged. Can be null.</param>
        /// <param name="stack2">The second stack that needs to be merged. Can be null.</param>
        /// <returns>The resulting stack.</returns>
        /// <exception cref="ArgumentException">Thrown if stacks are not of the same good.</exception>
        public static Stack MergeStacks(Stack stack1, Stack stack2)
        {
            if (stack1 != null && stack2 == null)
            {
                return stack1;
            }

            if (stack2 != null && stack1 == null)
            {
                return stack2;
            }

            if (stack1 != null && stack2 != null)
            {
                if (stack1.Good != stack2.Good)
                {
                    throw new ArgumentException(string.Format(
                        "Cannot merge two different type stacks: '{0}' and '{1}'",
                        stack1.Good.Name,
                        stack2.Good.Name));
                }

                return new Stack(stack1.Good, stack1.Quantity + stack2.Quantity);
            }

            return null;
        }

        /// <summary>
        /// Tests whether this stack is equivalent to the other.
        /// </summary>
        /// <param name="obj">The other stack.</param>
        /// <returns>returns true if both stacks are equal. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Stack))
            {
                return false;
            }

            Stack other = (Stack)obj;
            return
                object.Equals(this.Good, other.Good) &&
                this.Quantity == other.Quantity;
        }

        /// <summary>
        /// Gets the has code of this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return
                this.Good.GetHashCode() ^ this.Quantity.GetHashCode();
        }
    }
}
