using Trisatech.MWorkforce.Infrastructure.Interface;
namespace Trisatech.MWorkforce.Infrastructure.Services
{
    public class TextFileReader : ITextFileReader
    {
        public const string DATA_DELIMETER = ",";
        public IEnumerable<Dictionary<string, object>> Read(string path, string delimiter, int totalColumn)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            
            try
            {
                using(StreamReader reader = new StreamReader(path))
                {
                    string line;
                    int row = 0;
                    string[] columnName = new string[totalColumn];

                    while((line = reader.ReadLine()) != null)
                    {
                        string[] arrayData = line.Split(DATA_DELIMETER);
                        Dictionary<string, object> newItem = new Dictionary<string, object>();
                        if(row == 0)
                        {
                            for(int i = 0; i < totalColumn;i++)
                            {
                                columnName[i] = arrayData[i];
                            }
                        }
                        else
                        {
                            for (int i = 0; i < totalColumn; i++)
                            {
                                newItem.Add(columnName[i], arrayData[i]);
                            }

                            if (newItem != null)
                            {
                                result.Add(newItem);
                            }
                        }
                        row++;
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
