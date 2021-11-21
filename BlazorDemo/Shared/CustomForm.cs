using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDemo.Shared
{
    public class CustomForm : EditForm
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenRegion(0);

            builder.OpenComponent<CascadingValue<CustomForm>>(1);
            builder.AddAttribute(2, "Value", this);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)base.BuildRenderTree);

            builder.CloseComponent();
            builder.CloseRegion();
        }
    }
}
