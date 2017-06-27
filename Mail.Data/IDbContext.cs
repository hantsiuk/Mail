using System.Collections.Generic;

namespace Mail.Data
{
	public interface IDbContext
	{
		IEnumerable<TResult> ExecuteStoredProcedure<TResult>(string functionName, IDictionary<string, object> parameters);
		TResult ExecuteStoredProcedureScalar<TResult>(string functionName, IDictionary<string, object> parameters);
	}
}
