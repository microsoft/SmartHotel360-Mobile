using System;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;

namespace SmartHotel.Clients.NFC.ViewModels.Base
{
    public abstract class ExtendedBindableObject : BindableObject
    {
        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            var lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                var body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }

            return operand.Member;
        }
    }
}