using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PL_WPF
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
