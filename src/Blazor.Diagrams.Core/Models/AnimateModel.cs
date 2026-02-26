using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Diagrams.Core.Models
{
    public class AnimateModel
    {
        public string? AttributeName { get; set; }
        public string? From{ get; set; }
        public string? To { get; set; }
        public string? Duration { get; set; }
        public string? RepeatCount {  get; set; } = "indefinite";
    }
}
