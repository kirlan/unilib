using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleVectors;
using Random;

namespace LandscapeGeneration
{
    // Adapated from java source by Herman Tulleken
    // http://www.luma.co.za/labs/2008/02/27/poisson-disk-sampling/

    // The algorithm is from the "Fast Poisson Disk Sampling in Arbitrary Dimensions" paper by Robert Bridson
    // http://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

    public static class UniformPoissonDiskSampler
    {
        public const int DefaultPointsPerIteration = 30;

        static readonly float SquareRootTwo = (float)Math.Sqrt(2);

        struct Settings
        {
            public SimpleVector3d TopLeft, LowerRight, Center;
            public SimpleVector3d Dimensions;
            public float? RejectionSqDistance;
            public float MinimumDistance;
            public float CellSize;
            public int GridWidth, GridHeight;
        }

        struct State
        {
            public SimpleVector3d[,] Grid;
            public List<SimpleVector3d> ActivePoints, Points;
        }

        public static List<SimpleVector3d> SampleCircle(SimpleVector3d center, float radius, float minimumDistance)
        {
            return SampleCircle(center, radius, minimumDistance, DefaultPointsPerIteration);
        }
        public static List<SimpleVector3d> SampleCircle(SimpleVector3d center, float radius, float minimumDistance, int pointsPerIteration)
        {
            return Sample(center - new SimpleVector3d(radius), center + new SimpleVector3d(radius), radius, minimumDistance, pointsPerIteration);
        }

        public static List<SimpleVector3d> SampleRectangle(SimpleVector3d topLeft, SimpleVector3d lowerRight, float minimumDistance)
        {
            return SampleRectangle(topLeft, lowerRight, minimumDistance, DefaultPointsPerIteration);
        }
        public static List<SimpleVector3d> SampleRectangle(SimpleVector3d topLeft, SimpleVector3d lowerRight, float minimumDistance, int pointsPerIteration)
        {
            return Sample(topLeft, lowerRight, null, minimumDistance, pointsPerIteration);
        }

        static List<SimpleVector3d> Sample(SimpleVector3d topLeft, SimpleVector3d lowerRight, float? rejectionDistance, float minimumDistance, int pointsPerIteration)
        {
            var settings = new Settings
            {
                TopLeft = topLeft,
                LowerRight = lowerRight,
                Dimensions = lowerRight - topLeft,
                Center = (topLeft + lowerRight) / 2,
                CellSize = minimumDistance / SquareRootTwo,
                MinimumDistance = minimumDistance,
                RejectionSqDistance = rejectionDistance == null ? null : rejectionDistance * rejectionDistance
            };
            settings.GridWidth = (int)(settings.Dimensions.X / settings.CellSize) + 1;
            settings.GridHeight = (int)(settings.Dimensions.Y / settings.CellSize) + 1;

            var state = new State
            {
                Grid = new SimpleVector3d[settings.GridWidth, settings.GridHeight],
                ActivePoints = new List<SimpleVector3d>(),
                Points = new List<SimpleVector3d>()
            };

            AddFirstPoint(ref settings, ref state);

            while (state.ActivePoints.Count != 0)
            {
                var listIndex = Rnd.Get(state.ActivePoints.Count);

                var point = state.ActivePoints[listIndex];
                var found = false;

                for (var k = 0; k < pointsPerIteration; k++)
                    found |= AddNextPoint(point, ref settings, ref state);

                if (!found)
                    state.ActivePoints.RemoveAt(listIndex);
            }

            return state.Points;
        }

        static void AddFirstPoint(ref Settings settings, ref State state)
        {
            var added = false;
            while (!added)
            {
                var xr = settings.TopLeft.X + Rnd.Get(settings.Dimensions.X);

                var yr = settings.TopLeft.Y + Rnd.Get(settings.Dimensions.Y);

                var p = new SimpleVector3d((float)xr, (float)yr, 0);
                if (settings.RejectionSqDistance != null && SimpleVector3d.DistanceSquared(settings.Center, p) > settings.RejectionSqDistance)
                    continue;
                added = true;

                var index = Denormalize(p, settings.TopLeft, settings.CellSize);

                state.Grid[(int)index.X, (int)index.Y] = p;

                state.ActivePoints.Add(p);
                state.Points.Add(p);
            }
        }

        static bool AddNextPoint(SimpleVector3d point, ref Settings settings, ref State state)
        {
            var found = false;
            var q = GenerateRandomAround(point, settings.MinimumDistance);

            if (q.X >= settings.TopLeft.X && q.X < settings.LowerRight.X &&
                q.Y > settings.TopLeft.Y && q.Y < settings.LowerRight.Y &&
                (settings.RejectionSqDistance == null || SimpleVector3d.DistanceSquared(settings.Center, q) <= settings.RejectionSqDistance))
            {
                var qIndex = Denormalize(q, settings.TopLeft, settings.CellSize);
                var tooClose = false;

                for (var i = (int)Math.Max(0, qIndex.X - 2); i < Math.Min(settings.GridWidth, qIndex.X + 3) && !tooClose; i++)
                    for (var j = (int)Math.Max(0, qIndex.Y - 2); j < Math.Min(settings.GridHeight, qIndex.Y + 3) && !tooClose; j++)
                        if (state.Grid[i, j].HasValue && SimpleVector3d.Distance(state.Grid[i, j].Value, q) < settings.MinimumDistance)
                            tooClose = true;

                if (!tooClose)
                {
                    found = true;
                    state.ActivePoints.Add(q);
                    state.Points.Add(q);
                    state.Grid[(int)qIndex.X, (int)qIndex.Y] = q;
                }
            }
            return found;
        }

        static SimpleVector3d GenerateRandomAround(SimpleVector3d center, float minimumDistance)
        {
            var radius = minimumDistance + Rnd.Get(minimumDistance);

            var angle = Rnd.Get(MathHelper.TwoPi);

            var newX = radius * Math.Sin(angle);
            var newY = radius * Math.Cos(angle);

            return new SimpleVector3d((float)(center.X + newX), (float)(center.Y + newY), 0);
        }

        static SimpleVector3d Denormalize(SimpleVector3d point, SimpleVector3d origin, double cellSize)
        {
            return new SimpleVector3d((int)((point.X - origin.X) / cellSize), (int)((point.Y - origin.Y) / cellSize), 0);
        }
    }

    public static class MathHelper
    {
        public const float Pi = (float)Math.PI;
        public const float HalfPi = (float)(Math.PI / 2);
        public const float TwoPi = (float)(Math.PI * 2);
    }
}
