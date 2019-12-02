namespace ACVPCore.Providers
{
	public interface IDependencyProvider
	{
		void Delete(long dependencyID);
		void DeleteAllAttributes(long dependencyID);
		void DeleteAllOELinks(long dependencyID);
		void DeleteAttribute(long attributeID);
		void DeleteOELink(long dependencyID, long oeID);
	}
}