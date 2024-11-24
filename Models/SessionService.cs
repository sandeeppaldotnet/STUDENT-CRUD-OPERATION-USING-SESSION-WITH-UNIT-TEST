namespace Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models
{
    // ISessionService.cs
    public interface ISessionService
    {
        List<Student> GetStudentList();
        void SetStudentList(List<Student> students);
    }

    // SessionService.cs
    public class SessionService : ISessionService
    {
        private readonly ISession _session;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public List<Student> GetStudentList()
        {
            // Retrieve the student list from session (deserialize it from JSON)
            return _session.GetObjectFromJson<List<Student>>("StudentList") ?? new List<Student>();
        }

        public void SetStudentList(List<Student> students)
        {
            // Save the student list to session (serialize it to JSON)
            _session.SetObjectAsJson("StudentList", students);
        }
    }

}
