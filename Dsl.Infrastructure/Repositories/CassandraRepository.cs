namespace Dsl.Infrastructure.Repositories
{
    public interface ICassandraRepository
    {
        Cassandra.RowSet Execute(string cql);
    }

    public class CassandraRepository: ICassandraRepository
    {
        private Cassandra.ISession _session { get; set; }

        public CassandraRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public Cassandra.RowSet Execute(string cql)
        {
            return _session.Execute(cql);
        }
    }
}