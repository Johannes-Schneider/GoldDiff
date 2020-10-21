using System;
using System.Linq;
using Newtonsoft.Json;

namespace GoldDiff.Shared.LeagueOfLegends
{
    public sealed class LoLVersion : IEquatable<LoLVersion>
    {
        private const string ComponentSeparator = ".";
        
        [JsonProperty]
        private int[] Components { get; }
        
        public static LoLVersion Zero => new LoLVersion(0);

        public static bool TryParse(string? input, out LoLVersion? version)
        {
            try
            {
                var components = input?.Split(new[] {ComponentSeparator}, StringSplitOptions.None) ?? throw new ArgumentNullException(nameof(input));
                version = new LoLVersion(components.Select(component => Convert.ToInt32(component)).ToArray());
                return true;
            }
            catch
            {
                version = null;
                return false;
            }
        }

        public LoLVersion(params int[]? components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            if (components.Length < 1)
            {
                throw new AggregateException($"{nameof(components)} must contain at least 1 element!");
            }

            if (components.Any(component => component < 0))
            {
                throw new ArgumentException($"{nameof(components)} must not contain any negative elements!");
            }

            Components = components;
        }
        
        public override string ToString()
        {
            return string.Join(ComponentSeparator, Components);
        }

    #region IEquatable

        public override bool Equals(object? obj)
        {
            return Equals(obj as LoLVersion);
        }

        public bool Equals(LoLVersion? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (Components.Length != other.Components.Length)
            {
                return false;
            }

            return !Components.Where((component, componentIndex) => component != other.Components[componentIndex]).Any();
        }

        public override int GetHashCode()
        {
            return Components.GetHashCode();
        }

    #endregion

    #region Operators

        public static bool operator ==(LoLVersion? a, LoLVersion? b)
        {
            return a?.Equals(b) == true;
        }

        public static bool operator !=(LoLVersion? a, LoLVersion? b)
        {
            return a?.Equals(b) == false;
        }
        
        public static bool operator >(LoLVersion? a, LoLVersion? b)
        {
            if (ReferenceEquals(a, null))
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ReferenceEquals(b, null))
            {
                throw new ArgumentNullException(nameof(b));
            }

            for (var componentIndex = 0; componentIndex < Math.Max(a.Components.Length, b.Components.Length); ++componentIndex)
            {
                var componentA = componentIndex < a.Components.Length ? a.Components[componentIndex] : 0;
                var componentB = componentIndex < b.Components.Length ? b.Components[componentIndex] : 0;

                if (componentA > componentB)
                {
                    return true;
                }

                if (componentA < componentB)
                {
                    return false;
                }
            }

            return false;
        }
        
        public static bool operator >=(LoLVersion? a, LoLVersion? b)
        {
            if (ReferenceEquals(a, null))
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ReferenceEquals(b, null))
            {
                throw new ArgumentNullException(nameof(b));
            }

            for (var componentIndex = 0; componentIndex < Math.Max(a.Components.Length, b.Components.Length); ++componentIndex)
            {
                var componentA = componentIndex < a.Components.Length ? a.Components[componentIndex] : 0;
                var componentB = componentIndex < b.Components.Length ? b.Components[componentIndex] : 0;

                if (componentA > componentB)
                {
                    return true;
                }

                if (componentA < componentB)
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool operator <(LoLVersion? a, LoLVersion? b)
        {
            if (ReferenceEquals(a, null))
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ReferenceEquals(b, null))
            {
                throw new ArgumentNullException(nameof(b));
            }

            for (var componentIndex = 0; componentIndex < Math.Max(a.Components.Length, b.Components.Length); ++componentIndex)
            {
                var componentA = componentIndex < a.Components.Length ? a.Components[componentIndex] : 0;
                var componentB = componentIndex < b.Components.Length ? b.Components[componentIndex] : 0;

                if (componentA < componentB)
                {
                    return true;
                }

                if (componentA > componentB)
                {
                    return false;
                }
            }

            return false;
        }
        
        public static bool operator <=(LoLVersion? a, LoLVersion? b)
        {
            if (ReferenceEquals(a, null))
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ReferenceEquals(b, null))
            {
                throw new ArgumentNullException(nameof(b));
            }

            for (var componentIndex = 0; componentIndex < Math.Max(a.Components.Length, b.Components.Length); ++componentIndex)
            {
                var componentA = componentIndex < a.Components.Length ? a.Components[componentIndex] : 0;
                var componentB = componentIndex < b.Components.Length ? b.Components[componentIndex] : 0;

                if (componentA < componentB)
                {
                    return true;
                }

                if (componentA > componentB)
                {
                    return false;
                }
            }

            return true;
        }

    #endregion
    }
}