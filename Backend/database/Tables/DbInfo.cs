using LinqToDB.Mapping;

namespace AFRA.Members.Backend.database.Tables;

[Table(Name = "DbInfo")]
public class DbInfo {
	[Column(Name = "ID"), PrimaryKey, Identity, NotNull] public int Id    { get; set; }
	[Column(Name = "DbVer"), NotNull]                    public int DbVer { get; set; }
}
