using HomeNet.Cores.Devices;

using Microsoft.AspNetCore.Components;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace HomeNet.Applications.BlazorApp.Components.Devices;

public partial class DeviceActionComponent
{
    private IDeviceAction? _initializedAction;

    [Parameter, EditorRequired]
    public IDeviceAction Action { get; set; } = default!;

    private object? Parameters { get; set; }
    private IReadOnlyList<PropertyInfo> ParameterProperties { get; set; } = Array.Empty<PropertyInfo>();
    private Dictionary<string, string> ValidationErrors { get; } = new();
    private string? FormError { get; set; }

    protected override void OnParametersSet()
    {
        if (ReferenceEquals(Action, _initializedAction))
        {
            return;
        }

        _initializedAction = Action;
        ValidationErrors.Clear();
        FormError = null;
        Parameters = null;
        ParameterProperties = Array.Empty<PropertyInfo>();

        if (!Action.RequiresParameters)
        {
            return;
        }

        var parameterType = Action.ParametersType;
        if (parameterType is null || parameterType.IsAbstract || parameterType.IsInterface)
        {
            FormError = "Le type de paramètres doit être une classe concrète avec des propriétés modifiables.";
            return;
        }

        try
        {
            Parameters = Activator.CreateInstance(parameterType);
        }
        catch (MissingMethodException)
        {
            FormError = "Le type de paramètres doit avoir un constructeur public sans argument.";
            return;
        }
        catch (MemberAccessException)
        {
            FormError = "Le type de paramètres doit avoir un constructeur public sans argument.";
            return;
        }
        if (Parameters is not null)
        {
            var publicProperties = parameterType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => property.CanRead && property.SetMethod?.IsPublic == true && property.GetIndexParameters().Length == 0)
                .ToArray();
            ParameterProperties = publicProperties
                .Where(IsSupportedProperty)
                .ToArray();

            if (ParameterProperties.Count == 0)
            {
                FormError = "Le type de paramètres doit exposer au moins une propriété modifiable de type simple.";
            }
            else if (ParameterProperties.Count != publicProperties.Length)
            {
                FormError = "Toutes les propriétés du type de paramètres ne sont pas prises en charge par ce formulaire.";
            }
        }
    }

    private static bool IsSupportedProperty(PropertyInfo property)
    {
        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        return propertyType == typeof(bool) || TypeDescriptor.GetConverter(propertyType).CanConvertFrom(typeof(string));
    }

    private string GetInputId(PropertyInfo property) => $"{Action.Name}-{property.Name}";

    private static string GetDisplayName(PropertyInfo property) =>
        property.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? property.Name;

    private static bool IsBoolean(PropertyInfo property) =>
        Nullable.GetUnderlyingType(property.PropertyType) == typeof(bool) || property.PropertyType == typeof(bool);

    private string GetInputType(PropertyInfo property)
    {
        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        return Type.GetTypeCode(propertyType) is TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16
            or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Decimal or TypeCode.Single or TypeCode.Double
            ? "number"
            : "text";
    }

    private string GetStringValue(PropertyInfo property) =>
        Convert.ToString(property.GetValue(Parameters), CultureInfo.CurrentCulture) ?? string.Empty;

    private bool GetBooleanValue(PropertyInfo property) =>
        property.GetValue(Parameters) is true;

    private void UpdateParameter(PropertyInfo property, ChangeEventArgs args)
    {
        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        var value = args.Value;

        try
        {
            object? convertedValue;
            if (IsBoolean(property))
            {
                convertedValue = value is bool booleanValue && booleanValue;
            }
            else if (string.IsNullOrEmpty(Convert.ToString(value, CultureInfo.InvariantCulture)) && Nullable.GetUnderlyingType(property.PropertyType) is not null)
            {
                convertedValue = null;
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(propertyType);
                if (!converter.CanConvertFrom(typeof(string)))
                {
                    ValidationErrors[property.Name] = $"Le type {propertyType.Name} n'est pas pris en charge.";
                    return;
                }

                convertedValue = converter.ConvertFrom(null, CultureInfo.InvariantCulture, Convert.ToString(value, CultureInfo.InvariantCulture));
            }

            property.SetValue(Parameters, convertedValue);
            ValidationErrors.Remove(property.Name);
            ValidateProperty(property);
        }
        catch (Exception exception) when (exception is FormatException or ArgumentException or NotSupportedException)
        {
            ValidationErrors[property.Name] = "La valeur saisie est invalide.";
        }
    }

    private void ValidateProperty(PropertyInfo property)
    {
        var context = new ValidationContext(Parameters!) { MemberName = property.Name };
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateProperty(property.GetValue(Parameters), context, results))
        {
            ValidationErrors[property.Name] = results[0].ErrorMessage ?? "Valeur invalide.";
        }
    }

    private async Task ExecuteAsync()
    {
        FormError = null;
        if (Action.RequiresParameters)
        {
            if (Parameters is null)
            {
                FormError = "Impossible d'initialiser les paramètres de cette action.";
                return;
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(Parameters);
            if (!Validator.TryValidateObject(Parameters, context, results, validateAllProperties: true))
            {
                ValidationErrors.Clear();
                foreach (var result in results)
                {
                    var members = result.MemberNames.ToArray();
                    if (members.Length == 0)
                    {
                        FormError = result.ErrorMessage ?? "Les paramètres sont invalides.";
                        continue;
                    }

                    foreach (var memberName in members)
                    {
                        ValidationErrors[memberName] = result.ErrorMessage ?? "Valeur invalide.";
                    }
                }

                return;
            }
        }

        await Action.ExecuteAsync(Parameters);
    }
}
