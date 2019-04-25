using StackExchange.Redis;

namespace NoSqlWebApp.Providers
{
	public interface IRedisDatabaseProvider
	{
		IDatabase GetDatabase();
	}
}