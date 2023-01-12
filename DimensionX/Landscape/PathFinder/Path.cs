using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandscapeGeneration.PathFind
{
    internal class Path<Node> : IEnumerable<Node>
    {
        public Node LastStep { get; private set; }
        public Path<Node> PreviousSteps { get; }
        public double TotalCost { get; }
        private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }
        public Path(Node start) : this(start, null, 0) { }
        public Path<Node> AddStep(Node step, double stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }
        public IEnumerator<Node> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
