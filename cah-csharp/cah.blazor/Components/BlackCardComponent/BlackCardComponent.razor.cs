using cah.models;
using Microsoft.AspNetCore.Components;

namespace cah.blazor.Components.BlackCardComponent
{
    public partial class BlackCardComponent
    {
        [Parameter]
        public BlackCard BlackCard { get; set; }    
    }
}
