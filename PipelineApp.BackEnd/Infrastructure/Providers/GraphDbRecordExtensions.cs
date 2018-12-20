namespace PipelineApp.BackEnd.Infrastructure.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Neo4j.Driver.V1;

    /// <summary>
    /// Extension methods for managing graph DB records.
    /// </summary>
    public static class GraphDbRecordExtensions
    {
        /// <summary>
        /// Extension method for getting a value from a <see cref="IRecord"/>
        /// by key.
        /// </summary>
        /// <typeparam name="T">The type to which the result value should be cast.</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="key">The key whose value should be retrieved.</param>
        /// <param name="defaultValue">The default value to be returned if the key does not exist on the record.</param>
        /// <returns>A value of type <code>T</code> retrieved from the record (or the default value, if the key does not exist.</returns>
        public static T GetOrDefault<T>(this IRecord record, string key, T defaultValue)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Keys.Contains(key) == false)
            {
                return defaultValue;
            }

            return record.Values.GetOrDefault(key, defaultValue);
        }

        public static T GetOrDefault<T>(this IReadOnlyDictionary<string, object> dict, string key, T defaultValue)
        {
            if (dict.TryGetValue(key, out var value))
            {
                if (value == null)
                {
                    return defaultValue;
                }

                var actualType = typeof(T);
                var underlyingType = Nullable.GetUnderlyingType(actualType);
                if (underlyingType != null)
                {
                    actualType = underlyingType;
                }

                if (actualType.GetTypeInfo().IsEnum)
                {
                    return (T)Enum.Parse(actualType, Convert.ToString(value, CultureInfo.CurrentCulture), true);
                }

                if (actualType.IsInstanceOfType(value))
                {
                    return (T)value;
                }

                if (typeof(IComparable).IsAssignableFrom(actualType))
                {
                    return (T)Convert.ChangeType(value, actualType, CultureInfo.CurrentCulture);
                }

                throw new InvalidCastException($"Unable to cast {value.GetType().FullName} to {actualType.FullName}");
            }

            return defaultValue;
        }

        public static T GetOrDefault<T>(this IDictionary<string, object> dict, string key, T defaultValue)
            where T : IConvertible
        {
            if (dict.TryGetValue(key, out var value))
            {
                if (value == null)
                {
                    return defaultValue;
                }

                var actualType = typeof(T);
                var underlyingType = Nullable.GetUnderlyingType(actualType);
                if (underlyingType != null)
                {
                    actualType = underlyingType;
                }

                if (actualType.GetTypeInfo().IsEnum)
                {
                    return (T)Enum.Parse(actualType, Convert.ToString(value), true);
                }

                if (actualType.IsInstanceOfType(value))
                {
                    return (T)value;
                }

                if (typeof(IComparable).IsAssignableFrom(actualType))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }

                throw new InvalidCastException($"Unable to cast {value.GetType().FullName} to {actualType.FullName}");
            }

            return defaultValue;
        }

        public static T GetOrDefault<T>(this IEntity entity, string key, T defaultValue)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return entity.Properties.GetOrDefault(key, defaultValue);
        }
    }
}
