using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Helper
{
    public static class Helper
    {
        public static byte[] ToBytes(float[] embedding)
        {
            var bytes = new byte[
                embedding.Length * sizeof(float)];

            Buffer.BlockCopy(
                embedding,
                0,
                bytes,
                0,
                bytes.Length);

            return bytes;
        }
        public static float[] ToFloatArray(byte[] bytes)
        {
            var floats = new float[
                bytes.Length / sizeof(float)];

            Buffer.BlockCopy(
                bytes,
                0,
                floats,
                0,
                bytes.Length);

            return floats;
        }
        public static float CosineSimilarity(float[] vectorA,float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException(
                    "Embedding dimensions do not match.");

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return (float)(
                dotProduct /
                (Math.Sqrt(magnitudeA) *
                 Math.Sqrt(magnitudeB))
            );
        }
    }
}
