using System.Collections.Generic;

namespace Bookong.Application.DTOs
{
    public class FormOptionsDto
    {
        public List<SelectItemDto> Authors { get; set; } = new();
        public List<SelectItemDto> Genres { get; set; } = new();
        public List<SelectItemDto> Kinds { get; set; } = new();
        public List<SelectItemDto> Periods { get; set; } = new();
        public List<SelectItemDto> Publishers { get; set; } = new();
        public List<SelectItemDto> Warehouses { get; set; } = new();
    }
}
