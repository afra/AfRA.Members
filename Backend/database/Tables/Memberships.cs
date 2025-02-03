using LinqToDB.Mapping;

namespace AFRA.Members.Backend.database.Tables;

[Table(Name = "memberships")]
public class Membership {
	[Column(Name = "id"), PrimaryKey, Identity, NotNull] public int       MembershipId        { get; set; }
	[Column(Name = "member_id"), NotNull]                public int       MemberId            { get; set; }
	[Column(Name = "start")]                             public DateTime? Start     { get; set; }
	[Column(Name = "end")]                               public DateTime? End       { get; set; }
	[Column(Name = "monthly_fee_cents")]                 public int?      MonthlyFeeCents     { get; set; }
	[Column(Name = "note")]                              public string?   Notes               { get; set; }

	[Column(Name = "created_at"), NotNull]               public DateTime  CreatedAt           { get; set; }
	[Column(Name = "updated_at"), NotNull]               public DateTime  UpdatedAt           { get; set; }

	[Association(ThisKey = "MemberId", OtherKey = "MemberId", CanBeNull = false)]
    public Member Member { get; set; } = null!;
}
