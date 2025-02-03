using LinqToDB.Mapping;

namespace AFRA.Members.Backend.database.Tables;

[Table(Name = "members")]
public class Member {
	[Column(Name = "id"), PrimaryKey, Identity, NotNull] public int       MemberId            { get; set; }
	[Column(Name = "last_name")]                         public string?   LastName            { get; set; }
	[Column(Name = "first_name")]                        public string?   FirstName           { get; set; }
	[Column(Name = "nick")]                              public string?   Nick                { get; set; }
	[Column(Name = "street")]                            public string?   Street              { get; set; }
	[Column(Name = "zip")]                               public string?   Zip                 { get; set; }
	[Column(Name = "city")]                              public string?   City                { get; set; }
	[Column(Name = "country")]                           public string?   Country             { get; set; }
	[Column(Name = "first_address_line")]                public string?   FirstAddressLine    { get; set; }
	[Column(Name = "date_of_birth")]                     public DateTime? DateOfBirth         { get; set; }
	[Column(Name = "email")]                             public string?   Email               { get; set; }
	[Column(Name = "gpg_key_id")]                        public string?   GpgKeyId            { get; set; }
	[Column(Name = "phone")]                             public string?   Phone               { get; set; }
	[Column(Name = "language")]                          public string?   Language            { get; set; }
	[Column(Name = "accounting_query")]                  public string?   AccountingQuery     { get; set; }
	[Column(Name = "iban")]                              public string?   Iban                { get; set; }
	[Column(Name = "bic")]                               public string?   Bic                 { get; set; }
	[Column(Name = "membership_start")]                  public DateTime? MembershipStart     { get; set; }
	[Column(Name = "membership_end")]                    public DateTime? MembershipEnd       { get; set; }
	[Column(Name = "founding")]                          public bool?     Founding            { get; set; }
	[Column(Name = "type_of_payment")]                   public string?   TypeOfPayment       { get; set; }
	[Column(Name = "send_donation_receipt")]             public bool?     SendDonationReceipt { get; set; }
	[Column(Name = "monthly_fee_cents")]                 public int?      MonthlyFeeCents     { get; set; }
	[Column(Name = "note")]                              public string?   Notes               { get; set; }
	[Column(Name = "created_at"), NotNull]               public DateTime  CreatedAt           { get; set; }

	[Column(Name = "updated_at"), NotNull] public DateTime UpdatedAt { get; set; }
	[Column(Name = "non_voting")]          public bool?    NonVoting { get; set; }

	[Association(ThisKey = "MemberId", OtherKey = "MemberId", CanBeNull = true)]
	public IEnumerable<Membership> Memberships { get; set; } = new List<Membership>();
}
