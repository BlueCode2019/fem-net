﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FEM_NET
{
    public sealed class Vector : IEnumerable<double>
    {
        private readonly double[] elements;
        private double? norm;

        public int Length => elements.Length;
        public double this[int index] => elements[index];
        public double Norm
        {
            get
            {
                if (!norm.HasValue)
                    norm = CalculateNorm();
                return norm.Value;
            }
        }

        public Vector(IEnumerable<double> elements)
        {
            if (elements == null)
                throw new ArgumentException("Input is null.");

            this.elements = elements.ToArray();
            this.norm = null;
        }

        private double CalculateNorm()
        {
            double squareNorm = 0;
            foreach (var item in elements)
                squareNorm += item * item;
            return Math.Sqrt(squareNorm);
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            int length = vector1.Length;
            if (vector2.Length != length)
                throw new ArgumentException("Vector size must match.");

            double[] output = new double[length];
            for (int i = 0; i < length; i++)
                output[i] = vector1[i] + vector2[i];

            return new Vector(output);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            int length = vector1.Length;
            if (vector2.Length != length)
                throw new ArgumentException("Vector size must match.");

            double[] output = new double[length];
            for (int i = 0; i < length; i++)
                output[i] = vector1[i] - vector2[i];

            return new Vector(output);
        }

        public static Vector operator *(double scalar, Vector vector)
        {
            int length = vector.Length;

            double[] output = new double[length];
            for (int i = 0; i < length; i++)
                output[i] = scalar * vector[i];

            return new Vector(output);
        }

        public static double Dot(Vector vector1, Vector vector2)
        {
            int length = vector1.Length;
            if (vector2.Length != length)
                throw new ArgumentException("Vector size must match.");

            double output = 0;
            for (int i = 0; i < length; i++)
                output += vector1[i] * vector2[i];

            return output;
        }

        public static Vector Normalize(Vector vector)
            => (1 / vector.Norm) * vector;

        public IEnumerator<double> GetEnumerator()
            => ((IEnumerable<double>)elements).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<double>)elements).GetEnumerator();
    }
}
