using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;

namespace Blazor.FormBuilderComponent;

public class CustomFieldClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        return isValid ? "validField" : "invalidField";
    }
}

public class CustomFieldClassProvider2 : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        if (fieldIdentifier.FieldName == "FullName")
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? "validField" : "invalidField";
        }

        var attributes = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName).GetCustomAttributes();
        if (attributes.Any(x=> x.GetType() == typeof(BrowsableAttribute)))
        {
            return "hidden";
        }

        return string.Empty;
    }
}