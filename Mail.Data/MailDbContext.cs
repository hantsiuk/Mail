using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Mail.Data
{
	public class MailDbContext : DbContext, IDbContext
	{
		public IEnumerable<TResult> ExecuteStoredProcedure<TResult>(string functionName, IDictionary<string, object> parameters)
		{
			ObjectParameter[] objectParameters = parameters.Select(n => new ObjectParameter(n.Key, n.Value)).ToArray();
			ObjectResult<TResult> objectResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TResult>(functionName, objectParameters);
			
			return objectResult;
		}

		public TResult ExecuteStoredProcedureScalar<TResult>(string functionName, IDictionary<string, object> parameters)
		{
			return ExecuteStoredProcedure<TResult>(functionName, parameters).FirstOrDefault();
		}
	}	
}
