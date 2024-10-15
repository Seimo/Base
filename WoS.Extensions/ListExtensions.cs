using System.Text;

namespace Wos.Extensions
{
    public static class ListExtensions
    {

        public static string Join<T>(this IList<T> list, string joinString)
        {
            if (list == null || list.Count == 0) return string.Empty;
            if (list.Count == 1) return list[0].ToString();

            var result = new StringBuilder();
            result.Append(joinString);

            foreach (var listItem in list)
            {
                result.Append(listItem);
            }

            result.Remove(0, 1);
            return result.ToString();
        }

    }

}
