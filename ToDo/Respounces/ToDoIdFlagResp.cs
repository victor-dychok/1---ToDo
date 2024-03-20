namespace ToDo.Respounces
{
    public class ToDoIdFlagResp
    {
        public int Id {  get; set; }
        public bool IsDone { get; set; }
        public ToDoIdFlagResp(int id, bool isDone)
        {
            Id = id;
            IsDone = isDone;
        }
    }
}
