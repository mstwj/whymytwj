using TailuopaiWeabapi.Table;

namespace TailuopaiWeabapi.Base
{
    public static class MyTools
    {
        public static string GetTableAnswer(string myQuery)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_StandardInfo table_RocreadInfo = new Table_StandardInfo();
                    var firstEntity = context.Myopenaiinfo.FirstOrDefault(e => e.Query == myQuery);
                    if (firstEntity == null)
                    {                        
                        table_RocreadInfo.Id = 0; //(ID 产品表)
                        table_RocreadInfo.Query = myQuery;
                        table_RocreadInfo.Answer = null;
                        context.Myopenaiinfo.Add(table_RocreadInfo);

                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            return "添加成功";
                        }
                        return "添加失败";
                    }
                    Table_StandardInfo Info = (Table_StandardInfo)firstEntity;
                    return Info.Answer;
                }
            }
            catch (Exception ex) 
            {
                return string.Empty; 
            }
        }
    }
}
