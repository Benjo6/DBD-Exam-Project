using System.Text;

namespace Frontend.Data;

public abstract class BasePrescriptionService
{
    protected string GetPageParameter(int pageCount, int pageSize)
    {
        StringBuilder sb = new("?");

        if (pageCount > 0)
            sb.Append($"Number={pageCount}");
        if (pageSize > 0)
        {
            if (pageCount > 0)
                sb.Append('&');

            sb.Append($"Size={pageSize}");
        }

        return sb.ToString();
    }
}
