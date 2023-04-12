using Microsoft.AspNetCore.Components;
using MatBlazor;
using System.Reflection;
using Blazor.FormBuilderComponent.Extensions;

namespace Blazor.FormBuilderComponent
{
    public partial class FormComponent
    {
        private object? _dataContext;

        // Object model
        [Parameter]
        public object? DataContext
        {
            get => _dataContext;
            set
            {
                _dataContext = value;
                if (_dataContext != null)
                    Properties = _dataContext.GetType().GetProperties();
            }
        }

        // Event target
        [Parameter]
        public ComponentBase? EventTarget { get; set; }

        // List of properties to render
        public PropertyInfo[] Properties { get; set; } = new PropertyInfo[] { };

        /// <summary>
        /// Construct a form fragement using reflected property
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RenderFragment RenderFormElement(PropertyInfo prp) => builder =>
        {
            ArgumentNullException.ThrowIfNull(EventTarget);
            ArgumentNullException.ThrowIfNull(DataContext);
            ArgumentNullException.ThrowIfNull(prp);

            switch (prp.PropertyType.Name)
            {
                case nameof(String):
                    builder.CreateComponent<MatTextField<string>, string>(EventTarget, DataContext, prp);
                    break;
                case nameof(DateTime):
                    builder.CreateComponent<MatDatePicker<DateTime>, DateTime>(EventTarget, DataContext, prp);
                    break;
                case nameof(Int32):
                    builder.CreateComponent<MatNumericUpDownField<int>, int>(EventTarget, DataContext, prp);
                    break;
                case nameof(Decimal):
                    builder.CreateComponent<MatNumericUpDownField<decimal>, decimal>(EventTarget, DataContext, prp);
                    break;
                default:
                    if (prp.PropertyType.BaseType == typeof(Enum))
                    {
                        // When using reflection to call a generic method, we must first use reflection to get the method itself
                        MethodInfo method = typeof(FormBuilderExtensions).GetMethod(nameof(FormBuilderExtensions.CreateEnumComponent));
                        MethodInfo generic = method.MakeGenericMethod(prp.PropertyType);
                        generic.Invoke(null, new object[] { builder, EventTarget, DataContext, prp });
                    }
                    else
                    {
                        builder.CreateComponent<MatTextField<string>, string>(EventTarget, DataContext, prp);
                    }
                    break;
            }
        };

        /// <summary>
        /// Construct a form ValidationMessage using reflected property
        /// </summary>
        /// <param name="prp"></param>
        /// <returns></returns>
        public RenderFragment RenderValidationElement(PropertyInfo prp) => builder =>
        {
            ArgumentNullException.ThrowIfNull(DataContext);
            ArgumentNullException.ThrowIfNull(prp);

            builder.CreateValidationComponent(DataContext, prp);
        };

        /// <summary>
        /// Get DataContext instance value for reflected property
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object this[PropertyInfo property]
        {
            get
            {
                return property.GetValue(DataContext);
            }
        }

        //public EventCallback<EditContext> OnSubmit { get; set; }
    }
}