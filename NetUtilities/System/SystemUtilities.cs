﻿using NetUtilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SystemUtilities
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        /// Creates a <see cref="string"/> with the providen array.
        /// </summary>
        /// <param name="charArray">The array.</param>
        /// <returns></returns>
        [return: NotNull]
        public static string AsString(this char[] charArray)
            => new string(charArray);

        /// <summary>
        /// Creates a <see cref="string"/> with the provided span.
        /// </summary>
        /// <param name="charSpan">The span.</param>
        /// <returns></returns>
        [return: NotNull]
        public static string AsString(this ReadOnlySpan<char> charSpan)
            => new string(charSpan);

        /// <summary>
        /// Gets the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        [return: NotNull]
        public static string SubstringBetween([NotNull]this string input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start == 0) 
                return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end == -1 || start > end) 
                return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[start..end];
        }

        /// <summary>
        /// Gets the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        [return: NotNull]
        public static string SubstringBetween([NotNull]this MutableString input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start == 0) 
                return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end == -1 || start > end) 
                return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[start..end];
        }

        /// <summary>
        /// Gets an array of substrings between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        [return: NotNull]
        public static string[] SubstringsBetween([NotNull]this MutableString input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            var indexes = input.IndexesOf(leftBound, startIndex);

            if (indexes.Length == 0) 
                return Array.Empty<string>();

            var result = new string[indexes.Length];

            for (int resultIndex = 0; resultIndex < indexes.Length; resultIndex++)
                result[resultIndex] = input.SubstringBetween(leftBound, rightBound, indexes[resultIndex], includeBounds);

            return result;
        }

        /// <summary>
        /// Gets an array of substrings between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        [return: NotNull]
        public static string[] SubstringsBetween([NotNull]this string input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            var indices = input.IndexesOf(leftBound, startIndex, input.Length);

            if (indices.Count == 0) 
                return Array.Empty<string>();

            var result = new string[indices.Count];

            for (int resultIndex = 0; resultIndex < indices.Count; resultIndex++)
                result[resultIndex] = input.SubstringBetween(leftBound, rightBound, indices[resultIndex], includeBounds);

            return result;
        }

        /// <summary>
        /// Gets all the indexes of the providen <see cref="char"/>, this method returns an empty array if no indexes are found.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [return: NotNull]
        public static IReadOnlyList<int> IndexesOf([NotNull]this string input, char value, int startIndex, int count)
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            if (count == 0) 
                return Array.Empty<int>();

            var currentIndex = input.IndexOf(value, startIndex, count);
            var result = new List<int>();

            while (currentIndex != -1)
            {
                result.Add(currentIndex);
                currentIndex = input.IndexOf(value, currentIndex + 1);
            }

            return result;
        }

        /// <summary>
        /// Creates a <see cref="MutableString"/> with the <see cref="string"/> provided.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>A mutable string.</returns>
        [return: NotNull]
        public static MutableString ToMutable(this string value)
            => new MutableString(value);

        /// <summary>
        /// Returns a new <see cref="string"/> with the reversed characters.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the provided string is <see langword="null"/></exception>
        /// <param name="str">The string to be reversed.</param>
        /// <returns>A reversed string.</returns>
        [return: NotNull]
        public static string Reverse([NotNull]this string str)
            => string.Create(Ensure.NotNull(str, nameof(str)).Length, str,
                (span, state) =>
                {
                    for (int i = 0, j = span.Length - 1; i < span.Length; i++, j--)
                        span[i] = state[j];
                });

        /// <summary>
        /// Returns a boolean indicating if both strings are similar content-wise (case-insensitive by default)
        /// </summary>
        /// <param name="str">The string to be compared</param>
        /// <param name="other">The string to look for similarities</param>
        /// <param name="comparison">The comparison to be used to determine if they are alike</param>
        /// <returns>A boolean indicating if the string are alike.</returns>
        public static bool Like([NotNull]this string str, [NotNull]string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
            => Ensure.NotNull(str, nameof(str)).Equals(Ensure.NotNull(other, nameof(other)), comparison);

        /// <summary>
        /// Indicates if the string contains any of the words provided.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="words">The words.</param>
        /// <returns><see langword="true"/> if the string contains any of the providen words, otherwise <see langword="false"=""/></returns>
        public static bool ContainsAny([NotNull]this string str, [NotNull]params string[] words)
        {
            Ensure.NotNull(str, nameof(str));
            Ensure.NotNull(words, nameof(words));

            foreach (var word in words)
            {
                if (str.Contains(word))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Indicates if the string contains all of the words provided.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="words">The words.</param>
        /// <returns><see langword="true"/> if the string contains all of the providen words, otherwise <see langword="false"=""/></returns>
        public static bool ContainsAll([NotNull]this string str, [NotNull]params string[] words)
        {
            Ensure.NotNull(str, nameof(str));
            Ensure.NotNull(words, nameof(words));

            foreach (var word in words)
            {
                if (!str.Contains(word))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Quotes the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        [return: NotNull]
        public static string Quote([NotNull]this string str)
            => string.Create(Ensure.NotNull(str, nameof(str)).Length + 2, str, (span, state) =>
            {
                span[0] = '"';
                span[^1] = '"';

                for (var index = 1; index < span.Length - 1; index++)
                    span[index] = state[index - 1];
            });

        /// <summary>
        /// Checks if the runtime type of obj is the targeted one.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsType<T>(object? obj)
            => IsType(obj, typeof(T));

        /// <summary>
        /// Checks if the runtime type of obj is the targeted one.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsType(object? obj, Type targetType)
            => obj?.GetType() == targetType;

        /// <summary>
        /// Gets each flags of the given <typeparamref name="TEnum"/> instance.
        /// </summary>
        /// <typeparam name="TEnum">Type of the <see cref="Enum"/></typeparam>
        /// <param name="enum">The <see cref="Enum"/> object</param>
        /// <returns>A <see cref="{TEnum}"/>[] containing all the flags found in the current instance</returns>
        [return: NotNull]
        public static TEnum[] GetFlags<TEnum>(this TEnum @enum) where TEnum : unmanaged, Enum
        {
            EnumHelper<TEnum>.ThrowIfNotFlagEnum();

            var values = EnumHelper<TEnum>.Values;
            var length = 0;
            Span<TEnum> span = stackalloc TEnum[values.Length];

            foreach (var value in values)
            {
                if (@enum.HasFlag(value))
                    span[length++] = value;
            }

            return span[..length].ToArray();
        }

        private static class EnumHelper<TEnum> where TEnum : unmanaged, Enum
        {
            private static readonly bool _isFlag = typeof(TEnum).GetCustomAttribute<FlagsAttribute>() is object;

            public static TEnum[] Values { get; } = (TEnum[])Enum.GetValues(typeof(TEnum));

            public static void ThrowIfNotFlagEnum()
            {
                if (!_isFlag)
                    throw new InvalidOperationException("This method can only be used on enums with FlagsAttributes");
            }
        }
    }
}