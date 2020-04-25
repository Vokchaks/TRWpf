using TRWpf.Models;

namespace TRWpf.ViewModels
{
    public class ElementViewModel //: ViewModelBase
    {
        public ElementViewModel(Element element)
        {
            Unit = element;
        }
        public Element Unit { get; set; }
    }
}
