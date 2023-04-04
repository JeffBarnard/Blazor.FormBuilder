using MatBlazor;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components.CompilerServices;

namespace Blazor.FormBuilderComponent.Helpers.Extensions
{
    /// <summary>
    /// RenderTreeBuilder extensions to construct form components
    /// </summary>
    public static class FormBuilderExtensions
    {
        /// <summary>
        /// Create an HTML component and bind it to a property
        /// </summary>
        /// <typeparam name="TComponent">Type of UI control</typeparam>
        /// <typeparam name="TDataType">Data type (string, int)</typeparam>
        /// <param name="target"></param>
        /// <param name="model"></param>
        /// <param name="builder"></param>
        /// <param name="prp"></param>
        public static void CreateComponent<TComponent, TDataType>(this RenderTreeBuilder builder, ComponentBase target, object model, PropertyInfo prp)
        {
            var constant = Expression.Constant(model, model.GetType());
            var exp = Expression.Property(constant, prp.Name);

            builder.OpenComponent(0, typeof(TComponent));
            builder.AddAttribute(1, "Value", prp.GetValue(model));
            builder.AddAttribute(2, "EnableTime", true);
            builder.AddAttribute(3, "ValueChanged", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TDataType>(target, EventCallback.Factory.CreateInferred(target, __value => prp.SetValue(model, __value), (TDataType)prp.GetValue(model)))));
            builder.AddAttribute(4, "ValueExpression", Expression.Lambda<Func<TDataType>>(exp));
            builder.AddAttribute(5, "PlaceHolder", prp.Name);
            //if (attrList.DataType == DataType.MultilineText)
            //  builder.AddAttribute(6, "Multiline", true);

            builder.AddExtendedAttributes(prp);
            builder.CloseComponent();
        }

        /// <summary>
        /// Create a SelectItem UI control and set it's selectable values equal to the enum string values
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="builder"></param>
        /// <param name="target"></param>
        /// <param name="model"></param>
        /// <param name="prp"></param>
        /// <param name="exp"></param>
        /// <exception cref="InvalidCastException"></exception>
        public static void CreateEnumComponent<TEnum>(this RenderTreeBuilder builder, ComponentBase target, object model, PropertyInfo prp)
        {
            ArgumentNullException.ThrowIfNull(prp);

            if (typeof(TEnum).BaseType != typeof(Enum))
                throw new InvalidCastException();

            var constant = Expression.Constant(model, model.GetType());
            var exp = Expression.Property(constant, prp.Name);

            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

            builder.OpenComponent(0, typeof(MatSelectItem<TEnum>));
            builder.AddAttribute(1, "DataSource", value:string.Empty);
            builder.AddAttribute(2, "Items", values);
            builder.AddAttribute(3, "ValueChanged", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TEnum>(target, EventCallback.Factory.CreateInferred(target, __value => prp.SetValue(model, __value), (TEnum)prp.GetValue(model)))));
            builder.AddAttribute(4, "ValueExpression", Expression.Lambda<Func<TEnum>>(exp));
            builder.CloseComponent();
        }

        /// <summary>
        /// Create a ValidationMessage HTML component and bind it to a property
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="model"></param>
        /// <param name="prp"></param>
        public static void CreateValidationComponent(this RenderTreeBuilder builder, object model, PropertyInfo prp)
        {
            ArgumentNullException.ThrowIfNull(prp);

            // Create an expression to set the ForExpression - attribute.
            var access = Expression.Property(Expression.Constant(model, model.GetType()), prp);
            var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(prp.PropertyType), access);

            builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(prp.PropertyType));
            builder.AddAttribute(1, "For", lambda);
            builder.CloseComponent();
        }

        /// <summary>
        /// Add extended attributes to the HTML component
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="prp"></param>
        public static void AddExtendedAttributes(this RenderTreeBuilder builder, PropertyInfo prp)
        {
            ArgumentNullException.ThrowIfNull(prp);

            if (prp.CustomAttributes.Any(x => x.AttributeType == typeof(BrowsableAttribute)))
            {
                builder.AddAttribute(6, "class", "hidden");
            }

            if (prp.CustomAttributes.Any(x => x.AttributeType == typeof(PasswordPropertyTextAttribute)))
            {
                builder.AddAttribute(6, "type", "password");
            }

            if (prp.CustomAttributes.Any(x => x.AttributeType == typeof(ReadOnlyAttribute)))
            {
                builder.AddAttribute(6, "readonly", true);
                builder.AddAttribute(6, "disabled", true);
            }
        }
    }
}


//case nameof(Enum):
//if (attrList.CustomDataType == "DropdownList")
//{
//    builder.OpenComponent(0, typeof(MatInputTextComponent<Type>));
//    //builder.AddAttribute(1, "DataSource", countries.GetCountries());
//    //builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((builder2) =>
//    //{
//    //    builder2.AddMarkupContent(6, "\r\n    ");
//    //    builder2.OpenComponent<Syncfusion.Blazor.DropDowns.DropDownListFieldSettings>
//    //  (6);

//    //    builder2.AddAttribute(7, "Value", "Code");
//    //    builder2.AddAttribute(8, "Text", "Name");
//    //    builder2.CloseComponent();
//    //    builder2.AddMarkupContent(9, "\r\n");
//    //}));

//}
//else if (attrList.CustomDataType == "ComboBox")
//{
//    //builder.OpenComponent(0, typeof(Syncfusion.Blazor.DropDowns.SfComboBox<string, Cities>));
//    //builder.AddAttribute(1, "DataSource", cities.GetCities());
//    //builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((builder2) =>
//    //{
//    //    builder2.AddMarkupContent(6, "\r\n    ");
//    //    builder2.OpenComponent<Syncfusion.Blazor.DropDowns.ComboBoxFieldSettings>
//    //  (6);

//    //    builder2.AddAttribute(7, "Value", "Code");
//    //    builder2.AddAttribute(8, "Text", "Name");
//    //    builder2.CloseComponent();
//    //    builder2.AddMarkupContent(9, "\r\n");
//    //}));
//}
//builder.AddAttribute(3, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<System.String>>(EventCallback.Factory.Create<System.String>(this, EventCallback.Factory.CreateInferred(this, __value => prp.SetValue(model, __value), (string)prp.GetValue(model)))));
//builder.AddAttribute(4, "ValueExpression", Expression.Lambda<Func<string>>(exp));