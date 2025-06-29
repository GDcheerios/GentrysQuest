using System;
using osuTK;

namespace GentrysQuest.Game.Utils
{
    public static class MathBase
    {
        #region VectorMath

        /// <summary>
        /// Get the (Normalized) direction from one vector to the other.
        /// </summary>
        /// <param name="vectorFrom">The main vector</param>
        /// <param name="vectorTo">The target vector</param>
        /// <returns>The direction from the main to the target</returns>
        public static Vector2 GetDirection(Vector2 vectorFrom, Vector2 vectorTo)
        {
            Vector2 direction = vectorTo - vectorFrom;
            direction.Normalize();
            return direction;
        }

        /// <summary>
        /// Get the degrees from an angle from one vector to the other.
        /// </summary>
        /// <param name="vector1">Main vector</param>
        /// <param name="vector2">Target vector</param>
        /// <returns>The angle degrees.</returns>
        public static float GetAngle(Vector2 vector1, Vector2 vector2)
        {
            Vector2 direction = GetDirection(vector1, vector2);
            double angleRadians = Math.Atan2(direction.Y, direction.X);
            return (float)Math.Round(GetDegress(angleRadians));
        }

        /// <summary>
        /// Gets the magnitude of a vector.
        /// </summary>
        /// <param name="vector">The vector to perform the math on</param>
        /// <returns>The magnitude</returns>
        public static double GetMagnitude(Vector2 vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        /// <summary>
        /// Gets the dot product of two vectors.
        /// </summary>
        /// <returns>The dot product</returns>
        public static double GetDot(Vector2 vector1, Vector2 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
        }

        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="value">Radian value</param>
        /// <returns>The degrees</returns>
        public static double GetDegress(double value)
        {
            return value * (180 / Math.PI);
        }

        public static Vector2 GetAngleToVector(double degrees, bool negative = false)
        {
            double angleRadians = degrees * Math.PI / 180;
            double x = Math.Cos(angleRadians);
            double y = Math.Sin(angleRadians);
            if (negative) return new Vector2((float)-x, (float)-y);

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// Rotates a vector
        /// </summary>
        /// <param name="vector">the vector</param>
        /// <param name="degrees">the degrees</param>
        /// <returns>the rotated vector</returns>
        public static Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float radians = MathF.PI * degrees / 180.0f;

            float newX = vector.X * MathF.Cos(radians) - vector.Y * MathF.Sin(radians);
            float newY = vector.X * MathF.Sin(radians) + vector.Y * MathF.Cos(radians);

            return new Vector2(newX, newY);
        }

        public static double GetDistance(Vector2 firstPos, Vector2 secondPos)
        {
            return Math.Sqrt(Math.Pow(secondPos.X - firstPos.X, 2) + Math.Pow(secondPos.Y - firstPos.Y, 2));
        }

        #endregion

        #region RandomMath

        public static double GetPercent(double value, double percent) => value * (percent * 0.01f);
        public static int RandomInt(int min, int max) => Random.Shared.Next(min, max + 1);
        public static int RandomInt(int max) => Random.Shared.Next(max + 1);
        public static float RandomFloat(float min, float max) => min + Random.Shared.NextSingle() * (max - min);
        public static float RandomFloat(float max) => Random.Shared.NextSingle() * max;
        public static float RandomFloat() => Random.Shared.NextSingle();
        public static bool RandomBool() => Convert.ToBoolean(Random.Shared.Next(2));
        public static bool IsChanceSuccessful(int passingValue, int chance) => (passingValue >= RandomInt(chance));
        public static bool IsChanceSuccessful(float passingValue) => passingValue >= RandomFloat();
        public static int RandomChoice(int size) => Random.Shared.Next(size);

        #endregion

        #region TimeMath

        public static double SecondToMs(double input)
        {
            return input * 1000;
        }

        #endregion

        #region UnitConversionMath

        private const int ENTITY_INCHES = 12;

        private const int INCHES_TO_MILES = 63360;

        private const int INCHES_TO_FEET = 12;

        public static int GetMilesToPixels(double miles) => (int)(miles * (INCHES_TO_MILES * (ENTITY_INCHES * 0.1)));
        public static int GetFeetToPixels(double feet) => (int)(feet * (INCHES_TO_FEET * (ENTITY_INCHES * 0.1)));

        #endregion

        /// <summary>
        /// Get the star rating based off of difficulty
        /// </summary>
        /// <param name="difficulty">The difficulty</param>
        /// <returns>the star rating</returns>
        public static int GetStarRating(int difficulty)
        {
            // difficulty starts at zero so we add one
            int currentStarRating = difficulty + 1;
            if (IsChanceSuccessful(0.15f)) currentStarRating++;
            if (IsChanceSuccessful(0.03f)) currentStarRating++;
            return currentStarRating;
        }

        public static float CalculateShortestRotation(float currentRotation, float targetRotation)
        {
            // Normalize angles to [0, 360)
            currentRotation = (currentRotation % 360 + 360) % 360;
            targetRotation = (targetRotation % 360 + 360) % 360;

            // Determine shortest rotation direction and angle
            float rotationDifference = targetRotation - currentRotation;

            if (rotationDifference > 180)
                rotationDifference -= 360;
            else if (rotationDifference < -180)
                rotationDifference += 360;

            return currentRotation + rotationDifference;
        }
    }
}
