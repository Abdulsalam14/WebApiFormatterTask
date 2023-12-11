using System.Linq.Expressions;
using WebApiDemo1225.Entities;
using WebApiDemo1225.Repositories.Abstract;
using WebApiDemo1225.Services.Abstract;

namespace WebApiDemo1225.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Add(Student entity)
        {
            _studentRepository.Add(entity); 
        }

        public void Delete(int id)
        {
            var item = _studentRepository.Get(x=>x.Id==id);
            _studentRepository.Delete(item);
        }

        public Student Get(Expression<Func<Student, bool>> expression)
        {
            return _studentRepository.Get(expression);
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public void Update(Student entity)
        {
            _studentRepository.Update(entity);
        }
    }
}
