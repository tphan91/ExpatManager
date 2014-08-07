using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ExpatManager.Helper
{
    public static class HtmlDropDownExtensions
    {
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = (value.Equals(selectedValue))
                };

            return htmlHelper.DropDownList(
                name,
                items
                );
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            Type baseEnumType = Enum.GetUnderlyingType(enumType);

            TypeConverter converter = TypeDescriptor.GetConverter(enumType);

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = converter.ConvertToString(value),
                    //Value = value.ToString(),
                    Value = Convert.ChangeType(value, baseEnumType).ToString(),
                    Selected = value.Equals(metadata.Model)
                };

            if (metadata.IsNullableValueType)
            {
                items = SingleEmptyItem.Concat(items);
            }

            return htmlHelper.DropDownListFor(
                expression,
                items
                );
        }

        public static MvcHtmlString EnumDropDownListForX2<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {

            //Type enumType = typeof(TEnum);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            Type actualEnumType = Nullable.GetUnderlyingType(enumType); // handle nulls
            //IEnumerable<TEnum> values = Enum.GetValues(actualEnumType).Cast<TEnum>();
            TEnum prop = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            IEnumerable<SelectListItem> items = from value in values
                                                select
                                                new SelectListItem
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString(),
                                                    Selected = (value.Equals(prop))
                                                };
            if (enumType != actualEnumType)
            {
                items = SingleEmptyItem.Concat(items);
            }
            return SelectExtensions.DropDownListFor(htmlHelper, expression, items);
        }

        public static SelectList ToSelectList<T>(T selectedItem)
        {
            if (!typeof(T).IsEnum) throw new InvalidEnumArgumentException("The specified type is not an enum");

            var selectedItemName = Enum.GetName(typeof(T), selectedItem);
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var fi = typeof(T).GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();

                var enumName = Enum.GetName(typeof(T), item);
                var title = attribute == null ? enumName : ((DescriptionAttribute)attribute).Description;

                var listItem = new SelectListItem
                {
                    Value = enumName,
                    Text = title,
                    Selected = selectedItemName == enumName
                };
                items.Add(listItem);
            }

            return new SelectList(items, "Value", "Text");
        }

        //Helps split the Enum with spaces.
        public class PascalCaseWordSplittingEnumConverter : EnumConverter
        {

            public PascalCaseWordSplittingEnumConverter(Type type)

                : base(type)
            {

            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {

                if (destinationType == typeof(string))
                {

                    string stringValue = (string)base.ConvertTo(context, culture, value, destinationType);

                    stringValue = SplitString(stringValue);

                    return stringValue;

                }

                return base.ConvertTo(context, culture, value, destinationType);

            }

            public string SplitString(string stringValue)
            {

                StringBuilder buf = new StringBuilder(stringValue);

                // assume first letter is upper!

                bool lastWasUpper = true;

                int lastSpaceIndex = -1;

                for (int i = 1; i < buf.Length; i++)
                {

                    bool isUpper = char.IsUpper(buf[i]);

                    if (isUpper & !lastWasUpper)
                    {

                        buf.Insert(i, ' ');

                        lastSpaceIndex = i;

                    }

                    if (!isUpper && lastWasUpper)
                    {

                        if (lastSpaceIndex != i - 2)
                        {

                            buf.Insert(i - 1, ' ');

                            lastSpaceIndex = i - 1;

                        }

                    }

                    lastWasUpper = isUpper;

                }

                return buf.ToString();

            }

        }
    }

    /*
    public static class EnumDropDownList
    {
        public static HtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> modelExpression, string firstElement)
        {
            var typeOfProperty = modelExpression.ReturnType;
            if(!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));

            var enumValues = new SelectList(Enum.GetValues(typeOfProperty));
            return htmlHelper.DropDownListFor(modelExpression, enumValues, firstElement);
        }


        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };

            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
    

    public static class SelectExtensions
    {

        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            // p => p.Foo.Bar().Baz.ToString() => p.Foo OR throw...
            MethodCallExpression methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }
            return expression.Object.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            string inputName = GetInputName(expression);
            var value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);

            return htmlHelper.DropDownList(inputName, ToSelectList(typeof(TProperty)));
        }

        public static SelectList ToSelectList<T>(T selectedItem)
        {
            if (!typeof(T).IsEnum) throw new InvalidEnumArgumentException("The specified type is not an enum");

            var selectedItemName = Enum.GetName(typeof(T), selectedItem);
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var fi = typeof(T).GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();

                var enumName = Enum.GetName(typeof(T), item);
                var title = attribute == null ? enumName : ((DescriptionAttribute)attribute).Description;

                var listItem = new SelectListItem
                {
                    Value = enumName,
                    Text = title,
                    Selected = selectedItemName == enumName
                };
                items.Add(listItem);
            }

            return new SelectList(items, "Value", "Text");
        }
     }
     */
}
