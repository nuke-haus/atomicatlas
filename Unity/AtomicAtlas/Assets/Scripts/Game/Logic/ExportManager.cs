
public interface IExportManager
{
    public void ExportMap();
}

[Injectable(typeof(IExportManager))]
public class ExportManager : IExportManager
{
    public ExportManager()
    {
      
    }

    public void ExportMap()
    {

    }
}
