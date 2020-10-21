using System;
using System.Linq;

namespace GoldDiff.Shared.LeagueOfLegends
{
    public sealed class LoLVersion : IEquatable<LoLVersion>
    {
        private const string ComponentSeparator = ".";
        
        private int[] Components { get; }

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
            Components = components ?? throw new ArgumentNullException(nameof(components));
            if (Components.Length < 1)
            {
                throw new Exception($"{nameof(components)} must contain at least 1 element!");
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LoLVersion);
        }

        public bool Equals(LoLVersion? other)
        {
            if (other == null)
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

        public override string ToString()
        {
            return string.Join(ComponentSeparator, Components);
        }
    }
}