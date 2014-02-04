using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;

namespace arcmap_layer_search.Utilities
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <remarks></remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <example>
        /// // Example usage
        /// <code>
        /// OnPropertyChanged(() => PropertyName);
        /// </code>
        /// </example>
        /// <param name="execute">The execute.</param>
        /// <remarks></remarks>
        protected void OnPropertyChanged(Expression<Func<object>> execute)
        {
            try
            {
                var obj = execute.Body;
                MemberExpression expr = null;
                if (obj is UnaryExpression)
                {
                    expr = (MemberExpression)((UnaryExpression)execute.Body).Operand;

                }
                else if (obj is MemberExpression)
                {
                    expr = (MemberExpression)execute.Body;

                }
                if (expr != null)
                {
                    var prop = (PropertyInfo)expr.Member;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs(prop.Name));
                }
            }
            catch (Exception e)
            {
                var a = e.Message;
            }
        }
    }
}
