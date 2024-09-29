using System.Collections;
using System.Reflection;

namespace bdigne_api.Utils;

public class CycleHandler
{
    private HashSet<object> visited = new HashSet<object>();
    private HashSet<Type> visitedTypes = new HashSet<Type>();
    
    public IEnumerable<T> RemoveCycles<T>(IEnumerable<T> entityList)
    {
        IEnumerable<T> cleared = Enumerable.Empty<T>();
        
        foreach (T entity in entityList)
        {
            visited.Clear();
            visitedTypes.Clear();
            T withoutCycle = RemoveCycleReferences<T>(entity);
            cleared = cleared.Append(withoutCycle);
        }
        return cleared;
    }
    
    private T RemoveCycleReferences<T>(T entity, bool isRecursion = false)
    {
        if (entity == null || visited.Contains(entity))
        {
            return default; // Return null (or default value) if a cycle is detected
        }

        visited.Add(entity);
        if (!isRecursion)
        {
            visitedTypes.Add(entity.GetType());
        }

        var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => 
                (p.CanRead && p.CanWrite && p.PropertyType.IsClass && p.PropertyType != typeof(string)) //alapeset
                || visitedTypes.Contains(p.GetType()) //már találkoztunk ilyen type-al
                || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) && visitedTypes.Contains(p.PropertyType.GetGenericArguments()[0]))
            );

        properties = properties.Reverse();
        ;
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(entity);

            bool propertyValueIsHash = 
                property.PropertyType.IsGenericType 
                && property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) 
                && visitedTypes.Contains(property.PropertyType.GetGenericArguments()[0]);
            
            if (propertyValue != null && visited.Contains(propertyValue))
            {
                property.SetValue(entity, null); // Remove the cyclical reference
            }
            else if (propertyValue != null && propertyValueIsHash && propertyValue is IEnumerable enumerable)
            {
                foreach (var el in enumerable)
                {
                    if (visited.Contains(el))
                    {
                        property.SetValue(entity, null);
                    }
                }
            }
            else
            {
                // Recursively remove cycles in nested entities
                var cleanedValue = RemoveCycleReferences(propertyValue, true);
                property.SetValue(entity, cleanedValue);
            }
        }

        return entity;
    }
    
    
}