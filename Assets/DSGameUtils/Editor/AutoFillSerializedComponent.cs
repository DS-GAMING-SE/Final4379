using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WeightlessWayfare.EntityStates;

namespace DSGameUtils
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AutoFillSerializedComponent : Attribute
    {
        public AutoFillSerializedComponent()
        {
        }
        static AutoFillSerializedComponent()
        {
            ObjectFactory.componentWasAdded -= OnComponentAdded;
            ObjectFactory.componentWasAdded += OnComponentAdded;
        }
        private static void OnComponentAdded(Component component)
        {

        }
        private static void AutoFill(GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents(typeof(Component));
            foreach (var component in components)
            {
                IEnumerable<FieldInfo> fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (typeof(Component).IsAssignableFrom(field.FieldType))
                    {
                        ConfigSerializeFieldAttribute attribute = field.GetCustomAttribute<ConfigSerializeFieldAttribute>();
                        if (attribute != null && (Component)(field.GetValue(null)))
                        {
                            field.SetValue(component, gameObject.GetComponent(field.FieldType));
                        }
                    }
                }
            }
        }
    }
}
