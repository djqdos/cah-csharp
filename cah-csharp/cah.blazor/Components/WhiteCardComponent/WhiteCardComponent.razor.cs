using cah.models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace cah.blazor.Components.WhiteCardComponent
{
    public partial class WhiteCardComponent
    {
        

        [Parameter]
        public WhiteCard WhiteCard { get; set; }


        [Parameter]
        public EventCallback<WhiteCard> OnHandleDragStart { get; set; }


        private async Task HandleDragStart(DragEventArgs e, WhiteCard _selectedCard)
        {
            await OnHandleDragStart.InvokeAsync(_selectedCard);
		}
    }
}
